using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Tools
{
    class CloudsproutPuff : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Cloudsprout", "Floating bounce pad\nRemains even upon world exit\nLeft click on an existing cloud to remove it");
        public override void SetDefaults() => QuickItem.SetStaff(this, 40, 20, ProjectileID.GolemFist, 9, 0, 24, 0, 0, ItemRarityID.Green);
        public override void AddRecipes() => QuickItem.AddRecipe(this, Mod, TileID.LivingLoom, 1, (ModContent.ItemType<YellowBulb>(), 1), (ModContent.ItemType<PuffMaterial>(), 14));

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

            ForegroundManager.AddItem(new CloudbloomEntity(Main.MouseWorld, true), true, true);
            return false;
        }
    }
}
