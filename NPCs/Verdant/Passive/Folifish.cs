using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;

namespace Verdant.NPCs.Verdant.Passive
{
    public class Folifish : ModNPC //yoo thanks to Nimta on discord for the name
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 1;
            Main.npcCatchable[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.width = 50;
            npc.height = 34;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.dontTakeDamage = false;
            npc.value = 0f;
            npc.aiStyle = 16;
            aiType = NPCID.Goldfish;
            npc.dontCountMe = true;

            npc.catchItem = (short)ModContent.ItemType<FolifishItem>();
        }

        public override bool PreAI()
        {
            if (npc.ai[1] == 0)
                npc.ai[1] = Main.rand.Next(3) + 1;

            if (npc.wet && Main.rand.Next(1000) <= 8)
                Dust.NewDustPerfect(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), 34, new Vector2(Main.rand.NextFloat(-0.08f, 0.08f), Main.rand.NextFloat(-0.2f, -0.02f)));

            if (npc.velocity.X > 0) npc.spriteDirection = 1;
            else npc.spriteDirection = -1;

            Lighting.AddLight(npc.Center - new Vector2(20 * npc.spriteDirection, 10), new Vector3(0.5f, 0.16f, 0.30f) * 2.4f);
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 6; ++i)
                    Gore.NewGore(npc.Center, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), mod.GetGoreSlot("Gores/Verdant/LushLeaf"));
                for (int i = 0; i < 3; ++i)
                    Gore.NewGore(npc.Center, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), mod.GetGoreSlot("Gores/Verdant/RedPetalFalling"));
                for (int i = 0; i < 12; ++i)
                    Dust.NewDust(npc.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Color col = Lighting.GetColor((int)(npc.position.X / 16), (int)(npc.position.Y / 16), drawColor);
            Vector2 pos = npc.position - Main.screenPosition + (npc.Size / 2) + new Vector2(0, 6);
            SpriteEffects dir = SpriteEffects.None;
            if (npc.spriteDirection == 1)
                dir = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], pos, Main.npcTexture[npc.type].Frame(3, 1, (int)npc.ai[1] - 1, 0), col, 0f, new Vector2(24), 1f, dir, 1f);
            return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => ((spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant && spawnInfo.water) ? 1.5f : 0f) * (spawnInfo.playerInTown ? 1.75f : 1f);
    }
}