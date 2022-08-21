using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
    internal class VerdantChandelierLarge : ModTile
    {
        public override void SetStaticDefaults()
        {
            // Main.tileFlame[Type] = true; This breaks it.
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.WaterDeath = false;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidBottom | AnchorType.SolidTile, 1, 1);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            AddMapEntry(new Color(33, 124, 22), Language.GetText("MapObject.FloorLamp"));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantChandelierLargeItem>());

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int topX = i - tile.TileFrameX / 18 % 3;
            int topY = j - tile.TileFrameY / 18 % 3;
            short frameAdjustment = (short)(Framing.GetTileSafely(topX, topY).TileFrameX > 0 ? -54 : 54);
            for (int k = 0; k < 3; ++k)
            {
                for (int b = 0; b < 3; ++b)
                {
                    Main.tile[topX + k, topY + b].TileFrameX += frameAdjustment;
                    Wiring.SkipWire(topX + k, topY + b);
                }
            }
            NetMessage.SendTileSquare(-1, i, topY + 1, 2, TileChangeType.None);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Vector3 light = new Vector3(0.5f, 0.16f, 0.30f) * 2;
            if (Framing.GetTileSafely(i, j).TileFrameX < 54 && Framing.GetTileSafely(i, j).TileFrameY >= 18)
            {
                r = light.X;
                g = light.Y;
                b = light.Z;
            }
        }
    }
}
