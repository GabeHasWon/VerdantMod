using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Weapons
{
    class EtherialTag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Etherial Leash");
            Tooltip.SetDefault("Controls a memory lost long ago\nRight click to direct flames towards enemies");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.width = 20;
            item.height = 46;
            item.useTime = 20;
            item.useAnimation = 20;
            item.knockBack = 1;
            item.crit = 8;
            item.shootSpeed = 0f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.rare = ItemRarityID.Blue;
            item.shoot = ModContent.ProjectileType<VerdantWisp>();
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.channel = true;
            item.noMelee = true;
            item.useTurn = false;
            item.noUseGraphic = true;
            item.autoReuse = false;
            item.summon = true;
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
