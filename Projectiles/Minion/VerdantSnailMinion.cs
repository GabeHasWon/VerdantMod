using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Buffs.Minion;

namespace Verdant.Projectiles.Minion
{
    class VerdantSnailMinion : ModProjectile
    {
        ref float MovementState => ref Projectile.ai[0];
        ref float Timer => ref Projectile.ai[1];

        private int _target = -1;
        private int _skin = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snale");
            Main.projFrames[Projectile.type] = 15;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.OneEyedPirate);
            Projectile.aiStyle = -1;
            Projectile.width = 30;
            Projectile.height = 22;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.minionSlots = 0.75f;
            Projectile.minion = true;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;

            AIType = 0;
        }

        public override bool MinionContactDamage() => true;

        public override bool? CanCutTiles() => false;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (MovementState == 4)
                Projectile.velocity = -Projectile.velocity.RotatedByRandom(0.03f) * 0.94f;
            return false;
        }

        public const int AnimSpeedMult = 14;
        public const int AnimSpeedMultHasty = 6;
        public const int DistanceUntilReturn = 700;

        public override void AI()
        {
            Player p = Main.player[Projectile.owner];
            Timer++;

            if (!p.HasBuff(ModContent.BuffType<SnailBuff>()))
                Projectile.active = false;

            Projectile.timeLeft = 20;
            Projectile.friendly = MovementState == 4;

            if (MovementState == 0) //Spawn
            {
                Projectile.spriteDirection = Main.rand.NextBool(2) ? -1 : 1;
                MovementState = 1;
                Timer--;
                _target = -1;
                _skin = Main.rand.Next(3);
            }
            if (MovementState == 1) //Literally vibing too hard
            {
                Projectile.tileCollide = true;
                if (Timer == AnimSpeedMult)
                    SetFrame(1);
                if (Timer == 2 * AnimSpeedMult)
                {
                    SetFrame(0);
                    Projectile.velocity.X = 0.3f * Projectile.spriteDirection;
                }
                if (Timer == 3 * AnimSpeedMult)
                {
                    SetFrame(2);
                    if (Projectile.velocity.X == 0)
                        Projectile.spriteDirection *= -1;
                }
                if (Timer == 4 * AnimSpeedMult)
                {
                    Projectile.velocity.X = 0f;
                    SetFrame(0);
                    Timer = 0;
                }

                if (Vector2.Distance(p.position, Projectile.position) > DistanceUntilReturn)
                {
                    MovementState = 2;
                    Timer = 0;
                }

                if (p.HasMinionAttackTargetNPC) //Minion targetting!
                {
                    _target = p.MinionAttackTargetNPC;
                    MovementState = 4;
                    Timer = 0;
                    SetFrame(0);
                    Projectile.velocity *= 0f;
                    return;
                }

                if (_target == -1) //Get target
                {
                    int hasTarget = -1;
                    for (int i = 0; i < Main.npc.Length; ++i) //Find target
                    {
                        float dist = Vector2.Distance(Main.npc[i].position, Projectile.position);
                        bool line = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height);
                        if (Main.npc[i].CanBeChasedBy() && dist < 500 && line && (hasTarget == -1 || (hasTarget != -1 && Projectile.Distance(Main.npc[hasTarget].Center) < dist)))
                            hasTarget = i;
                    }

                    if (hasTarget != -1) //Select target & switch states
                    {
                        _target = hasTarget;
                        MovementState = 4;
                        Timer = 0;
                        SetFrame(0);
                        Projectile.velocity *= 0f;
                    }
                }
                else
                {
                    if (_target < -1)
                        _target++;
                }

                //Gravity
                Projectile.velocity.Y += 0.2f;
            }
            else if (MovementState == 2) //Catch up to player
            {
                Projectile.tileCollide = false;
                Projectile.spriteDirection = p.position.X < Projectile.position.X ? -1 : 1;
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
                    Projectile.velocity = Vector2.Normalize(p.position - Projectile.position) * 7 * mult;
                    Projectile.rotation += 0.4f * mult;

                    if (Vector2.Distance(p.position, Projectile.position) < DistanceUntilReturn * 0.6f)
                    {
                        MovementState = 3;
                        Timer = 0;
                    }
                }
            }
            else if (MovementState == 3) //Deccelerate
            {
                Projectile.tileCollide = true;
                float mult = 1 - (Timer / 240f);
                Projectile.velocity.X *= mult;
                Projectile.velocity.Y += 0.2f;
                Projectile.rotation += 0.4f * mult;

                if (Timer > 60)
                {
                    Timer = 0;
                    MovementState = 1;
                    Projectile.rotation = 0;
                    Projectile.spriteDirection = Main.rand.NextBool(2) ? -1 : 1;
                }
            }
            else if (MovementState == 4) //ENEMY DETECTED
            {
                Projectile.tileCollide = true;

                if (Timer == AnimSpeedMultHasty)
                    SetFrame(2);
                if (Timer == AnimSpeedMultHasty * 2)
                    SetFrame(3);
                if (Timer == AnimSpeedMultHasty * 3)
                    SetFrame(4);
                if (Timer > AnimSpeedMultHasty * 3)
                {
                    if (_target != -2 && Main.npc[_target].active)
                        Projectile.velocity += Vector2.Normalize(Main.npc[_target].Center - Projectile.Center) * 0.4f;
                    if (Projectile.velocity.Length() > 7f)
                        Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 7f;
                    if (Timer >= AnimSpeedMultHasty * 36 || _target == -2 || !Main.npc[_target].active || Projectile.velocity.Length() < 0.1f)
                    {
                        Projectile.velocity.Y += 0.2f;
                        Projectile.velocity.X *= 0.9999f;
                        if (Timer >= AnimSpeedMultHasty * 40)
                        {
                            _target = -100; //Target cooldown
                            MovementState = 3;
                            Timer = 0;
                        }
                    }
                    else
                        Projectile.rotation += 0.4f;
                }
            }

            if (_skin == 2)
                Lighting.AddLight(Projectile.Center - new Vector2(0, 10), new Vector3(0.1f, 0.03f, 0.06f) * 6f);
        }

        private void SetFrame(int frame) => Projectile.frame = frame + (_skin * 5);

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Slimed, 20);
            Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.Next(-70, 71) * 0.01f) * -1f;
        }
    }
}
