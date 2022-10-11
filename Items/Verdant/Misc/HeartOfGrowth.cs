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
            Item.accessory = false;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 50);
            Item.consumable = false;
            Item.width = 32;
            Item.height = 28;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool CanUseItem(Player player) => !player.GetModPlayer<VerdantPlayer>().heartOfGrowth;

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<VerdantPlayer>().heartOfGrowth = true;

            for (int i = 0; i < 22; ++i)
                Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, new Vector2(Main.rand.NextFloat(4, 12), 0).RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<HealingParticle>(), 0, 0, player.whoAmI, 0, 0);
            return true;
        }
    }
}