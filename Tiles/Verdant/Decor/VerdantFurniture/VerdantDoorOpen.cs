using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
	public class VerdantDoorOpen : ModTile
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
            name.SetDefault("Verdant Door");
            AddMapEntry(new Color(142, 62, 32), name);

            dustType = DustID.Grass;
			disableSmartCursor = true;
			adjTiles = new int[] { TileID.OpenDoor };
			closeDoorID = ModContent.TileType<VerdantDoorClosed>();
		}

        public override bool HasSmartInteract() => true;
        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantDoorItem>());
		}

		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
            player.showItemIcon2 = ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantDoorItem>();
        }
	}
}