using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Foreground;
using Verdant.Foreground.Parallax;
using Verdant.Items.Verdant.Materials;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Tools;

class PermVineWand : ModItem
{
    public override string Texture => base.Texture.Replace("Perm", "");

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Zipvine (Permanant)");
        Tooltip.SetDefault($"Allows the user to build a vine\nThe vine works like a rope and can be used in any open space\nThese vines use [i:{ModContent.ItemType<LushLeaf>()}] to build, and drop them on being destroyed.");
    }

    public override void SetDefaults()
    {
        Item.Size = new Vector2(36, 28);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 2;
        Item.useTime = 2;
        Item.autoReuse = false;
        Item.rare = ItemRarityID.Green;
        Item.channel = true;
        Item.shoot = ModContent.ProjectileType<PermVineWandProjectile>();
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.maxStack = 1;
        Item.tileWand = ModContent.ItemType<LushLeaf>();
    }

    public override bool AltFunctionUse(Player player) => true;
}

public class PermVineWandProjectile : ModProjectile
{
    public override string Texture => "Verdant/Items/Verdant/Tools/VineWand";

    public ref float Timer => ref Projectile.ai[0];

    public EnchantedVine LastVine = null;

    private bool _init = false;

    public override void SetStaticDefaults() => DisplayName.SetDefault("Enchanted Vine");

    public override void SetDefaults()
    {
        Projectile.width = 42;
        Projectile.height = 24;
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
            LastVine = null;
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
            var vine = ForegroundManager.PlayerLayerItems.FirstOrDefault(x => x is EnchantedVine && x.DistanceSQ(Main.MouseWorld) < 18 * 18) as EnchantedVine;
            if (vine != null && vine.perm)
            {
                vine.Kill();
                Main.player[Projectile.owner].QuickSpawnItem(Projectile.GetSource_FromAI(), ModContent.ItemType<LushLeaf>());
            }
            return;
        }

        if (!p.channel)
            Projectile.Kill();
        else
        {
            p.itemTime = 2;
            p.itemAnimation = 2;

            Timer--;

            if (Timer <= 0)
            {
                LastVine = VineWandCommon.BuildVine(Projectile.owner, LastVine);
                ConsumeTileWand(Main.player[Projectile.owner]);
                Timer = 3;
            }
        }
    }

    private static void ConsumeTileWand(Player player)
    {
        for (int i = 0; i < player.inventory.Length; ++i)
        {
            Item item = player.inventory[i];

            if (!item.IsAir && item.type == ModContent.ItemType<LushLeaf>())
            {
                item.stack--;

                if (item.stack <= 0)
                    item.TurnToAir();
                break;
            }
        }
    }
}
