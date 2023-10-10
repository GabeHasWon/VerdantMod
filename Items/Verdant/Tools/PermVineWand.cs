using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Items.Verdant.Materials;
using Terraria.Localization;
using Verdant.Systems.Syncing;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(1)]
class PermVineWand : ModItem
{
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(ModContent.ItemType<LushLeaf>());

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

    public override bool CanUseItem(Player player)
    {
        if (Main.myPlayer == player.whoAmI && player.altFunctionUse == 2 && !PermVineWandProjectile.ConsumeTileWand(player, true))
            PermVineWandProjectile.KillVineAtMouse(player);
        return true;
    }
}

public class PermVineWandProjectile : ModProjectile
{
    public override string Texture => "Verdant/Items/Verdant/Tools/PermVineWand";

    public ref float Timer => ref Projectile.ai[0];

    public ZipvineEntity lastVine = null;

    private bool _init = false;

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

                lastVine = VineWandCommon.BuildVine(MinDistance, lastVine);
                Timer = 3;
            }
        }
    }

    /// <summary>
    /// Kills the vine near the mouse. Should only be run on the multiplayer client that kills the vine.
    /// </summary>
    /// <param name="player"></param>
    internal static void KillVineAtMouse(Player player)
    {
        if (ForegroundManager.PlayerLayerItems.FirstOrDefault(x => x is ZipvineEntity && x.DistanceSQ(Main.MouseWorld) < 18 * 18) is ZipvineEntity vine)
        {
            vine.Kill();
            player.QuickSpawnItem(player.GetSource_FromThis(), ModContent.ItemType<LushLeaf>());

            if (Main.netMode != NetmodeID.SinglePlayer)
                new KillZipvineModule((short)Main.myPlayer, (short)ForegroundManager.PlayerLayerItems.IndexOf(vine)).Send();
        }
    }

    internal static bool ConsumeTileWand(Player player, bool justChecking = false)
    {
        for (int i = 0; i < player.inventory.Length; ++i)
        {
            Item item = player.inventory[i];

            if (!item.IsAir && item.type == ModContent.ItemType<LushLeaf>())
            {
                if (!justChecking) // Only consume if I'm not just checking
                {
                    item.stack--;

                    if (item.stack <= 0)
                        item.TurnToAir();
                }
                return true;
            }
        }
        return false;
    }
}
