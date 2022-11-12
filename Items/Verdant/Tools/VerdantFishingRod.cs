using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Tools
{
    [Sacrifice(1)]
    class VerdantFishingRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lush Fishing Rod");
            Tooltip.SetDefault("Gives off light when held");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FiberglassFishingPole);
            Item.fishingPole = 26;
            Item.shootSpeed = 13f;
            Item.shoot = ModContent.ProjectileType<Projectiles.Misc.VerdantBobber>();
            Item.value = Item.buyPrice(0, 0, 30, 0);
        }

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Misc.VerdantBobber>()] > 0)
                Lighting.AddLight(player.position + new Vector2(42 * player.direction, -6), new Vector3(0.1f, 0.03f, 0.06f) * 12);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<VerdantStrongVineMaterial>(16).
                AddIngredient<PinkPetal>(8).
                AddIngredient<Lightbulb>(4).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
