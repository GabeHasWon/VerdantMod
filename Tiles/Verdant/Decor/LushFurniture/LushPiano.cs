using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture
{
	public class LushPiano : ModTile
	{
		public override void SetDefaults()
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
			disableSmartCursor = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) => offsetY = 2;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			//Terraria.Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Blocks.Furniture.Reach.ReachPiano>());
		}

        public override bool NewRightClick(int i, int j)
        {
            int rand = Main.rand.Next(2);
            switch (rand)
            {
                case 0:
                    Main.PlaySound(SoundLoader.customSoundType, i * 16, j * 16, mod.GetSoundSlot(SoundType.Custom, "Sounds/Arpiano"), 1f, Main.rand.NextFloat(-0.05f, 0.05f));
                    break;
                default:
                    Main.PlaySound(SoundLoader.customSoundType, i * 16, j * 16, mod.GetSoundSlot(SoundType.Custom, "Sounds/SoftMelodyPiano"), 1f, Main.rand.NextFloat(-0.05f, 0.05f));
                    break;
            }
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.showItemIconText = "Play";
            Main.LocalPlayer.showItemIcon2 = -1;
            Main.LocalPlayer.noThrow = 2;
            Main.LocalPlayer.showItemIcon = true;
        }
    }
}