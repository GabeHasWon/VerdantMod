using Microsoft.Xna.Framework;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.NPCs.Enemy
{
    public class HostFly : ModNPC
    {
        public int[] babies = new int[6];

        public override bool IsLoadingEnabled(Mod mod) => VerdantMod.DebugModActive;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hostess Winglet [Removed]");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 76;
            NPC.height = 38;
            NPC.damage = 52;
            NPC.defense = 17;
            NPC.lifeMax = 80;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.dontTakeDamage = false;
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
                new FlavorTextBestiaryInfoElement("The matriarch of a small fly colony, angry that its children are being attacked. Seems to be extinct naturally, despite the commonality of the Lush Winglet."),
            });
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player target = Main.player[NPC.target];

            if (NPC.ai[0] == 0) //Spawn babies
            {
                if (Main.expertMode)
                    babies = new int[9];
                for (int i = 0; i < babies.Length; ++i)
                {
                    int n = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SmallFly>(), 0, 1);

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, n);

                    babies[i] = n;
                }
                NPC.ai[0] = 1;
                NPC.netUpdate = true;   
            }
            else if (NPC.ai[0] == 1)
            {
                for (int i = 0; i < babies.Length; ++i) //Redo baby's center
                {
                    Main.npc[babies[i]].ai[2] = NPC.Center.X;
                    Main.npc[babies[i]].ai[3] = NPC.Center.Y;
                }

                if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, target.position, target.width, target.height) && !target.dead) //Go to player
                {
                    float extraSpeed = 1f + (babies.Count(x => Main.npc[x].type == ModContent.NPCType<HostFly>() && Main.npc[x].life > 1 && Main.npc[x].ai[0] == 1) / (float)babies.Length * 5f);
                    NPC.velocity = Vector2.Normalize(target.Center - NPC.Center) * (Main.expertMode ? 2.4f : 1.6f) * extraSpeed;
                    NPC.ai[1] = 0;
                    NPC.ai[2] = 0;
                    NPC.ai[3] = 0;
                    NPC.netUpdate = true;
                }
                else //Idle
                {
                    NPC.ai[1]++;
                    if (NPC.ai[1] == 110)
                    {
                        NPC.ai[2] = NPC.position.X + Main.rand.Next(70, 170) * (Main.rand.NextBool(2) ? -1 : 1);
                        NPC.ai[3] = NPC.position.Y + Main.rand.Next(70, 170) * (Main.rand.NextBool(2) ? -1 : 1);
                        NPC.netUpdate = true;
                    }

                    if (Vector2.Distance(NPC.position, new Vector2(NPC.ai[2], NPC.ai[3])) < 20)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                        NPC.ai[3] = 0;
                        NPC.netUpdate = true;
                    }

                    if (NPC.ai[2] != 0) NPC.velocity = Vector2.Normalize(new Vector2(NPC.ai[2], NPC.ai[3]) - NPC.position) * 2.5f;
                    else NPC.velocity *= 0.98f;
                }
            }

            if (NPC.velocity.X > 0) NPC.spriteDirection = 1;
            else NPC.spriteDirection = -1;

            NPC.netUpdate = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            for (int i = 0; i < babies.Length; ++i)
                writer.Write(babies[i]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            for (int i = 0; i < babies.Length; ++i)
                babies[i] = reader.ReadInt32();
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ % 16 <= 3)
                NPC.frame.Y = 0;
            else if (NPC.frameCounter % 16 <= 7)
                NPC.frame.Y = 42;
            else if (NPC.frameCounter % 16 <= 11)
                NPC.frame.Y = 84;
            else
                NPC.frame.Y = 126;
        }

        public override bool CheckDead()
        {
            for (int i = 0; i < babies.Length; ++i) //Reset baby AIs
            {
                Main.npc[babies[i]].ai[0] = 0;
                Main.npc[babies[i]].ai[1] = 60;
                Main.npc[babies[i]].netUpdate = true;
                NPC.netUpdate = true;
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 9; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3)), !Main.rand.NextBool(3) ? Mod.Find<ModGore>("LushLeaf").Type : Mod.Find<ModGore>("PinkPetalFalling").Type);
                for (int i = 0; i < 12; ++i)
                    Dust.NewDust(NPC.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => 0f; //Temporarily(?) removed
    }
}