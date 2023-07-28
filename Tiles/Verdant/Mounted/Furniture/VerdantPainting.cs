using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Verdant.Items.Verdant.Blocks.Misc;
using Terraria.DataStructures;

namespace Verdant.Tiles.Verdant.Mounted.Furniture
{
	public abstract class VerdantPainting<T> : ModTile where T : ModItem
	{
        protected abstract int Width { get; }
        protected abstract int Height { get; }

        public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Width = Width;
			TileObjectData.newTile.Height = Height;
            TileObjectData.newTile.CoordinateHeights = new int[Height];

            for (int i = 0; i < Height; ++i)
                TileObjectData.newTile.CoordinateHeights[i] = 16;

			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);

			DustType -= 1;

			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Painting");
			AddMapEntry(new Color(109, 81, 69), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 3 : 7;
    }

    internal class ApotheoticPainting : VerdantPainting<ApotheoticPaintingItem>
    {
        protected override int Width => 4;
        protected override int Height => 4;
    }

    internal class LightbulbPainting : VerdantPainting<LightbulbPaintingItem>
    {
        protected override int Width => 2;
        protected override int Height => 2;
    }
}