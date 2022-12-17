using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.NPCs.Enemy.PestControl
{
    enum ThornState
    {
        Seek,
        Planting,
        Planted
    }

    public class ThornBeholder : ModNPC
    {
        private Player Target => Main.player[NPC.target];

        private ref float Timer => ref NPC.ai[0];
        private ThornState State { get => (ThornState)NPC.ai[1]; set => NPC.ai[1] = (float)value; }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thorny Beholder");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 82;
            NPC.damage = 52;
            NPC.defense = 30;
            NPC.lifeMax = 200;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = Item.buyPrice(0, 0, 3, 25);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.Critter;
            NPC.DeathSound = SoundID.NPCDeath4;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("The eye has seen far too much. A former ent, perhaps even a dryad, lost to the whim of corruption."),
            });
        }

        public override void AI()
        {
            NPC.TargetClosest(true);

            if (State != ThornState.Seek)
            {
                if (State == ThornState.Planting)
                    PlantingAI();
                else
                    PlantedAI();
            }
            else
                SeekAI();
        }

        private void PlantedAI()
        {
            
        }

        private void SeekAI()
        {
            if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, Target.position, Target.width, Target.height))
                State = ThornState.Planting;
        }

        private void PlantingAI()
        {
            Timer++;

            if (Timer <= 10)
                NPC.velocity.Y -= 0.4f;
            else if (Timer <= 100)
                NPC.velocity.Y += 0.4f;

            if (Collision.SolidCollision(NPC.position + new Vector2(0, 28), NPC.width, 10))
            {
                State = ThornState.Planted;
                Timer = 0;

                SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, NPC.position + new Vector2(0, 20));
                Collision.HitTiles(NPC.position + new Vector2(0, 28), NPC.velocity, NPC.width, 10);

                NPC.velocity = Vector2.Zero;
                NPC.netUpdate = true;
            }
        }
    }
}