using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture
{
	public class LushClock : ModTile
	{
		public override void SetStaticDefaults()
        {
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

            TileID.Sets.DisableSmartCursor[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.Origin = new Point16(0, 4);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(114, 69, 39), Language.GetText("ItemName.GrandfatherClock"));

			AdjTiles = new int[] { TileID.GrandfatherClocks };
		}

		public override bool RightClick(int x, int y)
        {
            TileHelper.PrintTime(Main.time);
            return true;
		}

		public override void NearbyEffects(int i, int j, bool closer)
        {
			if (closer)
				Main.SceneMetrics.HasClock = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
	}
}