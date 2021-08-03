using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Buffs.Minion;

namespace Verdant.Projectiles.Minion
{
    class VerdantSnailMinion : ModProjectile
    {
        ref float MovementState => ref projectile.ai[0];
        ref float Timer => ref projectile.ai[1];

        private int _target = -1;
        private int _skin = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snale");
            Main.projFrames[projectile.type] = 10;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.OneEyedPirate);
            projectile.aiStyle = -1;
            projectile.width = 30;
            projectile.height = 22;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.minionSlots = 0.75f;
            projectile.minion = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;

            aiType = 0;
        }

        public override bool MinionContactDamage() => true;

        public override bool? CanCutTiles() => false;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (MovementState == 4)
                projectile.velocity = -projectile.velocity.RotatedByRandom(0.03f) * 0.94f;
            return false;
        }

        public const int AnimSpeedMult = 14;
        public const int AnimSpeedMultHasty = 6;
        public const int DistanceUntilReturn = 700;

        public override void AI()
        {
            Player p = Main.player[projectile.owner];
            Timer++;

            if (!p.HasBuff(ModContent.BuffType<SnailBuff>()))
                projectile.active = false;

            projectile.timeLeft = 20;
            projectile.friendly = MovementState == 4;

            if (MovementState == 0) //Spawn
            {
                projectile.spriteDirection = Main.rand.NextBool(2) ? -1 : 1;
                MovementState = 1;
                Timer--;
                _target = -1;
                _skin = Main.rand.Next(2);
            }
            if (MovementState == 1) //Literally vibing too hard
            {
                projectile.tileCollide = true;
                if (Timer == AnimSpeedMult)
                    SetFrame(1);
                if (Timer == 2 * AnimSpeedMult)
                {
                    SetFrame(0);
                    projectile.velocity.X = 0.3f * projectile.spriteDirection;
                }
                if (Timer == 3 * AnimSpeedMult)
                {
                    SetFrame(2);
                    if (projectile.velocity.X == 0)
                        projectile.spriteDirection *= -1;
                }
                if (Timer == 4 * AnimSpeedMult)
                {
                    projectile.velocity.X = 0f;
                    SetFrame(0);
                    Timer = 0;
                }

                if (Vector2.Distance(p.position, projectile.position) > DistanceUntilReturn)
                {
                    MovementState = 2;
                    Timer = 0;
                }

                if (_target == -1) //Get target
                {
                    int hasTarget = -1;
                    for (int i = 0; i < Main.npc.Length; ++i) //Find target
                    {
                        float dist = Vector2.Distance(Main.npc[i].position, projectile.position);
                        if (Main.npc[i].CanBeChasedBy() && dist < 500 && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) &&
                            (hasTarget == -1 || (hasTarget != -1 && Vector2.Distance(Main.npc[hasTarget].Center, projectile.Center) < dist)))
                            hasTarget = i;
                    }

                    if (hasTarget != -1) //Select target & switch states
                    {
                        _target = hasTarget;
                        MovementState = 4;
                        Timer = 0;
                        SetFrame(0);
                        projectile.velocity *= 0f;
                    }
                }
                else
                {
                    if (_target < -1)
                        _target++;
                }

                //Gravity
                projectile.velocity.Y += 0.2f;
            }
            else if (MovementState == 2) //Catch up to player
            {
                projectile.tileCollide = false;
                projectile.spriteDirection = p.position.X < projectile.position.X ? -1 : 1;
                if (Timer == AnimSpeedMultHasty)
                    SetFrame(2);
                if (Timer == AnimSpeedMultHasty * 2)
                    SetFrame(3);
                if (Timer == AnimSpeedMultHasty * 3)
                    SetFrame(4);
                if (Timer > AnimSpeedMultHasty * 3)
                {
                    float adjTimer = Timer - (AnimSpeedMultHasty * 3);
                    float mult = 1f;
                    if (adjTimer <= 60)
                        mult = adjTimer / 60f;
                    projectile.velocity = Vector2.Normalize(p.position - projectile.position) * 7 * mult;
                    projectile.rotation += 0.4f * mult;

                    if (Vector2.Distance(p.position, projectile.position) < DistanceUntilReturn * 0.6f)
                    {
                        MovementState = 3;
                        Timer = 0;
                    }
                }
            }
            else if (MovementState == 3) //Deccelerate
            {
                projectile.tileCollide = true;
                float mult = 1 - (Timer / 240f);
                projectile.velocity.X *= mult;
                projectile.velocity.Y += 0.2f;
                projectile.rotation += 0.4f * mult;

                if (Timer > 60)
                {
                    Timer = 0;
                    MovementState = 1;
                    projectile.rotation = 0;
                    projectile.spriteDirection = Main.rand.NextBool(2) ? -1 : 1;
                }
            }
            else if (MovementState == 4) //ENEMY DETECTED
            {
                projectile.tileCollide = true;

                if (Timer == AnimSpeedMultHasty)
                    SetFrame(2);
                if (Timer == AnimSpeedMultHasty * 2)
                    SetFrame(3);
                if (Timer == AnimSpeedMultHasty * 3)
                    SetFrame(4);
                if (Timer > AnimSpeedMultHasty * 3)
                {
                    if (_target != -2 && Main.npc[_target].active)
                        projectile.velocity += Vector2.Normalize(Main.npc[_target].Center - projectile.Center) * 0.4f;
                    if (projectile.velocity.Length() > 7f)
                        projectile.velocity = Vector2.Normalize(projectile.velocity) * 7f;
                    if (Timer >= AnimSpeedMultHasty * 36 || _target == -2 || !Main.npc[_target].active || projectile.velocity.Length() < 0.1f)
                    {
                        projectile.velocity.Y += 0.2f;
                        projectile.velocity.X *= 0.9999f;
                        if (Timer >= AnimSpeedMultHasty * 40)
                        {
                            _target = -100; //Target cooldown
                            MovementState = 3;
                            Timer = 0;
                        }
                    }
                    else
                        projectile.rotation += 0.4f;
                }
            }
        }

        private void SetFrame(int frame)
        {
            projectile.frame = frame + (_skin * 5);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Slimed, 20);
            projectile.velocity = projectile.velocity.RotatedBy(Main.rand.Next(-70, 71) * 0.01f) * -1f;
        }
    }
}
