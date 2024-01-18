using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetEasy;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Verdant.Dusts;
using Verdant.Items.Verdant.Blocks.TileEntity;
using Verdant.Items.Verdant.Materials;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.TileEntities.Puff;

internal class Pickipuff : DrawableTE
{
    private float RealFactor
    {
        get
        {
            double sine = Math.Sin(timer * 0.008f * (length / 300d));
            float val = targetFactor + (float)(-Math.Pow(sine, 2) * (0.02 * (length / 80d)));
            val = MathHelper.Clamp(val, 0, 1);
            return val;
        }
    }

    private Vector2 PuffLocation
    {
        get
        {
            float len = RealFactor * length;
            if (len < 10)
                len = 10;
            return World + new Vector2(6, len);
        }
    }

    private Rectangle PuffHitbox => new((int)PuffLocation.X, (int)PuffLocation.Y, 18, 20);

    private const int Harvested = 4;
    private const int FullyFrightened = 3;
    private const int AlmostHidden = 2;
    private const int Retreating = 1;
    private const int Chilling = 0;

    internal float factor = 0;
    internal float targetFactor = 0;
    internal float factorTime = 0;
    internal int puffState = 0;
    internal int length = 0;
    private int timer = 0;

    protected override Point Size => new(18, (int)(targetFactor * length) + 22);

    public override void SaveData(TagCompound tag)
    {
        if (puffState != Chilling)
            tag.Add("state", puffState);

        tag.Add("length", length);
    }

    public override void LoadData(TagCompound tag)
    {
        if (tag.ContainsKey("state"))
            puffState = tag.GetInt("state");

        length = tag.GetInt("length");
    }

    public override void NetSend(BinaryWriter writer)
    {
        writer.Write(length);
        writer.Write(factor);
        writer.Write(factorTime);
    }

    public override void NetReceive(BinaryReader reader)
    {
        length = reader.ReadInt32();
        factor = reader.ReadSingle();
        factorTime = reader.ReadSingle();
    }

    public override bool IsTileValidForEntity(int x, int y)
    {
        Tile tile = Main.tile[x, y];
        return tile.HasTile && tile.TileType == ModContent.TileType<VerdantGrassLeaves>();
    }

    public override void Update()
    {
        timer++;

        if (length == 0)
        {
            length = Main.rand.Next(26, 34) * 10;
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
        }

        Tile anchor = Main.tile[Position.X, Position.Y];
        if (!anchor.HasTile || anchor.TileType != ModContent.TileType<VerdantGrassLeaves>())
        {
            Kill(Position.X, Position.Y);
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
        }

        Player nearest = Main.player[Player.FindClosest(World, 2, 2)];

        if (!nearest.dead && puffState != Harvested)
            ControlRetraction(nearest);

        CheckPickup();
        SpawnDust();

        if (Main.netMode == NetmodeID.Server)
            new PickipuffModule(Position.X, Position.Y, length, factor, puffState, targetFactor).Send(runLocally: false);
    }

    private void SpawnDust()
    {
        if (puffState != Harvested && Main.rand.NextBool(600 + (50 * puffState)))
            Dust.NewDust(PuffLocation, 16, 16, ModContent.DustType<PuffDust>(), Scale: Main.rand.NextFloat(0.5f, 0.9f));
    }

