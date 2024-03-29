﻿using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;

namespace Verdant.Tiles.Verdant.Decor.LushFurniture
{
    public class LushChair : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSatOnForNPCs[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            AddMapEntry(new Color(114, 69, 39), Language.GetText("MapObject.Chair"));

            DustType = DustID.t_BorealWood;
            AdjTiles = new int[] { TileID.Chairs };
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => FurnitureHelper.ChairInteract(i, j, settings);
        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info) => FurnitureHelper.ModifySittingTargetInfo(i, j, ref info);
        public override bool RightClick(int i, int j) => FurnitureHelper.RightClick(i, j);
        public override void MouseOver(int i, int j) => FurnitureHelper.MouseOver(i, j, ModContent.ItemType<Items.Verdant.Blocks.LushWood.LushChairItem>());
    }
}