using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture
{
	public class LushDoorOpen : ModTile
	{
		public override void SetDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileNoSunLight[Type] = true;

            TileID.Sets.HousingWalls[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;

            TileHelper.OpenDoorData(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Lush Door");
            AddMapEntry(new Color(142, 62, 32), name);

            dustType = DustID.t_BorealWood;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.OpenDoor };
			closeDoorID = TileType<LushDoorClosed>();
		}

        public override bool HasSmartInteract() => true;
        public override void NumDust(int i, int j, bool fail, ref int num) => num = 1;

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			Item.NewItem(i * 16, j * 16, 32, 48, ItemType<Items.Verdant.Blocks.LushWood.LushWoodDoorItem>());
		}

		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
            player.showItemIcon2 = ItemType<Items.Verdant.Blocks.LushWood.LushWoodDoorItem>();
        }
	}
}