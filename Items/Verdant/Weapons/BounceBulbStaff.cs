using Terraria.ModLoader;
using Verdant.Projectiles.Misc;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Weapons
{
    class BounceBulbStaff : ModItem
    {
        public override void SetDefaults() => QuickItem.SetStaff(this, 48, 48, ProjectileType<VerdantWisp>(), 9, 1, 8, 8, 0, 2);

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots a bulb which can rebound the player when hit.\nUnobtainable because it's not very fun to use.");
        }

        //public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        //{
        //    Projectile.NewProjectile(position, new Vector2(speedX, speedY), ProjectileType<VerdantBounceBulb>(), 1, 1f, player.whoAmI, 0, 0);
        //    return true;
        //}
    }
}
