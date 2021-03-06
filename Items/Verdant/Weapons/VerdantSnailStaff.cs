using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Buffs.Minion;
using Verdant.Items.Verdant.Blocks;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Projectiles.Minion;

namespace Verdant.Items.Verdant.Weapons
{
    class VerdantSnailStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snail Staff");
            Tooltip.SetDefault("Summons determined snails.\nThese snails take less than a full minion slot to summon.");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            QuickItem.SetMinion(this, 48, 48, ModContent.ProjectileType<VerdantSnailMinion>(), 12, 10, ItemRarityID.Green);
            item.buffType = ModContent.BuffType<SnailBuff>();
            item.buffTime = 20;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe m = new ModRecipe(mod);
            m.AddIngredient(ModContent.ItemType<VerdantStrongVineMaterial>(), 8);
            m.AddIngredient(ModContent.ItemType<VerdantWoodBlock>(), 10);
            m.AddTile(TileID.Anvils);
            m.SetResult(this);
            m.AddRecipe();
        }
    }
}
