using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;

namespace Verdant.NPCs.Passive
{
    public class Flotie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 36;
            NPC.height = 48;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = false;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.dontCountMe = true;
            NPC.catchItem = (short)ModContent.ItemType<FlotieItem>();
        }

        public override void AI()
        {
            NPC.TargetClosest(true);

            if (NPC.ai[1] == 0)
            {
                NPC.ai[0] = Main.rand.Next(1, 50);
                NPC.ai[1] = 1;
                NPC.ai[2] = Main.rand.Next(70, 100) * 0.01f;
                NPC.ai[3] = Main.rand.Next(110, 161) * 0.01f * (Main.rand.NextBool() ? -1 : 1);
            }

            NPC.rotation = NPC.velocity.X * 0.4f;
            NPC.velocity.Y = (float)(Math.Sin(NPC.ai[0]++ * 0.02f * NPC.ai[2]) * 0.6f);
            NPC.velocity.X = (float)(Math.Sin(NPC.ai[0]++ * 0.006f) * 0.15f) * NPC.ai[3];

            if (NPC.velocity.Y < 0.01f)
                NPC.frame.Y = 0;
            else
                NPC.frame.Y = 50;

            Lighting.AddLight(NPC.position, new Vector3(0.5f, 0.16f, 0.30f) * 1.4f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 6; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)), Vector2.Zero, Mod.Find<ModGore>("LushLeaf").Type);
                for (int i = 0; i < 3; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)), Vector2.Zero, Mod.Find<ModGore>("RedPetalFalling").Type);
                for (int i = 0; i < 12; ++i)
                    Dust.NewDust(NPC.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.AddCommon<LushLeaf>(1, 1, 2);
            npcLoot.AddCommon<RedPetal>();
            npcLoot.AddCommon<Lightbulb>(10);
        }

        public override int SpawnNPC(int tileX, int tileY)
        {
            int rnd = Main.rand.Next(4);
            for (int i = 0; i < rnd; ++i)
                NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (tileX * 16) + Main.rand.Next(-80, 80), (tileY * 16) + Main.rand.Next(-140, 140), ModContent.NPCType<Flotiny>());
            return base.SpawnNPC(tileX, tileY);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && (spawnInfo.PlayerInTown || spawnInfo.PlayerSafe))
                return 1.5f + (spawnInfo.Water ? 0.4f : 0f);
            return (spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant) ? (spawnInfo.Water ? 1f : 0.6f) : 0f;
        }
    }
}