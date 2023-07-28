using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
	public class VerdantDoorOpen : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileNoSunLight[Type] = true;

            TileID.Sets.HousingWalls[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CloseDoorID[Type] = ModContent.TileType<VerdantDoorClosed>();
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileHelper.OpenDoorData(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(33, 142, 22), name);
            RegisterItemDrop(ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantDoorItem>());
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);

            DustType = DustID.Grass;
			AdjTiles = new int[] { TileID.OpenDoor };
		}

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

		public override void MouseOver(int i, int j) {
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantDoorItem>();
        }
	}
}