using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Items.Verdant.Materials;
using Terraria.Localization;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(1)]
class ChlorovineWand : ModItem
{
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ModContent.ItemType<LushLeaf>());

    public override void SetDefaults()
    {
        Item.Size = new Vector2(28, 28);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 2;
        Item.useTime = 2;
        Item.autoReuse = false;
        Item.rare = ItemRarityID.Purple;
        Item.channel = true;
        Item.shoot = ModContent.ProjectileType<ChlorovineWandProjectile>();
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.maxStack = 1;
        Item.tileWand = ModContent.ItemType<LushLeaf>();
    }

    public override bool AltFunctionUse(Player player) => true;

    public override bool CanUseItem(Player player)
    {
        if (Main.myPlayer == player.whoAmI && player.altFunctionUse == 2 && !ChlorovineWandProjectile.ConsumeTileWand(player, true))
            ChlorovineWandProjectile.KillVineAtMouse(player);
        return true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<PermVineWand>()
            .AddIngredient(ItemID.ChlorophyteBar, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}

public class ChlorovineWandProjectile : ModProjectile
{
    public override string Texture => "Verdant/Items/Verdant/Tools/ChlorovineWand";

    public ref float Timer => ref Projectile.ai[0];

    public ZipvineEntity lastVine = null;

    private bool _init = false;

    public override void SetDefaults()
    {
        Projectile.width = 36;
        Projectile.height = 28;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.aiStyle = -1;

        DrawHeldProjInFrontOfHeldItemAndArms = true;
    }

    public override bool? CanDamage() => false;

    public override void AI()
    {
        if (!_init)
        {
            Timer = 1;
            lastVine = null;
            _init = true;
        }

        Player p = Main.player[Projectile.owner];
        p.heldProj = Projectile.whoAmI;
        p.direction = Math.Sign(Main.MouseWorld.X - p.Center.X);

        if (p.whoAmI != Main.myPlayer)
            return; //mp check (hopefully)

        Projectile.timeLeft++;
        Projectile.rotation = p.AngleFrom(Main.MouseWorld); //So it looks like the player is holding it properly
        Projectile.position = p.position - new Vector2(10, 0).RotatedBy(Projectile.rotation);
        Helper.ArmsTowardsMouse(p);

        if (Main.mouseRight)
        {
            KillVineAtMouse(p);
            return;
        }

        if (!p.channel)
            Projectile.Kill();
        else
        {
            p.itemTime = 2;
            p.itemAnimation = 2;

            Timer--;

            const int MinDistance = 16;

            if (Timer <= 0 && Projectile.owner == Main.myPlayer && (lastVine is null || Vector2.Distance(Main.MouseWorld, lastVine.Center) > MinDistance))
            {
                if (!ConsumeTileWand(Main.player[Projectile.owner]))
                {
                    p.channel = false;
                    return;
                }

                lastVine = VineWandCommon.BuildChlorovine(MinDistance, lastVine as ChlorovineEntity);
                Timer = 3;
            }
        }
    }

    internal static void KillVineAtMouse(Player player) => PermVineWandProjectile.KillVineAtMouse(player);
    internal static bool ConsumeTileWand(Player player, bool justChecking = false) => PermVineWandProjectile.ConsumeTileWand(player, justChecking);
}
