using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Throwing;

namespace Verdant.Items.Verdant.Weapons;

class LushDagger : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Lush Splinter");
        // Tooltip.SetDefault("Thrown in triplets\n'Nature's nuisance of choice'");
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.ThrowingKnife);
        Item.damage = 8;
        Item.useTime = 5;
        Item.useAnimation = 15;
        Item.reuseDelay = 15;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<LushDaggerProj>();
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        velocity = velocity.RotatedByRandom(0.07f) * Main.rand.NextFloat(0.96f, 1.04f);
    }
}
