using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Items.Verdant.Materials;
using System.Linq;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(1)]
class YellowPetalFloater : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Cloudsprout", "Floating bounce pad\nRemains even upon world exit\nLeft click on an existing cloud to remove it\nRight click for two seconds to remove all cloudsprouts");
    public override void SetDefaults() => QuickItem.SetStaff(this, 40, 20, ProjectileID.GolemFist, 9, 0, 24, 0, 0, ItemRarityID.Green);
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<YellowBulb>(), 1), (ModContent.ItemType<LushLeaf>(), 3), (ItemID.Cloud, 10));

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

    int rightClickTimer = 0;

    public override void HoldItem(Player player)
    {
        if (Main.mouseRight)
        {
            rightClickTimer++;

            if (rightClickTimer > 300)
            {
                ClearAll(false);
                rightClickTimer = 0;
            }
        }
    }

    internal static void ClearAll(bool puff)
    {
        foreach (var item in ForegroundManager.PlayerLayerItems.Where(x => x is CloudbloomEntity cloud && cloud.puff == puff))
        {
            item.killMe = true;
            int dust = puff ? DustID.PinkStarfish : DustID.Cloud;

            for (int i = 0; i < 14; ++i)
                Dust.NewDust(item.position, 50, 30, dust, Main.rand.NextFloat(-0.2f, 0.2f), Main.rand.NextFloat(2, 3f));
        }
    }
}
