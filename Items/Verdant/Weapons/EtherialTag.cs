using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Weapons
{
    class EtherialTag : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => false;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.width = 20;
            Item.height = 46;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 1;
            Item.crit = 8;
            Item.shootSpeed = 0f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<VerdantWisp>();
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.channel = true;
            Item.noMelee = true;
            Item.useTurn = false;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.DamageType = DamageClass.Summon;
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
