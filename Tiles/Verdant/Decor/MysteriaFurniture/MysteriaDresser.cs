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

        ModTranslation name = CreateMapEntryName();
        name.SetDefault("Mysteria Dresser");
        AddMapEntry(new Color(124, 93, 68), name);

        DustType = DustID.t_BorealWood;
        AdjTiles = new int[] { TileID.Dressers };

        ContainerName.SetDefault("Mysteria Dresser");
        DresserDrop = ModContent.ItemType<Items.Verdant.Blocks.Mysteria.Furniture.MysteriaDresserItem>();
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

    public override bool RightClick(int i, int j)
    {
        Player player = Main.LocalPlayer;
        if (Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY == 0)
        {
            Main.CancelClothesWindow(true);
            Main.mouseRightRelease = false;
            int left = Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameX / 18;
            left %= 3;
            left = Player.tileTargetX - left;
            int top = Player.tileTargetY - (Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY / 18);
            if (player.sign > -1)
            {
                SoundEngine.PlaySound(SoundID.MenuClose);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = string.Empty;
            }
            if (Main.editChest)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = string.Empty;
            }
            if (player.editedChestName)
            {
                NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
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
                    NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top, 0f, 0f, 0, 0, 0);
                    Main.stackSplit = 600;
                }
            }
            else
            {
                player.CloseSign();
                int chestInd = Chest.FindChest(left, top);
                if (chestInd != -1)
                {
                    Main.stackSplit = 600;
                    if (chestInd == player.chest)
                    {
                        player.chest = -1;
                        Recipe.FindRecipes();
                        SoundEngine.PlaySound(SoundID.MenuClose);
                    }
                    else if (chestInd != player.chest && player.chest == -1)
                    {
                        player.chest = chestInd;
                        Main.playerInventory = true;
                        Main.recBigList = false;
                        SoundEngine.PlaySound(SoundID.MenuOpen);
                        player.chestX = left;
                        player.chestY = top;
                    }
                    else
                    {
                        player.chest = chestInd;
                        Main.playerInventory = true;
                        Main.recBigList = false;
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        player.chestX = left;
                        player.chestY = top;
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
            Main.interactedDresserTopLeftX = Player.tileTargetX;
            Main.interactedDresserTopLeftY = Player.tileTargetY;
            Main.OpenClothesWindow();
        }
        return true;
    }

    public override void MouseOverFar(int i, int j)
    {
        Player player = Main.LocalPlayer;
        Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
        int left = Player.tileTargetX;
        int top = Player.tileTargetY;
        left -= tile.TileFrameX % 54 / 18;
        if (tile.TileFrameY % 36 != 0)
            top--;
        int chestIndex = Chest.FindChest(left, top);
        player.cursorItemIconID = -1;
        if (chestIndex < 0)
            player.cursorItemIconText = Language.GetTextValue("LegacyDresserType.0");
        else
        {
            if (Main.chest[chestIndex].name != "")
                player.cursorItemIconText = Main.chest[chestIndex].name;
            else
                player.cursorItemIconText = ContainerName.GetDefault();

            if (player.cursorItemIconText == ContainerName.GetDefault())
            {
                player.cursorItemIconID = DresserDrop;
                player.cursorItemIconText = "";
            }
        }
        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        if (player.cursorItemIconText == "")
        {
            player.cursorItemIconEnabled = false;
            player.cursorItemIconID = 0;
        }
    }

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;
        Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
        int left = Player.tileTargetX;
        int top = Player.tileTargetY;
        left -= tile.TileFrameX % 54 / 18;
        if (tile.TileFrameY % 36 != 0)
            top--;
        int chestInd = Chest.FindChest(left, top);
        player.cursorItemIconID = -1;
        if (chestInd < 0)
            player.cursorItemIconText = Language.GetTextValue("LegacyDresserType.0");
        else
        {
            if (Main.chest[chestInd].name != "")
                player.cursorItemIconText = Main.chest[chestInd].name;
            else
                player.cursorItemIconText = ContainerName.GetDefault();

            if (player.cursorItemIconText == ContainerName.GetDefault())
            {
                player.cursorItemIconID = DresserDrop;
                player.cursorItemIconText = "";
            }
        }
        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        if (Main.tile[Player.tileTargetX, Player.tileTargetY].TileFrameY > 0)
            player.cursorItemIconID = ItemID.FamiliarShirt;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, DresserDrop);
        Chest.DestroyChest(i, j);
    }
}