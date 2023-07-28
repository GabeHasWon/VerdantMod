using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
	public class VerdantPiano : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.addTile(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            AddMapEntry(new Color(33, 142, 22), Language.GetText("ItemName.Piano"));
        }

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override void SetDrawPositions(int i, int j, ref int w, ref int offsetY, ref int h, ref short x, ref short y) => offsetY = 2;

        public override bool RightClick(int i, int j)
		{
			int rand = Main.rand.Next(2);
			SoundEngine.PlaySound(new SoundStyle(rand == 0 ? "Verdant/Sounds/Arpiano" : "Verdant/Sounds/SoftMelodyPiano") with { PitchVariance = 0.05f }, new Vector2(i, j) * 16);
			return true;
        }

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.cursorItemIconText = "Play";
            Main.LocalPlayer.cursorItemIconID = -1;
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.cursorItemIconEnabled = true;
        }
    }
}