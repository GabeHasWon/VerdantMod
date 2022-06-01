using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Tools
{
    class YellowPetalFloater : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Cloudsprout", "Floating bounce pad\nRemains even upon world exit");
        public override void SetDefaults() => QuickItem.SetStaff(this, 48, 48, ModContent.ProjectileType<YellowPetalFloaterProj>(), 9, 0, 24, 0, 0, ItemRarityID.Green);
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.LivingLoom, 1, (ModContent.ItemType<YellowBulb>(), 1), (ModContent.ItemType<LushLeaf>(), 3), (ItemID.Cloud, 10));

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            return true;
        }
    }
}
