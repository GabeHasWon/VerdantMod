using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Aquamarine;
using Verdant.Items.Verdant.Materials;
using Verdant.Projectiles.Magic;

namespace Verdant.Items.Verdant.Weapons;

class AquamarineStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Aquamarine Staff");
        // Tooltip.SetDefault("Summons bolts to come from your minions\nThe bolts get some added damage from your minions");

        Item.staff[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.RubyStaff);
        Item.DamageType = DamageClass.Summon;
        Item.autoReuse = true;
        Item.shootSpeed = 14;
        Item.shoot = ModContent.ProjectileType<AquamarineBolt>();
        Item.damage = 8;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.mana = 4;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        List<Projectile> projs = new();

        for (int i = 0; i < Main.maxProjectiles; ++i)
        {
            Projectile p = Main.projectile[i];

            if (p.active && p.owner == player.whoAmI && p.minionSlots > 0)
                projs.Add(p);
        }

        if (projs.Count == 0)
            return;

        var choice = Main.rand.Next(projs);
        damage += (int)(choice.damage * 0.6f);
        position = choice.Center;
        velocity = choice.DirectionTo(Main.MouseWorld) * Item.shootSpeed;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupSystem.AquamarineRecipeGroup, 8)
            .AddIngredient(ItemID.GoldBar, 10)
            .AddIngredient<LushLeaf>(4)
            .AddTile(TileID.Anvils)
            .Register();

        Recipe.Create(ModContent.ItemType<AquamarinePlatinumStaff>())
            .AddRecipeGroup(RecipeGroupSystem.AquamarineRecipeGroup, 8)
            .AddIngredient(ItemID.PlatinumBar, 10)
            .AddIngredient<LushLeaf>(4)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

internal class AquamarinePlatinumStaff : AquamarineStaff
{
    public override void AddRecipes() { }
}