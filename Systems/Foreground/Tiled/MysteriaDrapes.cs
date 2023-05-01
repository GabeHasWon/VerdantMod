using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Verdant.Items.Verdant.Blocks.Mysteria;

namespace Verdant.Systems.Foreground.Tiled;

internal class MysteriaDrapes : TiledForegroundItem
{
    private List<byte> _variant = new();
    private short _length = 1;

    public override bool SaveMe => true;

    public MysteriaDrapes() : base(new Point(0, 0), "Textures/MysteriaDrapes", new Point(8, 8), false, true)
    {
        _variant = new() { (byte)Main.rand.Next(4) };
    }

    public MysteriaDrapes(Point tilePosition) : base(tilePosition, "Textures/MysteriaDrapes", new Point(8, 8), false, true)
    {
        _variant = new() { (byte)Main.rand.Next(4) };
    }

    public override void Update()
    {
        base.Update();

        if (Main.rand.NextBool(1500 + (_length * _length * _length)) && !WorldGen.SolidTile((int)(position.X / 16), (int)(position.Y / 16) + _length))
            Grow();
    }

    protected override void CheckAnchor()
    {
        var pos = position.ToTileCoordinates();
        Tile t = Framing.GetTileSafely(pos);

        if (!t.HasTile)
            Kill();
    }

    public void Kill()
    {
        killMe = true;

        var pos = position.ToTileCoordinates();
        var source = new EntitySource_TileBreak(pos.X, pos.Y, "Verdant:MysteriaDrapes");
        Item.NewItem(source, position, 16, 16, ModContent.ItemType<MysteriaDrapesItem>(), Main.rand.Next((int)(_length * 0.5f), _length));
    }

    public void Grow()
    {
        _length++;
        _variant.Add((byte)Main.rand.Next(4));
    }

    public override void Draw()
    {
        for (int i = 0; i < _length; ++i)
        {
            Vector2 pos = position + new Vector2(0, i * 16);
            Color col = Lighting.GetColor(pos.ToTileCoordinates());
            float sine = MathF.Sin((Main.GameUpdateCount * 0.12f) + ((pos.X * 2 + pos.Y * 2) * 0.04f));
            Rectangle source = new(0, 18 * _variant[i], 16, 16);

            if (i < 3)
                sine *= i / 3f;

            pos.X += sine;

            if (i == 0)
                source = new(18, (_variant[i] % 2 * 18), 16, 16);
            else if (i == _length - 1)
                source = new(18, 36 + (_variant[i] % 2 * 18), 16, 16);

            Main.spriteBatch.Draw(Texture.Value, pos - Main.screenPosition, source, col, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }

    public override void Save(TagCompound tag)
    {
        tag.Add("location", position.ToTileCoordinates16());
        tag.Add("variant", _variant);
        tag.Add("length", _length);
    }

    public override void Load(TagCompound tag)
    {
        position = tag.Get<Point16>("location").ToWorldCoordinates(0, 0);
        _variant = tag.GetList<byte>("variant") as List<byte>;
        _length = tag.GetShort("length");
    }
}
