using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using System;
using System.Runtime.CompilerServices;

namespace Verdant.Tiles.Verdant.Basic.Plants;

internal class PetalPlant : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileWaterDeath[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Origin = new Point16(0, 0);
        TileObjectData.newTile.WaterDeath = false;
        TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
        TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidBottom | AnchorType.SolidTile, 2, 1);
        TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
        TileObjectData.newTile.RandomStyleRange = 3;
        TileObjectData.newTile.DrawYOffset = -2;
        TileObjectData.addTile(Type);

        DustType = DustID.JungleGrass;

        AddMapEntry(new Color(90, 104, 79));
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;

    public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
    {
        if (Main.rand.NextBool(60) && tileFrameY % 36 == 0)
            Dust.NewDust(new Vector2(i, j).ToWorldCoordinates(0, 0), 8, 8, ModContent.DustType<PetalPlantPollen>());
    }

    private struct PollenInformation(float sine, Point16 sineOffset)
    {
        public readonly float SineStrength = sine;

        public int Timer;
        public Point16 SineOffset = sineOffset;
    }

    private class PetalPlantPollen : ModDust
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ref PollenInformation Data(Dust dust) => ref Unsafe.Unbox<PollenInformation>(dust.customData); 

        public override void OnSpawn(Dust dust)
        {
            dust.noLightEmittence = true;
            dust.frame = new Rectangle(0, 0, 6, 6);
            dust.velocity.Y = Main.rand.NextFloat(0.6f, 1.1f);
            dust.customData = new PollenInformation(Main.rand.NextFloat(0.05f, 0.2f), new Point16(Main.rand.Next(0, 1500), Main.rand.Next(0, 1500)));
        }

        public override bool Update(Dust dust)
        {
            ref PollenInformation data = ref Data(dust);
            dust.velocity.X = MathF.Sin((data.Timer++ + data.SineOffset.X) * data.SineStrength);
            dust.position += dust.velocity * new Vector2(1, MathF.Sin((data.Timer + data.SineOffset.Y) * data.SineStrength * 0.4f) * 0.15f + 1);
            dust.rotation = dust.velocity.X * 0.7f;

            Tile tile = Main.tile[dust.position.ToTileCoordinates16()];

            if (tile.HasTile && Main.tileSolid[tile.TileType])
                dust.active = false;

            return false;
        }
    }
}