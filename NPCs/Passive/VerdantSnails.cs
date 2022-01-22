using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;

namespace Verdant.NPCs.Passive
{
    public class VerdantRedGrassSnail : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leafy Snail");

            Main.npcCatchable[npc.type] = true;
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Snail];
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Snail);
            npc.width = 22;
            npc.height = 18;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            npc.dontCountMe = true;
            npc.catchItem = (short)ModContent.ItemType<FlotinyItem>();

            animationType = NPCID.Snail;
            drawOffsetY = 2;
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
                return 2f;
            return (spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant) ? 0.8f : 0f;
        }
    }

    public class VerdantBulbSnail : VerdantRedGrassSnail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightbulb Snail");

            Main.npcCatchable[npc.type] = true;
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Snail];
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Snail);
            npc.width = 20;
            npc.height = 16;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            npc.dontCountMe = true;
            npc.catchItem = (short)ModContent.ItemType<FlotinyItem>();

            animationType = NPCID.Snail;
        }

        public override void AI() => Lighting.AddLight(npc.Center, Color.HotPink.ToVector3() * 0.4f);
    }
}