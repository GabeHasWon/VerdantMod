using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Equipables;

namespace Verdant.Projectiles.Minion
{
    class HealingFlower : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Yellow Sprout");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.width = 30;
            Projectile.height = 34;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
        }

        public override bool? CanCutTiles() => false;
        public override bool MinionContactDamage() => false;

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.hide)
                overPlayers.Add(index);
        }

        public override void AI()
        {
            const float XMod = 0.02f;

            Player plr = Main.player[Projectile.owner];
            if (plr.dead || !plr.GetModPlayer<HealingFlowerPlayer>().hasHealFlower)
                Projectile.Kill();

            float xOff = (float)Math.Sin(Projectile.ai[0]++ * XMod);
            Player player = Main.player[Projectile.owner];
            
            Projectile.Center = player.Center + new Vector2(0, player.gfxOffY);
            Projectile.position.X += xOff * 42f;
            Projectile.position.Y += (float)Math.Sin(Projectile.ai[1]++ * 0.06f) * 8f;
            Projectile.rotation = xOff * 0.2f;

            Lighting.AddLight(Projectile.Center, new Vector3(0.4f, 0.12f, 0.24f) * 0.8f);

            if (Math.Cos(Projectile.ai[0] * XMod) > 0)
                Projectile.hide = true;
            else
                Projectile.hide = false;
        }
    }
}
