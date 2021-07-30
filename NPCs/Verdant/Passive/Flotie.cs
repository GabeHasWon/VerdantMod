using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;

namespace Verdant.NPCs.Verdant.Passive
{
    public class Flotie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcCatchable[npc.type] = true;
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.width = 36;
            npc.height = 48;
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
            npc.catchItem = (short)ModContent.ItemType<FlotieItem>();
        }

        public override void AI()
        {
            npc.TargetClosest(true);

            if (npc.ai[1] == 0)
            {
                npc.ai[0] = Main.rand.Next(1, 50);
                npc.ai[1] = 1;
                npc.ai[2] = Main.rand.Next(70, 100) * 0.01f;
                npc.ai[3] = Main.rand.Next(110, 161) * 0.01f * (Main.rand.NextBool() ? -1 : 1);
            }

            npc.rotation = npc.velocity.X * 0.4f;
            npc.velocity.Y = (float)(Math.Sin(npc.ai[0]++ * 0.02f * npc.ai[2]) * 0.6f);
            npc.velocity.X = (float)(Math.Sin(npc.ai[0]++ * 0.006f) * 0.15f) * npc.ai[3];

            if (npc.velocity.Y < 0.01f)
                npc.frame.Y = 0;
            else
                npc.frame.Y = 50;

            Lighting.AddLight(npc.position, new Vector3(0.5f, 0.16f, 0.30f) * 1.4f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 6; ++i)
                    Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), Vector2.Zero, mod.GetGoreSlot("Gores/Verdant/LushLeaf"));
                for (int i = 0; i < 3; ++i)
                    Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), Vector2.Zero, mod.GetGoreSlot("Gores/Verdant/RedPetalFalling"));
                for (int i = 0; i < 12; ++i)
                    Dust.NewDust(npc.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ModContent.ItemType<LushLeaf>(), Main.rand.Next(1, 3));
            Item.NewItem(npc.getRect(), ModContent.ItemType<RedPetal>(), 1);
            if (Main.rand.NextBool(10))
                Item.NewItem(npc.getRect(), ModContent.ItemType<Lightbulb>(), 1);
        }

        public override int SpawnNPC(int tileX, int tileY)
        {
            int rnd = Main.rand.Next(4);
            for (int i = 0; i < rnd; ++i)
                NPC.NewNPC((tileX * 16) + Main.rand.Next(-80, 80), (tileY * 16) + Main.rand.Next(-140, 140), ModContent.NPCType<Flotiny>());
            return base.SpawnNPC(tileX, tileY);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant && (spawnInfo.playerInTown || spawnInfo.playerSafe))
                return 1.5f + (spawnInfo.water ? 0.4f : 0f);
            return (spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant) ? (spawnInfo.water ? 1f : 0.6f) : 0f;
        }
    }
}