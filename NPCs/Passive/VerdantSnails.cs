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

            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Snail];
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Snail);
            NPC.width = 22;
            NPC.height = 18;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.dontCountMe = true;
            NPC.catchItem = (short)ModContent.ItemType<FlotinyItem>();

            AnimationType = NPCID.Snail;
            DrawOffsetY = 2;
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
                return 2f;
            return (spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant) ? 0.8f : 0f;
        }
    }

    public class VerdantBulbSnail : VerdantRedGrassSnail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightbulb Snail");

            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Snail];
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Snail);
            NPC.width = 20;
            NPC.height = 16;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.dontCountMe = true;
            NPC.catchItem = (short)ModContent.ItemType<FlotinyItem>();

            AnimationType = NPCID.Snail;
        }

        public override void AI() => Lighting.AddLight(NPC.Center, Color.HotPink.ToVector3() * 0.4f);
    }
}