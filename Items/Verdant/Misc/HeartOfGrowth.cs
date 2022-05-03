using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Misc
{
    class HeartOfGrowth : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Heart of Growth");
            Tooltip.SetDefault("Permenantly increases max minions by one.\nCan only be used once.\n'A heart that beats everliving.'");
        }

        public override void SetDefaults()
        {
            item.accessory = false;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(gold: 50);
            item.consumable = true;
            item.width = 32;
            item.height = 28;
            item.useAnimation = item.useTime = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
        }

        public override bool CanUseItem(Player player) => player.GetModPlayer<VerdantPlayer>().heartOfGrowth;

        public override bool UseItem(Player player)
        {
            player.GetModPlayer<VerdantPlayer>().heartOfGrowth = true;

            for (int i = 0; i < 22; ++i)
                Projectile.NewProjectile(player.Center, new Vector2(Main.rand.NextFloat(4, 12), 0).RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<HealingParticle>(), 0, 0, player.whoAmI, 0, 0);
            return true;
        }
    }
}