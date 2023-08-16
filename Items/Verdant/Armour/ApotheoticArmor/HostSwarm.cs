using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Armour.ApotheoticArmor;

internal class HostSwarm : ModProjectile
{
    const int MaxBees = 25;
    const float MaxTimeLeft = 20;

    public static Asset<Texture2D> BeeTexture;

    private Player Owner => Main.player[Projectile.owner];
    private bool Active => !Owner.dead && !Main.isMouseLeftConsumedByUI && Owner.HeldItem.CountsAsClass(DamageClass.Summon) && Owner.HeldItem.damage > 0;

    private List<HostSwarmBee> bees = null;

    private bool Remove
    {
        get => Projectile.ai[0] == 1;
        set => Projectile.ai[0] = value ? 1 : 0;
    }

    private ref float Timer => ref Projectile.ai[1];

    private float aimX = 0;
    private float aimY = 0;

    public override void SetStaticDefaults() => BeeTexture = ModContent.Request<Texture2D>(Texture + "Bee");
    public override void Unload() => BeeTexture = null;

    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.width = Projectile.height = 70;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.extraUpdates = 1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 7;
        Projectile.timeLeft = (int)MaxTimeLeft;

        AIType = 0;
    }

    public override void AI()
    {
        SetBees();
        Projectile.velocity = Vector2.Zero;
        
        if (!Remove)
            Projectile.timeLeft++;

        foreach (var item in bees)
            item.Update(Remove);

        if ((!Remove && !Owner.GetModPlayer<HostHelmetPlayer>().active) || Owner.dead)
            Remove = true;

        int damage = Owner.GetModPlayer<HostHelmetPlayer>().setBonus ? 10 : 6;

        if (Owner.strongBees)
            damage = (int)(damage * 1.25f);

        Projectile.damage = (int)Owner.GetDamage(DamageClass.Summon).ApplyTo(damage);

        if (Main.myPlayer == Projectile.owner)
        {
            Vector2 target = Active ? Main.MouseWorld : Owner.Center - new Vector2(0, 30);
            Owner.LimitPointToPlayerReachableArea(ref target);

            if (aimX != target.X || aimY != target.Y)
            {
                Projectile.netUpdate = true;
                aimX = target.X;
                aimY = target.Y;
            }

            Projectile.Center = Vector2.Lerp(Projectile.Center, target, Active ? 0.03f : 0.05f);
        }
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(aimX);
        writer.Write(aimY);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        aimX = reader.ReadSingle();
        aimY = reader.ReadSingle();
    }

    public override void PostDraw(Color lightColor)
    {
        SetBees();

        Timer++;
        float alpha = Remove ? Projectile.timeLeft / MaxTimeLeft : 1f;

        for (int i = 0; i < bees.Count; ++i)
            bees[i].Draw(alpha, i + (int)Timer);
    }

    private void SetBees()
    {
        if (bees is null)
        {
            bees = new List<HostSwarmBee>(MaxBees);

            for (int i = 0; i < MaxBees; ++i)
                bees.Add(new(Projectile));
        }
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => modifiers.FinalDamage.Base += Projectile.damage;
    public override bool? CanHitNPC(NPC target) => Active ? null : false;
    public override bool? CanCutTiles() => false;

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (target.life <= 0 && !target.SpawnedFromStatue)
        {
            var item = Owner.QuickSpawnItemDirect(target.GetSource_OnHurt(Projectile), ModContent.ItemType<HoneyHeart>());
            item.Center = target.Center;
        }
    }

    /// <summary>
    /// Controls a single bee. Client side.
    /// </summary>
    internal class HostSwarmBee
    {
        internal Vector2 position;
        internal Vector2 velocity;
        internal Projectile parent;
        internal Vector2 scale;

        public HostSwarmBee(Projectile parent)
        {
            this.parent = parent;

            position = Vector2.Zero;
            scale = new(Main.rand.NextFloat(0.6f, 1.4f));
            velocity = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(1f, 3f);
        }

        public void Update(bool fade)
        {
            const float MaxDistance = 40;

            position += velocity;

            if (!fade && position.LengthSquared() > MaxDistance * MaxDistance)
            {
                velocity *= -1;
                position += velocity;
            }
            else
                velocity = velocity.RotatedByRandom(0.08f);
        }

        public void Draw(float alpha, int timer)
        {
            var realPos = parent.Center + position;
            var light = Lighting.GetColor(realPos.ToTileCoordinates()) * alpha;
            var effect = velocity.X <= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            var source = new Rectangle(14 * (timer % 10 < 5 ? 1 : 0), 0, 12, 14);
            var origin = source.Size() / 2f;

            Main.spriteBatch.Draw(BeeTexture.Value, realPos - Main.screenPosition, source, light, 0f, origin, scale, effect, 0);

            if (Main.player[parent.owner].GetModPlayer<HostHelmetPlayer>().setBonus)
            {
                light = Lighting.GetColor(realPos.ToTileCoordinates(), Color.Yellow) * alpha;
                Main.spriteBatch.Draw(BeeTexture.Value, realPos - Main.screenPosition, source, light * 0.4f, 0f, origin, scale * 1.2f, effect, 0);
                Main.spriteBatch.Draw(BeeTexture.Value, realPos - Main.screenPosition, source, light * 0.15f, 0f, origin, scale * 1.6f, effect, 0);
            }
        }
    }
}