using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;

namespace Verdant.NPCs.Passive
{
    public class Folifish : ModNPC //yoo thanks to Nimta on discord for the name
    {
        public override bool IsLoadingEnabled(Mod mod) => VerdantMod.DebugModActive;

        public override void SetStaticDefaults()
        {
            Main.npcCatchable[NPC.type] = true;

            NPCID.Sets.CountsAsCritter[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 34;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.dontTakeDamage = false;
            NPC.value = 0f;
            NPC.aiStyle = 16;
            NPC.dontCountMe = true;
            NPC.catchItem = (short)ModContent.ItemType<FolifishItem>();
            
            AIType = NPCID.Goldfish;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A fish overgrown with plant life. While eating it isn't particularly tasty, it is quite healthy. However, it seems to have all but disappeared from the Verdant."),
            });
        }

        public override bool PreAI()
        {
            if (NPC.ai[1] == 0)
                NPC.ai[1] = Main.rand.Next(3) + 1;

            if (NPC.wet && Main.rand.Next(1000) <= 8)
                Dust.NewDustPerfect(NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)), 34, new Vector2(Main.rand.NextFloat(-0.08f, 0.08f), Main.rand.NextFloat(-0.2f, -0.02f)));

            if (NPC.velocity.X > 0) NPC.spriteDirection = 1;
            else NPC.spriteDirection = -1;

            Lighting.AddLight(NPC.Center - new Vector2(20 * NPC.spriteDirection, 10), new Vector3(0.5f, 0.16f, 0.30f) * 2.4f);
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 6; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
                for (int i = 0; i < 3; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("RedPetalFalling").Type);
                for (int i = 0; i < 12; ++i)
                    Dust.NewDust(NPC.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy && NPC.ai[1] == 0)
                NPC.ai[1] = Main.rand.Next(3) + 1;

            Color col = NPC.IsABestiaryIconDummy ? Color.White : NPC.GetNPCColorTintedByBuffs(Lighting.GetColor((int)(NPC.position.X / 16), (int)(NPC.position.Y / 16), drawColor));
            Vector2 pos = NPC.position - screenPos + (NPC.Size / 2) + new Vector2(0, 6);
            SpriteEffects dir = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                dir = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, pos, TextureAssets.Npc[NPC.type].Value.Frame(3, 1, (int)NPC.ai[1] - 1, 0), col, 0f, new Vector2(24), 1f, dir, 1f);
            return false;
        }
    }
}