﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Verdant.Systems.Foreground.Parallax;

public class ZipvineEntity : ParallaxedFGItem
{
    public virtual Vector2 HoldOffset => new(-6, 18);
    public virtual float ClimbSpeed => 0.5f;
    public virtual byte VineLength => 16;
    public virtual int VineId => 0;

    public override bool SaveMe => true;

    public bool HasNext => nextVine is not null;
    public bool HasPrior => priorVine is not null;

    protected int Frame
    {
        get
        {
            if (HasNext && HasPrior)
                return 1;
            else if (HasNext || HasPrior)
                return 0;
            return 2;
        }
    }

    public Rectangle Hitbox => new((int)position.X, (int)position.Y, 12, 12);

    public int whoAmI = 0;
    public ZipvineEntity nextVine = null;
    public ZipvineEntity priorVine = null;

    protected int _lifeTime = 0;

    public ZipvineEntity() : base(Vector2.Zero, Vector2.Zero, 0, "Parallax/ZipvineBasic")
    {
    }

    public ZipvineEntity(string tex = "Parallax/ZipvineBasic") : base(Vector2.Zero, Vector2.Zero, 0, tex)
    {
    }

    public ZipvineEntity(Vector2 position, long priorWho = -1, long nextWho = -1, string tex = "Parallax/ZipvineBasic") : base(position, Vector2.Zero, 1f, tex)
    {
        //DateTime.MinValue.Ticks
        whoAmI = (int)(DateTime.UtcNow.Ticks % int.MaxValue);

        if (priorWho != -1)
            priorVine = ForegroundManager.PlayerLayerItems.First(x => x is ZipvineEntity zip && zip.whoAmI == priorWho) as ZipvineEntity;

        if (nextWho != -1)
            nextVine = ForegroundManager.PlayerLayerItems.First(x => x is ZipvineEntity zip && zip.whoAmI == nextWho) as ZipvineEntity;
    }

    public override void Update() => _lifeTime++;

    public void Kill()
    {
        for (int i = 0; i < 3; ++i)
            Dust.NewDust(position + Hitbox.Size() / 3f, Hitbox.Width / 2, Hitbox.Height / 2, DustID.Grass, 0, 0);

        killMe = true;

        if (priorVine is not null) // Remove me from prior vine and next vine
            priorVine.nextVine = null;

        if (nextVine is not null)
            nextVine.priorVine = null;
    }

    public override void Draw()
    {
        int dir = (((whoAmI % 2) + (whoAmI % 9) + (whoAmI % 3)) % int.MaxValue); //"randomize" direction
        SpriteEffects effects = (dir % 2 == 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        drawColor = Lighting.GetColor(position.ToTileCoordinates());
        drawPosition = position;
        rotation = 0f;

        if (Frame == 0)
        {
            if (HasNext)
                rotation = DirectionTo(nextVine.Center).ToRotation() - MathHelper.PiOver2;
            else
                rotation = DirectionTo(priorVine.Center).ToRotation() - MathHelper.PiOver2;
        }
        else if (Frame == 1)
            rotation = MathHelper.Lerp(DirectionTo(nextVine.Center).ToRotation(), DirectionTo(priorVine.Center).ToRotation(), 0.5f);

        float offset = (float)Math.Sin((_lifeTime + whoAmI % 500000 * 6) * 0.03f) * 0.25f;
        var frame = new Rectangle(dir % 3 * 16, Frame * 20, 14, 18);

        if (!killMe)
            Main.spriteBatch.Draw(Texture.Value, drawPosition - Main.screenPosition, frame, drawColor, rotation + offset, new Vector2(7, 9), 1f, effects, 0);
    }

    public override void Save(TagCompound tag)
    {
        tag.Add("position", position);
        tag.Add("who", whoAmI);
        tag.Add("priorWho", priorVine is null ? -1 : priorVine.whoAmI);
    }

    public override void Load(TagCompound tag)
    {
        position = tag.Get<Vector2>("position");
        whoAmI = tag.GetInt("who");
        long priorWho = tag.GetInt("priorWho");

        if (priorWho >= 0)
        {
            var prior = ForegroundManager.PlayerLayerItems.FirstOrDefault(x => x is ZipvineEntity zip && zip.whoAmI == priorWho);

            if (prior is not ZipvineEntity)
                return;

            priorVine = prior as ZipvineEntity;
            priorVine.nextVine = this;
        }
    }
}