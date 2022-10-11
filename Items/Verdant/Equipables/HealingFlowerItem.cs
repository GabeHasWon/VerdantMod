using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Minion;

namespace Verdant.Items.Verdant.Equipables
{
    class HealingFlowerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yellow Sprout");
            Tooltip.SetDefault("Increases healing from potions by 20%\nIncreases mana healing from potions by 33%\nProvides a tiny bit of light\n'You are followed by the strength of willpower'");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<HealingFlowerPlayer>().hasHealFlower = true;

            if (player.ownedProjectileCounts[ModContent.ProjectileType<HealingFlower>()] == 0)
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<HealingFlower>(), 0, 0, player.whoAmI);
        }
    }

    class HealingFlowerPlayer : ModPlayer
    {
        internal bool hasHealFlower = false;

        public override void ResetEffects() => hasHealFlower = false;

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (hasHealFlower)
                healValue = (int)(healValue * 1.2f);
        }

        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            if (hasHealFlower)
                healValue = (int)(healValue * 1.3333f);
        }
    }
}