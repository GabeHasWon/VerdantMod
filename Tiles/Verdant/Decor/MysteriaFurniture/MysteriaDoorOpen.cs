using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Dusts;
using Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

namespace Verdant.Tiles.Verdant.Decor.MysteriaFurniture;

public class MysteriaDoorOpen : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileSolid[Type] = false;
        Main.tileLavaDeath[Type] = true;
        Main.tileNoSunLight[Type] = true;

        TileID.Sets.HousingWalls[Type] = true;
        TileID.Sets.HasOutlines[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;
        TileID.Sets.CloseDoorID[Type] = ModContent.TileType<MysteriaDoorClosed>();

        TileHelper.OpenDoorData(Type);

        LocalizedText name = CreateMapEntryName();
        AddMapEntry(new Color(124, 93, 68), name);
        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
        RegisterItemDrop(ModContent.ItemType<MysteriaDoorItem>());

        DustType = ModContent.DustType<MysteriaWoodDust>();
        AdjTiles = [TileID.OpenDoor];
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
    public override void NumDust(int i, int j, bool fail, ref int num) => num = 1;

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;
        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = ModContent.ItemType<Items.Verdant.Blocks.Mysteria.Furniture.MysteriaDoorItem>();
    }
}