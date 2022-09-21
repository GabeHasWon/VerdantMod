using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture
{
	public class LushPiano : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 2;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Lush Piano");
			AddMapEntry(new Color(179, 146, 107), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			//Terraria.Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Blocks.Furniture.Reach.ReachPiano>());
		}

        public override bool RightClick(int i, int j)
        {
            int rand = Main.rand.Next(2);
            SoundEngine.PlaySound(new SoundStyle(rand == 0 ? "Verdant/Sounds/Arpiano" : "Verdant/Sounds/SoftMelodyPiano") with { PitchVariance = 0.05f, Volume = 0.8f }, new Vector2(i, j) * 16);
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