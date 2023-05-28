using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Localization;
using Verdant.Items.Verdant.Blocks.VerdantFurniture;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
    internal abstract class VerdantHungTableBase<TItem> : ModTile where TItem : ModItem
    {
        public const int ChainLength = 22;

        public abstract string LeafType { get; }
        public virtual bool Lightless => true;

        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.AnchorTop = new AnchorData(Terraria.Enums.AnchorType.AlternateTile, 1, 1);
            TileObjectData.newTile.AnchorAlternateTiles = new int[] { ModContent.TileType<VerdantStrongVine>() };
            QuickTile.SetMultiLocalized(this, 3, 2, DustID.Grass, SoundID.Grass, false, new Color(33, 142, 22), false, false, false, Language.GetText("MapObject.Table"));

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            Main.tileCut[Type] = false;
            Main.tileSolidTop[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileFrameImportant[Type] = true;

            if (!Lightless)
                Main.tileLighted[Type] = true;

            AdjTiles = new int[] { TileID.Tables };
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 5;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            (r, g, b) = (0, 0, 0);
            if (Framing.GetTileSafely(i, j).TileFrameX == 18 && Framing.GetTileSafely(i, j).TileFrameY == 18)
                (r, g, b) = (0.1f, 0.03f, 0.06f);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i * 16, j * 16), ModContent.ItemType<TItem>(), 1);

            if (Main.netMode != NetmodeID.Server)
            {
                for (int v = 0; v < 4; ++v)
                {
                    Vector2 off = new Vector2(Main.rand.Next(54), Main.rand.Next(32));
                    int type = Main.rand.NextBool(2) ? Mod.Find<ModGore>("LushLeaf").Type : Mod.Find<ModGore>(LeafType).Type;
                    Gore.NewGore(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16 + off, new Vector2(0), type, 1);
                }
            }
        }
    }

    internal class VerdantHungTable_Red : VerdantHungTableBase<VerdantHungTableBlock_Red>
    {
        public override string LeafType => "RedPetalFalling";
    }

    internal class VerdantHungTable_RedLightless : VerdantHungTableBase<VerdantHungTableBlock_RedLightless>
    {
        public override string LeafType => "RedPetalFalling";
        public override bool Lightless => true;
    }

    internal class VerdantHungTable_Pink : VerdantHungTableBase<VerdantHungTableBlock_Pink>
    {
        public override string LeafType => "PinkPetalFalling";
    }

    internal class VerdantHungTable_PinkLightless : VerdantHungTableBase<VerdantHungTableBlock_PinkLightless>
    {
        public override string LeafType => "PinkPetalFalling";
        public override bool Lightless => true;
    }
}