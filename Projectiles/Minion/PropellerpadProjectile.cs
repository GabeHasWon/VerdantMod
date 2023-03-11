using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;

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

    Player Owner => Main.player[Projectile.owner];

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Razorpad");

        Main.projFrames[Type] = 4;

        ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.OneEyedPirate);
        Projectile.aiStyle = -1;
        Projectile.width = 74;
        Projectile.height = 56;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.minionSlots = 0.75f;
        Projectile.minion = true;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.penetrate = -1;

        AIType = 0;
    }

    public override void AI()
    {
        if (Projectile.Center.HasNaNs())
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.position = Owner.Center - new Vector2(0, 80) + Projectile.Size / 2f;
        }

        Projectile.timeLeft = 2;
        Owner.gravity *= 0.15f;

        if (Main.rand.NextBool(State == AIState.PlayerHanging ? 1 : 3))
            Dust.NewDustPerfect(Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), 8), ModContent.DustType<WindLine>(), new Vector2(0, Main.rand.NextFloat(10, 14) + Projectile.velocity.Y));
        
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
            Projectile.Center = Owner.Center - new Vector2(0, 80);
    }

    private void Hanging()
    {
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

            y = System.Math.Min(y, y2);

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

        if (Owner.Hitbox.Intersects(GrabHitbox()) && Owner.controlUp && FlightTime > 0 && !Owner.mount.Active)
            State = AIState.PlayerHanging;

        if (Collision.SolidCollision(Owner.BottomLeft, Owner.width, 6))
            FlightTime = MathHelper.Min(FlightTime + 2.5f, MaxFlightTime);

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
