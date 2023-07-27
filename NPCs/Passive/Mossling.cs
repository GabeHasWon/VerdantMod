using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;

namespace Verdant.NPCs.Passive
{
    public class Mossling : ModNPC
    {
        ref float BaseState => ref NPC.ai[0];
        ref float ScaleY => ref NPC.ai[1];
        ref float Timer => ref NPC.ai[2];
        ref float ScaleSpeed => ref NPC.ai[3];

        public override bool IsLoadingEnabled(Mod mod) => VerdantMod.DebugModActive;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.CountsAsCritter[Type] = true;

            // DisplayName.SetDefault("Mossling [Deprecated]");
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.dontCountMe = true;

            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 1;
            NPC.catchItem = (short)ModContent.ItemType<FlotieItem>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("An almost microscopic speck, growing in and around lush, humid areas. Moves to a beat you cannot hear. Sadly, they can't be found naturally any longer."),
            });
        }

        public const int MaxDistance = 40;
        public override void AI()
        {
            if (BaseState == 0)
            {
                ScaleSpeed = Main.rand.NextFloat(0.8f, 1.2f);

                NPC.frame.X = 20 * Main.rand.Next(4);

                //lets hit that fat scan
                bool[] validGrounds = new bool[4] { false, false, false, false }; //Implemented from my code for ZeroG's sentry
                for (int i = (int)(NPC.position.Y / 16f); i < (int)(NPC.position.Y / 16f) + MaxDistance; ++i) //above
                    if (Framing.GetTileSafely((int)(NPC.position.X / 16f), i).HasTile && Main.tileSolid[Framing.GetTileSafely((int)(NPC.position.X / 16f), i).TileType])
                        validGrounds[0] = true;
                for (int i = (int)(NPC.position.Y / 16f); i > (int)(NPC.position.Y / 16f) - MaxDistance; --i) //below
                    if (Framing.GetTileSafely((int)(NPC.position.X / 16f), i).HasTile && Main.tileSolid[Framing.GetTileSafely((int)(NPC.position.X / 16f), i).TileType])
                        validGrounds[1] = true;

                for (int i = (int)(NPC.position.X / 16f); i > (int)(NPC.position.X / 16f) - MaxDistance; --i) //left
                    if (Framing.GetTileSafely(i, (int)(NPC.position.Y / 16f)).HasTile && Main.tileSolid[Framing.GetTileSafely(i, (int)(NPC.position.Y / 16f)).TileType])
                        validGrounds[2] = true;
                for (int i = (int)(NPC.position.X / 16f); i < (int)(NPC.position.X / 16f) + MaxDistance; ++i) //right
                    if (Framing.GetTileSafely(i, (int)(NPC.position.Y / 16f)).HasTile && Main.tileSolid[Framing.GetTileSafely(i, (int)(NPC.position.Y / 16f)).TileType])
                        validGrounds[3] = true;

                int index;
                int repeats = 0;
                while (true)
                {
                    index = Main.rand.Next(4);
                    repeats++;
                    if (validGrounds[index] || repeats > 60)
                        break;
                }

                BaseState = index + 1;
                NPC.spriteDirection = Main.rand.NextBool(2) ? -1 : 1;

                switch (BaseState)
                {
                    case 1:
                        for (int i = (int)(NPC.position.Y / 16f); i < (int)(NPC.position.Y / 16f) + MaxDistance; ++i) //below
                        {
                            if (Framing.GetTileSafely((int)(NPC.position.X / 16f), i).HasTile && Main.tileSolid[Framing.GetTileSafely((int)(NPC.position.X / 16f), i).TileType])
                            {
                                NPC.position.Y = (i * 16f) - 12;
                                break;
                            }
                        }
                        break;
                    case 2:
                        for (int i = (int)(NPC.position.Y / 16f); i > (int)(NPC.position.Y / 16f) - MaxDistance; --i) //above
                        {
                            if (Framing.GetTileSafely((int)(NPC.position.X / 16f), i).HasTile && Main.tileSolid[Framing.GetTileSafely((int)(NPC.position.X / 16f), i).TileType])
                            {
                                NPC.position.Y = (i + 1) * 16f;
                                break;
                            }
                        }
                        NPC.rotation = MathHelper.ToRadians(180);
                        break;
                    case 3:
                        for (int i = (int)(NPC.position.X / 16f); i > (int)(NPC.position.X / 16f) - MaxDistance; --i) //left
                        {
                            if (Framing.GetTileSafely(i, (int)(NPC.position.Y / 16f)).HasTile && Main.tileSolid[Framing.GetTileSafely(i, (int)(NPC.position.Y / 16f)).TileType])
                            {
                                NPC.position.X = (i * 16f) + 12;
                                break;
                            }
                        }
                        NPC.rotation = MathHelper.ToRadians(90);
                        break;
                    case 4:
                        for (int i = (int)(NPC.position.X / 16f); i < (int)(NPC.position.X / 16f) + MaxDistance; ++i) //right
                        {
                            if (Framing.GetTileSafely(i, (int)(NPC.position.Y / 16f)).HasTile && Main.tileSolid[Framing.GetTileSafely(i, (int)(NPC.position.Y / 16f)).TileType])
                            {
                                NPC.position.X = (i * 16f) - 10;
                                break;
                            }
                        }
                        NPC.rotation = MathHelper.ToRadians(270);
                        break;
                    default: break;
                }
            }
            else
                ScaleY = (float)(0.25f * Math.Sin(Timer++ * 0.03f * ScaleSpeed)) + 1;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.IsABestiaryIconDummy)
                BaseState = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D t = Mod.Assets.Request<Texture2D>("NPCs/Passive/Mossling").Value;
            Vector2 offset = new Vector2(0, 6);
            if      (BaseState == 2) offset = new Vector2(0, -12);
            else if (BaseState == 3) offset = new Vector2(-8, 0);
            else if (BaseState == 4) offset = new Vector2(6, 0);
            spriteBatch.Draw(t, NPC.Center - screenPos + offset, new Rectangle(NPC.frame.X, NPC.frame.Y, NPC.width, NPC.height), NPC.GetLightColor(), NPC.rotation, new Vector2(9, t.Height), new Vector2(1f, ScaleY), SpriteEffects.None, 0f);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
                for (int i = 0; i < 2; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => ((spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.raining) ? 2f : 0f) * (spawnInfo.PlayerInTown ? 1.75f : 0f);
    }
}