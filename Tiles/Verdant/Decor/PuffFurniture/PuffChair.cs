using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Dusts;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

public class PuffChair : ModTile
{
    [Sacrifice(3)]
    public class PuffChairItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 32, ModContent.TileType<PuffChair>());
        public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<LushLeaf>(), 2), (ModContent.ItemType<PuffMaterial>(), 2));
    }

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
        TileObjectData.newTile.CoordinateHeights = [16, 18];
        TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
        TileObjectData.newTile.StyleWrapLimit = 2;
        TileObjectData.newTile.StyleMultiplier = 2;
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
        TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
        TileObjectData.addAlternate(1);
        TileObjectData.addTile(Type);

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
        AddMapEntry(new Color(255, 112, 202), Language.GetText("MapObject.Chair"));
        RegisterItemDrop(ModContent.ItemType<PuffChairItem>());

        DustType = ModContent.DustType<PuffDust>();
        AdjTiles = [TileID.Chairs];
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => FurnitureHelper.ChairInteract(i, j, settings);
    public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info) => FurnitureHelper.ModifySittingTargetInfo(i, j, ref info);
    public override bool RightClick(int i, int j) => FurnitureHelper.RightClick(i, j);
    public override void MouseOver(int i, int j) => FurnitureHelper.MouseOver(i, j, ModContent.ItemType<PuffChairItem>());
}