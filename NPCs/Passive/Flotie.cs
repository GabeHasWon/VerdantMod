using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;

namespace Verdant.NPCs.Passive
{
    public class Flotie : ModNPC
    {
        static Asset<Texture2D> glowTexture;

        public override void SetStaticDefaults()
        {
            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 4;

            NPCID.Sets.CountsAsCritter[Type] = true;

            glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
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
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A curious glowing critter found in lush caves. It's usually found surrounded by its younglings."),
            });
        }

        public override void AI()
        {
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

            Lighting.AddLight(NPC.position, new Vector3(0.5f, 0.16f, 0.30f) * 1.4f);
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter == 0)
                NPC.frameCounter = (Main.rand.Next(2) * 2) + 1;

            int frame = (int)NPC.frameCounter - 1;

            if (NPC.velocity.Y < 0.01f)
                NPC.frame.Y = frame * 50;
            else
                NPC.frame.Y = (frame + 1) * 50;
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

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color color = GetAlpha(Color.White) ?? Color.White;

            if (NPC.IsABestiaryIconDummy)
                color = Color.White;

            Main.EntitySpriteDraw(glowTexture.Value, NPC.Center - screenPos + new Vector2(0, 3), NPC.frame, color, NPC.rotation, NPC.frame.Size() / 2f, 1f, SpriteEffects.None, 0);
        }
    }
}