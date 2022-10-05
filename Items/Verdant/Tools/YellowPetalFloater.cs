using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Foreground;
using Verdant.Foreground.Parallax;
using Verdant.Items.Verdant.Materials;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Tools
{
    class YellowPetalFloater : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Cloudsprout", "Floating bounce pad\nRemains even upon world exit");
        public override void SetDefaults() => QuickItem.SetStaff(this, 48, 48, ProjectileID.GolemFist, 9, 0, 24, 0, 0, ItemRarityID.Green);
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.LivingLoom, 1, (ModContent.ItemType<YellowBulb>(), 1), (ModContent.ItemType<LushLeaf>(), 3), (ItemID.Cloud, 10));

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => position = Main.MouseWorld;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            foreach (var item in ForegroundManager.PlayerLayerItems)
            {
                if (item is CloudbloomEntity && Vector2.DistanceSquared(item.Center, Main.MouseWorld) < 40 * 40)
                {
                    item.killMe = true;
                    return false;
                }
            }   
            
            ForegroundManager.AddItem(new CloudbloomEntity(Main.MouseWorld), true, true);
            return false;
        }
    }
}
