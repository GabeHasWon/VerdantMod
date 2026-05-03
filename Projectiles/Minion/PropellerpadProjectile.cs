using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Buffs.Minion;
using Verdant.Dusts;
using Verdant.Gores.Verdant;
using Verdant.Players;

namespace Verdant.Projectiles.Minion;

class PropellerpadProjectile : ModProjectile
{
    const int MaxFlightTime = 600;

    private enum AIState
    {
        Idle,
        PlayerHanging
    }

    AIState State
    {
        get => (AIState)Projectile.ai[0];
        set => Projectile.ai[0] = (float)value;
    }

    private ref float FlightTime => ref Projectile.ai[1];

    private bool Init
    {
        get => Projectile.ai[2] == 1;
        set => Projectile.ai[2] = value ? 1 : 0;
    }

    Player Owner => Main.player[Projectile.owner];

    public override void SetStaticDefaults() => Main.projFrames[Type] = 4;

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.OneEyedPirate);
        Projectile.aiStyle = -1;
        Projectile.width = 74;
        Projectile.height = 56;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.minion = false;
        Projectile.minionSlots = 0;

        AIType = ProjectileID.None;
    }

    public override void AI()
    {
        if (!Init)
        {
            Init = true;

            Owner.AddBuff(ModContent.BuffType<PropellerpadBuff>(), 2);
        }

        if (!Owner.HasBuff<PropellerpadBuff>())
            Projectile.Kill();

        if (Projectile.Center.HasNaNs())
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.position = Owner.Center - new Vector2(0, 80) + Projectile.Size / 2f;
        }

        Projectile.timeLeft = 2;
        Owner.gravity *= 0.15f;

        if (Main.rand.NextBool(State == AIState.PlayerHanging ? 1 : 3))
        {
            float factor = FlightTime / MaxFlightTime;
            Vector2 dustPos = Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), 8);
            Dust.NewDustPerfect(dustPos, ModContent.DustType<WindLine>(), new Vector2(0, 5 + Projectile.velocity.Y + factor * Main.rand.NextFloat(7, 12)));
        }

        if (State == AIState.Idle)
            Idle();
        else
            Hanging();
    }

    public override void PostAI()
    {
        if (State == AIState.PlayerHanging)
        {
            Owner.Center = Projectile.Center + new Vector2(0, 10) + Projectile.velocity;
            Owner.bodyFrame.Y = 56 * 3;
        }
        else if (Projectile.DistanceSQ(Owner.Center) > 1000 * 1000)
        {
            TeleportVFX();
            Projectile.Center = Owner.Center - new Vector2(0, 80);
            Projectile.velocity = Vector2.Zero;
            TeleportVFX();
        }
    }

    private void TeleportVFX()
    {
        if (Main.dedServ)
            return;

        SoundEngine.PlaySound(SoundID.Grass with { PitchRange = (-0.2f, 0.4f), Volume = 0.75f }, Projectile.Center);

        for (int i = 0; i < 14; ++i)
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height / 2, DustID.Grass);

            if (i < 3)
            {
                Vector2 position = Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(12));
                Gore.NewGore(Projectile.GetSource_FromThis(), position, Vector2.Zero, ModContent.GoreType<LushLeaf>());
            }
        }
    }

    private void Hanging()
    {
        Owner.GetModPlayer<PropellerpadPlayer>().onPropellerpad = true;

        Projectile.height = 65;
        Owner.gfxOffY = 0;

        if (FlightTime <= 0)
        {
            State = AIState.Idle;
            Owner.velocity = new Vector2(0, -10f);
        }
        else
        {
            const float VerticalMoveSpeed = 0.2f;
            const float HorizontalMoveSpeed = 0.2f;

            const float MaxVerticalSpeed = 6;
            const float MaxHorizontalSpeed = 9;

            const float SlowdownCutoff = 120;

            if (Owner.controlUp)
                Projectile.velocity.Y -= VerticalMoveSpeed;

            if (Owner.controlDown)
                Projectile.velocity.Y += VerticalMoveSpeed;

            if (Owner.controlRight)
                Projectile.velocity.X += HorizontalMoveSpeed;

            if (Owner.controlLeft)
                Projectile.velocity.X -= HorizontalMoveSpeed;

            if (Owner.controlJump || Owner.mount.Active)
                State = AIState.Idle;

            float mod = FlightTime < SlowdownCutoff ? FlightTime / SlowdownCutoff : 1f;
            Projectile.velocity = Vector2.Clamp(Projectile.velocity, new Vector2(-MaxHorizontalSpeed, -MaxVerticalSpeed) * mod, new Vector2(MaxHorizontalSpeed, Owner.maxFallSpeed) * mod);
            Projectile.velocity.Y += Owner.gravity;
            Projectile.velocity.X *= 0.99f;

            if (Collision.SolidCollision(Owner.BottomLeft, Owner.width, 6) && Projectile.velocity.Y > 0)
                Projectile.velocity.Y = 0;

            Owner.Center = Projectile.Center + new Vector2(0, 4) + Projectile.velocity;
            Owner.velocity = Projectile.velocity;
            Owner.fallStart = (int)Projectile.Center.Y / 16;

            if (!Owner.empressBrooch)
                FlightTime--;

            if (Projectile.frameCounter++ > 4 - (mod * 3))
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }

            int y = (int)(Projectile.Center.Y / 16f) + 1;
            while (!WorldGen.SolidOrSlopedTile((int)(Projectile.position.X / 16), y++)) { }

            int y2 = (int)(Projectile.Center.Y / 16f) + 1;
            while (!WorldGen.SolidOrSlopedTile((int)((Projectile.position.X + Projectile.width) / 16), y2++)) { }

            y = Math.Min(y, y2);

            if (y - (Projectile.Center.Y / 16f) < 10)
            {
                Projectile.velocity.Y -= VerticalMoveSpeed * 1.5f;

                if (Projectile.velocity.Y > 0)
                    Projectile.velocity.Y *= 0.92f;
            }
        }
    }

    private void Idle()
    {
        Projectile.height = 56;

        Vector2 center = Owner.Center - new Vector2(0, 80);
        Projectile.velocity += Projectile.DirectionTo(center) * 0.08f;

        if (Projectile.DistanceSQ(center) < 400 * 400)
            Projectile.velocity *= 0.98f;

        if (Projectile.velocity.LengthSquared() > 16 * 16)
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 8;

        if (Owner.Hitbox.Intersects(GrabHitbox()) && Owner.controlUp && FlightTime > 0 && !Owner.mount.Active && Owner.GetModPlayer<ZipvinePlayer>().zipvine == null)
            State = AIState.PlayerHanging;

        if (Collision.SolidCollision(Owner.BottomLeft, Owner.width, 6, true))
        {
            FlightTime = MathHelper.Min(FlightTime + 2.5f, MaxFlightTime);

            if (Main.rand.NextBool(12))
            {
                Dust d = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(Projectile.Hitbox with { Height = 20 }), DustID.Grass, Main.rand.NextVector2Circular(2, 2));
                d.noGravity = true;
                d.alpha = 155;
                d.scale = Main.rand.NextFloat(1, 2);
                d.velocity.Y = -Math.Abs(d.velocity.Y);
            }
        }

        if (Projectile.frameCounter++ > 4)
        {
            Projectile.frameCounter = 0;
            if (++Projectile.frame >= Main.projFrames[Type])
                Projectile.frame = 0;
        }
    }

    private Rectangle GrabHitbox()
    {
        var loc = Projectile.Center - new Vector2(6, 4);
        return new Rectangle((int)loc.X, (int)loc.Y, 18, 40);
    }

    public override bool MinionContactDamage() => false;
    public override bool? CanCutTiles() => false;
    public override bool OnTileCollide(Vector2 oldVelocity) => false;

    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        fallThrough = true;
        return true;
    }
}
