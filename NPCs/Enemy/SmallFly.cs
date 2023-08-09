using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.NPCs.Enemy
{
    public class SmallFly : ModNPC
    {
        private short distanceToHost = -1;

        public override void SetStaticDefaults() => Main.npcFrameCount[NPC.type] = 2;

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 18;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = false;
            NPC.value = 0;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.Critter;
            NPC.DeathSound = SoundID.Critter;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A tiny creature made of leaf, petal and flesh. Skittish and harmless."),
            });
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];

            if (distanceToHost == -1)
                distanceToHost = (short)Main.rand.Next(30, 110);

            if (NPC.ai[0] == 0) //Alone
            {
                NPC.ai[1]++; //Timer
                if (NPC.ai[1] == 90) //If timer elapses a set time
                {
                    NPC.ai[2] = NPC.position.X + Main.rand.Next(70, 170) * (Main.rand.NextBool(2)? -1 : 1); //Find a position in the world
                    NPC.ai[3] = NPC.position.Y + Main.rand.Next(70, 170) * (Main.rand.NextBool(2)? -1 : 1);

                    if (Vector2.Distance(NPC.position, target.position) < 240) //If the player is too close, find a position away from the player in the world
                    {
                        Vector2 offset = -Vector2.Normalize(target.position - NPC.position) * (Main.rand.Next(120, 200));
                        NPC.ai[2] = NPC.position.X + offset.X;
                        NPC.ai[3] = NPC.position.Y + offset.Y;
                        NPC.netUpdate = true;
                    }
                }

                float mult = (Vector2.Distance(NPC.position, target.position) < 240) ? 1.5f : 1f; //Extra speed if the player is too close

                if (NPC.ai[2] != 0) NPC.velocity = Vector2.Normalize(new Vector2(NPC.ai[2], NPC.ai[3]) - NPC.position) * (3.5f * mult); //Speed
                else NPC.velocity *= 0.975f; //Slow down when not going to a place

                if (Vector2.Distance(NPC.position, new Vector2(NPC.ai[2], NPC.ai[3])) < 20) //Go to position chosen
                {
                    NPC.ai[1] = Main.rand.Next(10);
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                }

                if (NPC.frameCounter++ % 6 <= 2) NPC.frame.Y = 20; //Animate
                else NPC.frame.Y = 0;
            }
            else if (NPC.ai[0] == 1) //With host fly
            {
                if (NPC.ai[1] == 0)
                {
                    NPC.velocity = new Vector2(3.75f, 0).RotatedByRandom(3.14);
                    if (Main.expertMode)
                        NPC.velocity = new Vector2(4.5f, 0).RotatedByRandom(3.14);
                    NPC.ai[1] = 1;
                }
                else
                {
                    NPC.velocity = NPC.velocity.RotatedByRandom(0.45);
                    if (Vector2.Distance(NPC.Center, new Vector2(NPC.ai[2], NPC.ai[3])) > distanceToHost)
                        NPC.velocity = Vector2.Normalize(new Vector2(NPC.ai[2], NPC.ai[3]) - NPC.Center) * 4.5f;
                }

                if (NPC.frameCounter++ % 6 <= 2)
                    NPC.frame.Y = 20;
                else
                    NPC.frame.Y = 0;
            }

            if (NPC.velocity.X > 0) NPC.spriteDirection = 1;
            else NPC.spriteDirection = -1;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 2; ++i)
                    Gore.NewGore(NPC.GetSource_OnHurt(null), NPC.Center, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3)), Mod.Find<ModGore>("LushLeaf").Type);
                for (int i = 0; i < 6; ++i)
                    Dust.NewDust(NPC.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant) && !spawnInfo.PlayerInTown ? (spawnInfo.Water ? 1.4f : 1f) : 0f;
    }
}