    private void CheckPickup()
    {
        if (puffState < FullyFrightened)
        {
            for (int i = 0; i < Main.maxPlayers; ++i)
            {
                Player p = Main.player[i];

                if (p.active && !p.dead && p.Hitbox.Intersects(PuffHitbox))
                {
                    p.QuickSpawnItem(p.GetSource_Loot("Verdant:Pickipuff"), ModContent.ItemType<PuffMaterial>(), puffState == Chilling ? 4 : 2);
                    puffState = Harvested;
                    factor = 0;

                    for (int j = 0; j < 4; ++j)
                        Dust.NewDust(PuffLocation, 16, 16, ModContent.DustType<PuffDust>(), Scale: Main.rand.NextFloat(0.5f, 0.9f));
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
                    break;
                }
            }
        }

        if (puffState == Harvested)
        {
            targetFactor = MathHelper.Lerp(targetFactor, factor, 0.02f);

            if (Main.rand.NextBool(3000))
            {
                factor = 1;
                puffState = FullyFrightened;

                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
        }
    }

    private void ControlRetraction(Player nearest)
    {
        if (nearest.DistanceSQ(World) < 500 * 500)
        {
            if (puffState == FullyFrightened && factor < 0.05f)
                factorTime = 120;

            float magnitude = nearest.velocity.LengthSquared();

            if (magnitude > 12 * 12)
            {
                factor = 0;
                factorTime = 120;
                puffState = FullyFrightened;

                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
            }
            else if (magnitude > 3 * 3)
            {
                factor -= magnitude / (12 * 12) * 0.05f;
                factorTime = MathHelper.Clamp(factorTime + 5, 0, 120);

                if (factor > 0.95f)
                    puffState = Chilling;
                else if (factor > 0.75f)
                    puffState = Retreating;
                else if (factor > 0.5f)
                    puffState = AlmostHidden;
                else if (factor > 0.25f)
                    puffState = FullyFrightened;
            }
        }

        factor = MathHelper.Clamp(factor, 0, 1);
        targetFactor = MathHelper.Lerp(targetFactor, factor, 0.05f);

        if (--factorTime < 0)
        {
            factor = MathHelper.Lerp(factor, 1, 0.2f);

            if (factor > 0.9f && factor <= 0.93f && puffState > AlmostHidden)
                puffState = AlmostHidden;

            if (factor > 0.93f && factor <= 0.96f && puffState > Retreating)
                puffState = Retreating;

            if (factor > 0.96 && puffState > Chilling)
                puffState = Chilling;
        }
    }

    public override void OnKill()
    {
        int type = puffState == Harvested ? ModContent.ItemType<HarvestedPickipuffItem>() : ModContent.ItemType<PickipuffItem>();
        Item.NewItem(new EntitySource_TileBreak(Position.X, Position.Y, "Verdant:Pickipuff"), World, type, 1);
    }

    public void SetToHarvested() => puffState = Harvested;

    internal override void Draw(SpriteBatch spriteBatch)
    {
        Texture2D tex = ModContent.Request<Texture2D>("Verdant/Tiles/TileEntities/Puff/Pickipuff").Value;
        Texture2D vine = ModContent.Request<Texture2D>("Verdant/Tiles/TileEntities/Puff/PickipuffVine").Value;

        Vector2 offScreen = Vector2.Zero;// Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
        Vector2 basePos = World - Main.screenPosition + offScreen - new Vector2(6, 0);

        int vines = length / 10;
        for (int i = 0; i < vines; ++i)
        {
            float pointOnVine = (float)(i / (float)vines) * length * RealFactor;
            var drawPos = basePos + new Vector2(0, pointOnVine);
            spriteBatch.Draw(vine, drawPos, new Rectangle(0, (i % 4) * 10, 18, 10), Lighting.GetColor((drawPos + Main.screenPosition).ToTileCoordinates()), 0f, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
        }

        spriteBatch.Draw(tex, PuffLocation - Main.screenPosition, new Rectangle(0, 20 * puffState, 18, 20), Lighting.GetColor(PuffLocation.ToTileCoordinates()), 0f, new Vector2(12, 0), 1f, SpriteEffects.None, 0f);
    }
}

[Serializable]
public class PickipuffModule : Module
{
    private int x;
    private int y;
    private int length;
    private float factor;
    private int puffState;
    private float targetFactor;

    public PickipuffModule(int x, int y, int length, float factor, int puffState, float targetFactor)
    {
        this.x = x;
        this.y = y;
        this.length = length;
        this.factor = factor;
        this.puffState = puffState;
        this.targetFactor = targetFactor;
    }

    protected override void Receive()
    {
        if (Main.netMode != NetmodeID.Server)
        {
            if (!TileEntity.ByPosition.TryGetValue(new Point16(x, y), out var entity))
                return;

            var puff = entity as Pickipuff;
            puff.length = length;
            puff.factor = factor;
            puff.puffState = puffState;
            puff.targetFactor = targetFactor;
        }
    }
}
