using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.TileEntity
{
    public class PickipuffItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Pickipuff", "Can only be placed on lush leaves\nCan be picked to harvest Puff\nRegrows after a while");
        public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 28, ItemRarityID.Orange, 99, true);
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.LivingLoom, 1, (ModContent.ItemType<PuffMaterial>(), 12), (ModContent.ItemType<LushLeaf>(), 8));

        public override bool CanUseItem(Player player)
        {
            Tile atTarget = Main.tile[Player.tileTargetX, Player.tileTargetY];
            return player.whoAmI == Main.myPlayer && player.InInteractionRange(Player.tileTargetX, Player.tileTargetY) && atTarget.HasTile && atTarget.TileType == ModContent.TileType<VerdantGrassLeaves>();
        }

        public override bool? UseItem(Player player)
        {
            var j = Main.MouseWorld.ToTileCoordinates16();
            ModContent.GetInstance<Tiles.TileEntities.Puff.Pickipuff>().Place(j.X, j.Y);
            return true;
        }
    }
}
