using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Mysteria.Furniture;

namespace Verdant.Tiles.Verdant.Decor.MysteriaFurniture;

public class MysteriaDresser : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolidTop[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileContainer[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileID.Sets.HasOutlines[Type] = true;
        TileID.Sets.BasicDresser[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.Origin = new Point16(1, 1);
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
        TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
        TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
        TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
        TileObjectData.addTile(Type);

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

        LocalizedText name = CreateMapEntryName();
        // name.SetDefault("Mysteria Dresser");
        AddMapEntry(new Color(124, 93, 68), name);

        DustType = DustID.t_BorealWood;
        AdjTiles = new int[] { TileID.Dressers };
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

    public override bool RightClick(int i, int j)
    {
        Player player = Main.LocalPlayer;
        int left = Main.tile[i, j].TileFrameX / 18;
        left %= 3;
        left = i - left;
        int top = j - Main.tile[i, j].TileFrameY / 18;
        if (Main.tile[i, j].TileFrameY == 0)
        {
            Main.CancelClothesWindow(true);
            Main.mouseRightRelease = false;
            player.CloseSign();
            player.SetTalkNPC(-1);
            Main.npcChatCornerItem = 0;
            Main.npcChatText = "";
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
                player.editedChestName = false;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (left == player.chestX && top == player.chestY && player.chest != -1)
                {
                    player.chest = -1;
                    Recipe.FindRecipes();
                    SoundEngine.PlaySound(SoundID.MenuClose);
                }
                else
                {
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                player.piggyBankProjTracker.Clear();
                player.voidLensChest.Clear();
                int chestIndex = Chest.FindChest(left, top);
                if (chestIndex != -1)
                {
                    Main.stackSplit = 600;
                    if (chestIndex == player.chest)
                    {
                        player.chest = -1;
                        Recipe.FindRecipes();
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    else if (chestIndex != player.chest && player.chest == -1)
                    {
                        player.OpenChest(left, top, chestIndex);
                        SoundEngine.PlaySound(SoundID.MenuOpen);
                    }
                    else
                    {
                        player.OpenChest(left, top, chestIndex);
                        SoundEngine.PlaySound(SoundID.MenuTick);
                    }
                    Recipe.FindRecipes();
                }
            }
        }
        else
        {
            Main.playerInventory = false;
            player.chest = -1;
            Recipe.FindRecipes();
            player.SetTalkNPC(-1);
            Main.npcChatCornerItem = 0;
            Main.npcChatText = "";
            Main.interactedDresserTopLeftX = left;
            Main.interactedDresserTopLeftY = top;
            Main.OpenClothesWindow();
        }
        return true;
    }

    // This is not a hook, this is just a normal method used by the MouseOver and MouseOverFar hooks to avoid repeating code.
    public static void MouseOverNearAndFarSharedLogic(Player player, int i, int j)
    {
        Tile tile = Main.tile[i, j];
        int left = i;
        int top = j;
        left -= tile.TileFrameX % 54 / 18;
        if (tile.TileFrameY % 36 != 0)
        {
            top--;
        }
        int chestIndex = Chest.FindChest(left, top);
        player.cursorItemIconID = -1;
        if (chestIndex < 0)
        {
            player.cursorItemIconText = Language.GetTextValue("LegacyDresserType.0");
        }
        else
        {
            string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY); // This gets the ContainerName text for the currently selected language

            if (Main.chest[chestIndex].name != "")
                player.cursorItemIconText = Main.chest[chestIndex].name;
            else
                player.cursorItemIconText = defaultName;

            if (player.cursorItemIconText == defaultName)
            {
                player.cursorItemIconID = ModContent.ItemType<MysteriaDresserItem>();
                player.cursorItemIconText = "";
            }
        }

        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
    }

    public override void MouseOverFar(int i, int j)
    {
        Player player = Main.LocalPlayer;
        MouseOverNearAndFarSharedLogic(player, i, j);

        if (player.cursorItemIconText == "")
        {
            player.cursorItemIconEnabled = false;
            player.cursorItemIconID = 0;
        }
    }

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;
        MouseOverNearAndFarSharedLogic(player, i, j);

        if (Main.tile[i, j].TileFrameY > 0)
        {
            player.cursorItemIconID = ItemID.FamiliarShirt;
            player.cursorItemIconText = "";
        }
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Chest.DestroyChest(i, j);
}