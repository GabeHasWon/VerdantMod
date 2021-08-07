using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;

namespace Verdant.NPCs.Passive
{
    public class Flotiny : ModNPC
    {
        public override void SetStaticDefaults() => Main.npcCatchable[npc.type] = true;

        public override void SetDefaults()
        {
            npc.width = 22;
            npc.height = 26;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = false;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.dontCountMe = true;

            npc.catchItem = (short)ModContent.ItemType<FlotinyItem>();
        }

        public override void AI()
        {
            npc.TargetClosest(true);

            if (npc.ai[1] == 0)
            {
                npc.ai[0] = Main.rand.Next(50);
                npc.ai[1] = 1;
                npc.ai[2] = Main.rand.Next(90, 131) * 0.01f * (Main.rand.NextBool() ? -1 : 1);
                npc.ai[3] = Main.rand.Next(100, 121) * 0.01f * (Main.rand.NextBool() ? -1 : 1);
            }

            npc.rotation = npc.velocity.X * 0.25f;
            npc.velocity.Y = (float)(Math.Sin(npc.ai[0]++ * 0.02f) * 0.4f) * npc.ai[2];
            npc.velocity.X = (float)(Math.Sin(npc.ai[0]++ * 0.006f) * 0.05f);

            Lighting.AddLight(npc.position, new Vector3(0.5f, 0.16f, 0.30f) * 1.0f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 3; ++i)
                    Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), Vector2.Zero, mod.GetGoreSlot("Gores/Verdant/LushLeaf"));
                for (int i = 0; i < 4; ++i)
                    Dust.NewDust(npc.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant && spawnInfo.playerInTown)
                return 2f + (spawnInfo.water ? 1f : 0f);
            return (spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant) ? ((spawnInfo.water) ? 1.2f : 0.8f) : 0f;
        }
    }
}