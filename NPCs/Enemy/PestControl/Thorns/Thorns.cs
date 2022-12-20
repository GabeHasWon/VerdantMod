using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Drawing;
using Verdant.Projectiles.Particles;

namespace Verdant.NPCs.Enemy.PestControl.Thorns
{
    internal class SmallThorn : ModNPC, IDrawAdditive
    {
        private const int ExplosionCutoff = 220;

        private static Asset<Texture2D> auraTex;

        protected virtual Vector2 Size => new(10, 22);
        protected virtual string BestiaryText => "The branch of a Thorny Beholder, which explodes when approached. Holds eye contact well...too well.";
        protected virtual int ExplosionRadius => 80;

        protected int ExplosionRadiusSquared => ExplosionRadius * ExplosionRadius;

        private ref float Frame => ref NPC.ai[0];
        private ref float Timer => ref NPC.ai[1];
        private ref float BombTimer => ref NPC.ai[2];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thorn");
            Main.npcFrameCount[NPC.type] = 1;

            auraTex = ModContent.Request<Texture2D>("Verdant/NPCs/Enemy/PestControl/Thorns/ThornAura");
        }

        public override void SetDefaults()
        {
            NPC.width = (int)Math.Min(Size.X, Size.Y);
            NPC.height = (int)Math.Min(Size.X, Size.Y);
            NPC.damage = 50;
            NPC.defense = 30;
            NPC.lifeMax = 70;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(0, 0, 0, 10);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.Critter;
            NPC.DeathSound = SoundID.NPCDeath4;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
            {
                new FlavorTextBestiaryInfoElement(BestiaryText),
            });
        }

        public override void AI()
        {
            Timer++;

            if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height) && Timer > 20)
            {
                NPC.velocity = Vector2.Zero;
                GroundedBehaviour();
            }
            else
            {
                NPC.velocity.Y += 0.3f;
                NPC.rotation = NPC.velocity.ToRotation() - MathHelper.PiOver2;
            }
        }

        private void GroundedBehaviour()
        {
            if (BombTimer == 0)
                SoundEngine.PlaySound(SoundID.Dig with { Volume = 0.75f, PitchVariance = 0.2f, Pitch = 0.5f }, NPC.Center);

            BombTimer++;

            if (BombTimer > ExplosionCutoff)
            {
                int closest = Player.FindClosest(NPC.position, NPC.width, NPC.height);
                Player nearest = Main.player[closest];

                if (nearest.active && !nearest.dead && nearest.DistanceSQ(NPC.Center) < ExplosionRadiusSquared)
                    Detonate();
            }
        }

        private void Detonate()
        {
            for (int i = 0; i < Main.maxPlayers; ++i)
            {
                Player plr = Main.player[i];

                if (plr.active && !plr.dead && plr.DistanceSQ(NPC.Center) < ExplosionRadiusSquared * 1.2f)
                    plr.Hurt(PlayerDeathReason.ByCustomReason($"{plr.name} couldn't handle the corruption."), Main.DamageVar(120), 0);
            }

            NPC.life = 0;
            NPC.active = false;

            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion with { Volume = 0.75f, PitchVariance = 0.2f }, NPC.Center);

            float increase = MathF.Pow(ExplosionRadius / 80f, 1.2f);
            for (int i = 0; i < 25 * increase; ++i)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, Main.rand.NextFloat(-6, 6) * increase, Main.rand.NextFloat(-6, 6) * increase);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, Main.rand.NextFloat(-6, 6) * increase, Main.rand.NextFloat(-6, 6) * increase);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Main.rand.NextVector2Circular(6, 6) * increase, ModContent.ProjectileType<Smoke>(), 0, 0);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (Frame == 0)
                Frame = Main.rand.Next(3) + 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            var frame = new Rectangle((int)((Size.X + 2) * (Frame - 1)), 0, (int)Size.X, (int)Size.Y);
            Main.EntitySpriteDraw(tex, NPC.Center - screenPos, frame, drawColor, NPC.rotation, frame.Size() / 2, 1f, SpriteEffects.None, 0);
            return false;
        }

        void IDrawAdditive.DrawAdditive(AdditiveLayer layer)
        {
            const int MinTimer = ExplosionCutoff - 60;

            if (BombTimer < MinTimer || layer == AdditiveLayer.AfterPlayer)
                return;

            float scale = ExplosionRadius / 40f;
            float factor = (BombTimer - MinTimer) / (ExplosionCutoff - MinTimer);

            if (BombTimer <= ExplosionCutoff)
                scale *= factor;

            float rot = (BombTimer * (1 / scale) * 0.005f) * (NPC.whoAmI % 2 == 0 ? -1 : 1);
            Color color = Color.Lerp(new Color(111, 113, 93), new Color(142, 0, 0), factor);

            Main.spriteBatch.Draw(auraTex.Value, NPC.Center - Main.screenPosition, null, color, rot, auraTex.Value.Size() / 2, scale, SpriteEffects.None, 0);
        }
    }

    internal class BigThorn : SmallThorn
    {
        protected override Vector2 Size => new(16, 32);
        protected override string BestiaryText => "A larger branch from a Thorny Beholder. Does not hold eye contact as well as its smaller brethren. It is significantly more dangerous though.";
        protected override int ExplosionRadius => 125;

        public override void SetDefaults()
        {
            base.SetDefaults();

            NPC.lifeMax = 100;
            NPC.damage = 80;
        }
    }
}
