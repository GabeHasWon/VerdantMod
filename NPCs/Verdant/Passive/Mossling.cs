using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;

namespace Verdant.NPCs.Verdant.Passive
{
    public class Mossling : ModNPC
    {
        ref float BaseState => ref npc.ai[0];
        ref float ScaleY => ref npc.ai[1];
        ref float Timer => ref npc.ai[2];
        ref float ScaleSpeed => ref npc.ai[3];

        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 18;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.dontCountMe = true;

            Main.npcCatchable[npc.type] = true;
            Main.npcFrameCount[npc.type] = 1;
            npc.catchItem = (short)ModContent.ItemType<FlotieItem>();
        }

        public const int MaxDistance = 40;
        public override void AI()
        {
            if (BaseState == 0)
            {
                ScaleSpeed = Main.rand.NextFloat(0.8f, 1.2f);

                npc.frame.X = 20 * Main.rand.Next(4);

                //lets hit that fat scan
                bool[] validGrounds = new bool[4] { false, false, false, false }; //Implemented from my code for ZeroG's sentry
                for (int i = (int)(npc.position.Y / 16f); i < (int)(npc.position.Y / 16f) + MaxDistance; ++i) //above
                    if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
                        validGrounds[0] = true;
                for (int i = (int)(npc.position.Y / 16f); i > (int)(npc.position.Y / 16f) - MaxDistance; --i) //below
                    if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
                        validGrounds[1] = true;

                for (int i = (int)(npc.position.X / 16f); i > (int)(npc.position.X / 16f) - MaxDistance; --i) //left
                    if (Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).active() && Main.tileSolid[Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).type])
                        validGrounds[2] = true;
                for (int i = (int)(npc.position.X / 16f); i < (int)(npc.position.X / 16f) + MaxDistance; ++i) //right
                    if (Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).active() && Main.tileSolid[Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).type])
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
                npc.spriteDirection = Main.rand.NextBool(2) ? -1 : 1;

                switch (BaseState)
                {
                    case 1:
                        for (int i = (int)(npc.position.Y / 16f); i < (int)(npc.position.Y / 16f) + MaxDistance; ++i) //below
                        {
                            if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
                            {
                                npc.position.Y = (i * 16f) - 12;
                                break;
                            }
                        }
                        break;
                    case 2:
                        for (int i = (int)(npc.position.Y / 16f); i > (int)(npc.position.Y / 16f) - MaxDistance; --i) //above
                        {
                            if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
                            {
                                npc.position.Y = (i + 1) * 16f;
                                break;
                            }
                        }
                        npc.rotation = MathHelper.ToRadians(180);
                        break;
                    case 3:
                        for (int i = (int)(npc.position.X / 16f); i > (int)(npc.position.X / 16f) - MaxDistance; --i) //left
                        {
                            if (Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).active() && Main.tileSolid[Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).type])
                            {
                                npc.position.X = (i * 16f) + 12;
                                break;
                            }
                        }
                        npc.rotation = MathHelper.ToRadians(90);
                        break;
                    case 4:
                        for (int i = (int)(npc.position.X / 16f); i < (int)(npc.position.X / 16f) + MaxDistance; ++i) //right
                        {
                            if (Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).active() && Main.tileSolid[Framing.GetTileSafely(i, (int)(npc.position.Y / 16f)).type])
                            {
                                npc.position.X = (i * 16f) - 10;
                                break;
                            }
                        }
                        npc.rotation = MathHelper.ToRadians(270);
                        break;
                    default: break;
                }
            }
            else
            {
                ScaleY = (float)(0.25f * Math.Sin(Timer++ * 0.03f * ScaleSpeed)) + 1;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D t = VerdantMod.Instance.GetTexture("NPCs/Verdant/Passive/Mossling");
            Vector2 offset = new Vector2(0, 6);
            if      (BaseState == 2) offset = new Vector2(0, -12);
            else if (BaseState == 3) offset = new Vector2(-8, 0);
            else if (BaseState == 4) offset = new Vector2(6, 0);
            spriteBatch.Draw(t, npc.Center - Main.screenPosition + offset, new Rectangle(npc.frame.X, npc.frame.Y, npc.width, npc.height), npc.GetLightColor(), npc.rotation, new Vector2(9, t.Height), new Vector2(1f, ScaleY), SpriteEffects.None, 0f);
            return false;
        }

        public override bool CheckDead()
        {
            for (int i = 0; i < 2; ++i)
                Gore.NewGore(npc.Center, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), mod.GetGoreSlot("Gores/Verdant/LushLeaf"));
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => ((spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.raining) ? 2f : 0f) * (spawnInfo.playerInTown ? 1.75f : 0f);
    }
}