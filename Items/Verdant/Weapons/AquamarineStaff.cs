using Microsoft.Xna.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Projectiles.Magic;

namespace Verdant.Items.Verdant.Weapons;

#nullable enable

class AquamarineStaff : ModItem
{
    public override void SetStaticDefaults() => Item.staff[Type] = true;

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.RubyStaff);
        Item.DamageType = DamageClass.Summon;
        Item.autoReuse = true;
        Item.shootSpeed = 14;
        Item.shoot = ModContent.ProjectileType<AquamarineBolt>();
        Item.damage = 8;
        Item.useTime = 15;
        Item.useAnimation = 15;
        Item.mana = 4;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (!GetClosestMinion(player, out Projectile? closest))
            return;

        damage += (int)(closest.damage * 0.6f);
        position = closest.Center;
        velocity = closest.DirectionTo(Main.MouseWorld) * Item.shootSpeed;
        player.ChangeDir(Math.Sign(velocity.X));
    }

    private static bool GetClosestMinion(Player player, [NotNullWhen(true)] out Projectile? closest)
    {
        closest = null;
        for (int i = 0; i < Main.maxProjectiles; ++i)
        {
            Projectile p = Main.projectile[i];

            if (p.active && p.owner == player.whoAmI && p.minionSlots > 0 && (closest is null || closest.DistanceSQ(player.Center) > p.DistanceSQ(player.Center)))
                closest = p;
        }

        if (closest is null)
            return false;

        return true;
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