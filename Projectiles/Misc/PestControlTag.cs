using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Misc;
using Verdant.Systems;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Projectiles.Misc;

class PestControlTag : ModProjectile
{
    public override void SetStaticDefaults() => Main.projFrames[Type] = 1;

    public override void SetDefaults()
    {
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.timeLeft = 6000;
        Projectile.tileCollide = false;
        Projectile.aiStyle = 0;
    }

    public override bool? CanCutTiles() => false;

    public override void AI()
    {
        Projectile.rotation += 0.01f;
        Lighting.AddLight(Projectile.Center, TorchID.Crimson);

        Player close = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];

        if (close.active && !close.dead)
        {
            float dist = close.Distance(Projectile.Center);

            Projectile.alpha = Math.Clamp((int)(255 * ((dist + 20) / 600)), 0, 255);

            if (dist > 600)
            {
                Projectile.Kill();

                if (close.whoAmI == Main.myPlayer)
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, WorldGen.crimson ? ModContent.ItemType<CrimsonEffigy>() : ModContent.ItemType<CorruptEffigy>(), 1);
            }
            else if (close.Hitbox.Intersects(Projectile.Hitbox))
            {
                ScreenTextManager.CurrentText = null;
                DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".PestControl");
                Projectile.Kill();
            }
        }
    }
}
