using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            QuickItem.SetMinion(this, 48, 48, ModContent.ProjectileType<VerdantSnailMinion>(), 12, 10, ItemRarityID.Green);
            Item.buffType = ModContent.BuffType<SnailBuff>();
            Item.buffTime = 20;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe m = CreateRecipe();
            m.AddIngredient(ModContent.ItemType<VerdantStrongVineMaterial>(), 8);
            m.AddIngredient(ModContent.ItemType<VerdantWoodBlock>(), 10);
            m.AddTile(TileID.Anvils);
            m.Register();
        }
    }
}
