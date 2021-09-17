using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.NPCs.Enemy
{
    public class SmallFly : ModNPC
    {
        private short distanceToHost = -1;
            
        public override void SetStaticDefaults() => DisplayName.SetDefault("Minifly");

        public override void SetDefaults()
        {
            npc.width = 26;
            npc.height = 18;
            npc.damage = 25;
            npc.defense = 4;
            npc.lifeMax = 25;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = false;
            npc.value = Item.buyPrice(0, 0, 0, 75);
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.HitSound = new Terraria.Audio.LegacySoundStyle(SoundID.Critter, 0);
            npc.DeathSound = new Terraria.Audio.LegacySoundStyle(SoundID.Critter, 0);
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void AI()
        {
            npc.TargetClosest(true);
            Player target = Main.player[npc.target];

            if (distanceToHost == -1)
                distanceToHost = (short)Main.rand.Next(30, 110);

            if (npc.ai[0] == 0) //Alone
            {
                npc.ai[1]++; //Timer
                if (npc.ai[1] == 90) //If timer elapses a set time
                {
                    npc.ai[2] = npc.position.X + Main.rand.Next(70, 170) * (Main.rand.Next(2) == 0 ? -1 : 1); //Find a position in the world
                    npc.ai[3] = npc.position.Y + Main.rand.Next(70, 170) * (Main.rand.Next(2) == 0 ? -1 : 1);

                    if (Vector2.Distance(npc.position, target.position) < 240) //If the player is too close, find a position away from the player in the world
                    {
                        Vector2 offset = -Vector2.Normalize(target.position - npc.position) * (Main.rand.Next(120, 200));
                        npc.ai[2] = npc.position.X + offset.X;
                        npc.ai[3] = npc.position.Y + offset.Y;
                        npc.netUpdate = true;
                    }
                }

                float mult = (Vector2.Distance(npc.position, target.position) < 240) ? 1.5f : 1f; //Extra speed if the player is too close

                if (npc.ai[2] != 0) npc.velocity = Vector2.Normalize(new Vector2(npc.ai[2], npc.ai[3]) - npc.position) * (3.5f * mult); //Speed
                else npc.velocity *= 0.975f; //Slow down when not going to a place

                if (Vector2.Distance(npc.position, new Vector2(npc.ai[2], npc.ai[3])) < 20) //Go to position chosen
                {
                    npc.ai[1] = Main.rand.Next(10);
                    npc.ai[2] = 0;
                    npc.ai[3] = 0;
                }

                if (npc.frameCounter++ % 8 <= 3) npc.frame.Y = 20; //Animate
                else npc.frame.Y = 0;
            }
            else if (npc.ai[0] == 1) //With large fly
            {
                if (npc.ai[1] == 0)
                {
                    npc.velocity = new Vector2(3.75f, 0).RotatedByRandom(3.14);
                    if (Main.expertMode)
                        npc.velocity = new Vector2(4.5f, 0).RotatedByRandom(3.14);
                    npc.ai[1] = 1;
                }
                else
                {
                    npc.velocity = npc.velocity.RotatedByRandom(0.45);
                    if (Vector2.Distance(npc.Center, new Vector2(npc.ai[2], npc.ai[3])) > distanceToHost)
                        npc.velocity = Vector2.Normalize(new Vector2(npc.ai[2], npc.ai[3]) - npc.Center) * 4.5f;
                }

                if (npc.frameCounter++ % 6 <= 2)
                    npc.frame.Y = 20;
                else
                    npc.frame.Y = 0;
            }

            if (npc.velocity.X > 0) npc.spriteDirection = 1;
            else npc.spriteDirection = -1;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 2; ++i)
                    Gore.NewGore(npc.Center, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3)), mod.GetGoreSlot("Gores/Verdant/LushLeaf"));
                for (int i = 0; i < 6; ++i)
                    Dust.NewDust(npc.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant) && !spawnInfo.playerInTown ? (spawnInfo.water ? 1.4f : 1f) : 0f;
    }
}
