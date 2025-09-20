using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.VerdantFurniture;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture;

public class VerdantDoorOpen : ModTile
{
    public override void SetStaticDefaults()
    {
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
        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
        RegisterItemDrop(ModContent.ItemType<VerdantDoorItem>());

        DustType = DustID.Grass;
        AdjTiles = [TileID.OpenDoor];
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;
        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = ModContent.ItemType<VerdantDoorItem>();
    }
}