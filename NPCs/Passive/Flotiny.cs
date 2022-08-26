using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;

namespace Verdant.NPCs.Passive
{
    public class Flotiny : ModNPC
    {
        public override void SetStaticDefaults() => Main.npcCatchable[NPC.type] = true;

        public override void SetDefaults()
        {
            NPC.width = 22;
            NPC.height = 26;
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
            NPC.catchItem = (short)ModContent.ItemType<FlotinyItem>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A tiny floating creature. Despite its clear flight, or at least hovering, it makes no sound and doesn't move apart from gentle swaying."),
            });
        }

        public override void AI()
        {
            NPC.TargetClosest(true);

            if (NPC.ai[1] == 0)
            {
                NPC.ai[0] = Main.rand.Next(50);
                NPC.ai[1] = 1;
                NPC.ai[2] = Main.rand.Next(90, 131) * 0.01f * (Main.rand.NextBool() ? -1 : 1);
                NPC.ai[3] = Main.rand.Next(100, 121) * 0.01f * (Main.rand.NextBool() ? -1 : 1);
            }

            NPC.rotation = NPC.velocity.X * 0.25f;
            NPC.velocity.Y = (float)(Math.Sin(NPC.ai[0]++ * 0.02f) * 0.4f) * NPC.ai[2];
            NPC.velocity.X = (float)(Math.Sin(NPC.ai[0]++ * 0.006f) * 0.05f);

            Lighting.AddLight(NPC.position, new Vector3(0.5f, 0.16f, 0.30f) * 1.0f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 3; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)), Vector2.Zero, Mod.Find<ModGore>("LushLeaf").Type);
                for (int i = 0; i < 4; ++i)
                    Dust.NewDust(NPC.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && spawnInfo.PlayerInTown)
                return 2f + (spawnInfo.Water ? 1f : 0f);
            return (spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant) ? ((spawnInfo.Water) ? 1.2f : 0.8f) : 0f;
        }
    }
}