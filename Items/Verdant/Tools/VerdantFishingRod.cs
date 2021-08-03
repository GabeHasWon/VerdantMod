using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks;
using Verdant.Items.Verdant.Materials;
using Verdant.Projectiles.Magic;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Tools
{
    class VerdantFishingRod : ModItem
    {
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.FiberglassFishingPole);
            item.fishingPole = 26;
            item.shootSpeed = 13f;
            item.shoot = ProjectileType<Projectiles.Misc.VerdantBobber>();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Growth Fishing Rod");
            Tooltip.SetDefault("Gives off light when held");
        }

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ProjectileType<Projectiles.Misc.VerdantBobber>()] > 0)
                Lighting.AddLight(player.position + new Vector2(42 * player.direction, -6), new Vector3(0.1f, 0.03f, 0.06f) * 12);
        }

        public override void AddRecipes()
        {
            ModRecipe m = new ModRecipe(mod);
            m.AddIngredient(ItemType<VerdantStrongVineMaterial>(), 16);
            m.AddIngredient(ItemType<PinkPetal>(), 8);
            m.AddIngredient(ItemType<Lightbulb>(), 8);
            m.AddTile(TileID.Anvils);
            m.SetResult(this);
            m.AddRecipe();
        }
    }
}
