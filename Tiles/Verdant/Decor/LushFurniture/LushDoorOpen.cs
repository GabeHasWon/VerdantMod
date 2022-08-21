using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture
{
	public class LushDoorOpen : ModTile
	{
		public override void SetStaticDefaults() {
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

            DustType = DustID.t_BorealWood;
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { TileID.OpenDoor };
			CloseDoorID = ModContent.TileType<LushDoorClosed>();
		}

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
        public override void NumDust(int i, int j, bool fail, ref int num) => num = 1;

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Verdant.Blocks.LushWood.LushWoodDoorItem>());
		}

		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Verdant.Blocks.LushWood.LushWoodDoorItem>();
        }
	}
}