using Microsoft.Xna.Framework;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.NPCs.Verdant.Enemy
{
    public class HostFly : ModNPC
    {
        public int[] babies = new int[6];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hostfly");
        }

        public override void SetDefaults()
        {
            npc.width = 76;
            npc.height = 38;
            npc.damage = 52;
            npc.defense = 17;
            npc.lifeMax = 80;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.dontTakeDamage = false;
            npc.value = Item.buyPrice(0, 0, 3, 25);
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void AI()
        {
            npc.TargetClosest(true);
            Player target = Main.player[npc.target];

            if (npc.ai[0] == 0) //Spawn babies
            {
                if (Main.expertMode)
                    babies = new int[9];
                for (int i = 0; i < babies.Length; ++i)
                {
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<SmallFly>(), 0, 1);
                    babies[i] = n;
                }
                npc.ai[0] = 1;
            }
            else if (npc.ai[0] == 1)
            {
                for (int i = 0; i < babies.Length; ++i) //Redo baby's center
                {
                    Main.npc[babies[i]].ai[2] = npc.Center.X;
                    Main.npc[babies[i]].ai[3] = npc.Center.Y;
                }

                if (Collision.CanHitLine(npc.position, npc.width, npc.height, target.position, target.width, target.height) && !target.dead) //Go to player
                {
                    float extraSpeed = 1f + ((babies.Count(x => Main.npc[x].type == NPCType<HostFly>() && Main.npc[x].life > 1 && Main.npc[x].ai[0] == 1) / (float)babies.Length) * 5f);
                    npc.velocity = Vector2.Normalize(target.Center - npc.Center) * (Main.expertMode ? 2.4f : 1.6f) * extraSpeed;
                    npc.ai[1] = 0;
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                }
                else //Idle
                {
                    npc.ai[1]++;
                    if (npc.ai[1] == 110)
                    {
                        npc.ai[2] = npc.position.X + Main.rand.Next(70, 170) * (Main.rand.Next(2) == 0 ? -1 : 1);
                        npc.ai[3] = npc.position.Y + Main.rand.Next(70, 170) * (Main.rand.Next(2) == 0 ? -1 : 1);
                    }

                    if (Vector2.Distance(npc.position, new Vector2(npc.ai[2], npc.ai[3])) < 20)
                    {
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }

                    if (npc.ai[2] != 0) npc.velocity = Vector2.Normalize(new Vector2(npc.ai[2], npc.ai[3]) - npc.position) * 2.5f;
                    else npc.velocity *= 0.98f;
                }
            }

            if (npc.velocity.X > 0) npc.spriteDirection = 1;
            else npc.spriteDirection = -1;

            npc.netUpdate = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            //writer.Write(babies);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            //babies = reader.ReadInt32();
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ % 16 <= 3)
                npc.frame.Y = 0;
            else if (npc.frameCounter % 16 <= 7)
                npc.frame.Y = 42;
            else if (npc.frameCounter % 16 <= 11)
                npc.frame.Y = 84;
            else
                npc.frame.Y = 126;
        }

        public override bool CheckDead()
        {
            for (int i = 0; i < babies.Length; ++i) //Reset baby AIs
            {
                Main.npc[babies[i]].ai[0] = 0;
                Main.npc[babies[i]].ai[1] = 60;
            }
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 9; ++i)
                    Gore.NewGore(npc.Center, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3)), Main.rand.Next(3) != 0 ? mod.GetGoreSlot("Gores/Verdant/LushLeaf") : mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling"));
                for (int i = 0; i < 12; ++i)
                    Dust.NewDust(npc.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant) && !spawnInfo.playerInTown ? 0.1f : 0f;
    }
}
