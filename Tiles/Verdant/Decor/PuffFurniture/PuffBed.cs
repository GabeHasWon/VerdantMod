using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Blocks.Mysteria.Furniture;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

public class PuffBed : ModTile
{
    [Sacrifice(1)]
    public class PuffBedItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 54, 34, ModContent.TileType<PuffBed>());
        public override void AddRecipes() 
            => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<LushLeaf>(), 6), (ModContent.ItemType<PuffMaterial>(), 9), (ItemID.Silk, 5));
    }

    public override void SetStaticDefaults()
    {
        BedHelper.Defaults<PuffBedItem>(this, new Color(255, 112, 202));
        DustType = ModContent.DustType<PuffDust>();
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
	public override void NumDust(int i, int j, bool fail, ref int num) => num = 1;
	public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY) => (width, height) = (2, 2);
	public override void ModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info) => info.VisualOffset.Y += 4f;
    public override bool RightClick(int i, int j) => BedHelper.RightClick(i, j);

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;

        if (!Player.IsHoveringOverABottomSideOfABed(i, j))
        {
            if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
            {
                player.noThrow = 2;
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = ItemID.SleepingIcon;
            }
        }
        else
        {
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<PuffBedItem>();
        }
    }
}