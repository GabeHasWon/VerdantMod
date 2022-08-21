using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Verdant.Items.Verdant.Materials;
using Terraria.Localization;
using Verdant.Items.Verdant.Misc;
using Verdant.Foreground.Parallax;
using Verdant.Foreground;
using Verdant.World;
using Verdant.Items.Verdant.Tools;
using Terraria.DataStructures;
using System.Linq;

namespace Verdant.Tiles.Verdant.Decor
{
    internal class Apotheosis : ModTile
    {
        private int _timer = 0;

        public override void SetStaticDefaults() => QuickTile.SetMulti(this, 16, 12, DustID.Stone, SoundID.Dig, false, new Color(142, 120, 124), false, false, false, "Apotheosis");

        public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

        public override bool CanExplode(int i, int j) => false;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

        public override void NearbyEffects(int i, int j, bool closer)
        {
            _timer++;

            if (Framing.GetTileSafely(i, j).TileFrameX == 126 && Framing.GetTileSafely(i, j).TileFrameY == 36)
            {
                Vector2 p = (new Vector2(i, j) * 16);
                float LightMult = (float)((Math.Sin(Main.time * 0.03f) * 0.6) + 0.7);
                if (ModContent.GetInstance<VerdantWorld>().apotheosisEvilDown) LightMult *= 1.3f;
                if (ModContent.GetInstance<VerdantWorld>().apotheosisSkelDown) LightMult *= 1.6f;
                Lighting.AddLight(p, new Vector3(0.44f, 0.17f, 0.28f) * 2f * LightMult);
                Lighting.AddLight(p, new Vector3(0.1f, 0.03f, 0.06f));
            }

            if (Main.rand.NextBool(ApotheosisParticle.SpawnChance) && !Main.gamePaused && Main.hasFocus)
            {
                Vector2 pos = (new Vector2(i, j) * 16) - new Vector2(Main.rand.Next(-Main.screenWidth, Main.screenWidth), 
                    Main.rand.Next(-(int)(Main.screenHeight * 2f), (int)(Main.screenHeight * 2f)));
                ForegroundManager.AddItem(new ApotheosisParticle(pos));
            }
        }

        private readonly string[] assurance = new[] { "Remember to breathe...", "Keep the plants alive...", "Return to me once you've slain the EVILBOSS...", "...", "There is nothing more to say.", "We have no more to add." };
        private readonly string[] boss3Assurance = new[] { "We no longer hear theEVENT, peace at last...", "The mind is calm now...", "Silence, finally..." };
        private readonly string[] skeleAssurance = new[] { "The poor man's freedom is obtained, thank you.", "We can still hear some screams..." };

        public override bool RightClick(int i, int j)
        {
            Point adjPos = (Main.MouseWorld / 16f).ToPoint();
            Point adjOff = new Point(Framing.GetTileSafely(adjPos.X, adjPos.Y).TileFrameX / 18, Framing.GetTileSafely(adjPos.X, adjPos.Y).TileFrameY / 18);
            Vector2 realPos = new Vector2((adjPos.X - adjOff.X) * 16, (adjPos.Y - adjOff.Y) * 16);

            int playerCount = Main.player.Where(x => x.active).Count();

            if (_timer > 3000)
            {
                if (NPC.downedBoss2 && !ModContent.GetInstance<VerdantWorld>().apotheosisEvilDown) //BoC/EoW text
                {
                    string msg = "My gratitude for defeating the " + (WorldGen.crimson ? "mind" : "devourer") + ", here...";
                    Speak(msg);

                    Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Rectangle((int)realPos.X, (int)realPos.Y, 288, 216), ModContent.ItemType<YellowBulb>(), 8 * playerCount); //temp ID
                    ModContent.GetInstance<VerdantWorld>().apotheosisEvilDown = true;
                    return true;
                }

                if (NPC.downedBoss3 && !ModContent.GetInstance<VerdantWorld>().apotheosisSkelDown) //Skeleton boss text
                {
                    Speak("Our blessings for slaying the skeleton, here...");
                    ModContent.GetInstance<VerdantWorld>().apotheosisSkelDown = true;

                    for (int k = 0; k < playerCount; ++k)
                        Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Rectangle((int)realPos.X, (int)realPos.Y, 288, 216), ModContent.ItemType<VineWand>(), 1);
                    return true;
                }

                if (Main.hardMode && !ModContent.GetInstance<VerdantWorld>().apotheosisWallDown) //WoF boss text
                {
                    Speak("We sense a powerful spirit released...take this.");

                    for (int v = 0; v < playerCount; ++v)
                        Item.NewItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Rectangle((int)realPos.X, (int)realPos.Y, 288, 216), ModContent.ItemType<HeartOfGrowth>(), 1);
                    ModContent.GetInstance<VerdantWorld>().apotheosisWallDown = true;
                    return true;
                }

                if (ModContent.GetInstance<VerdantWorld>().apotheosisDialogueIndex < 3) //Boss text
                {
                    string msg = assurance[ModContent.GetInstance<VerdantWorld>().apotheosisDialogueIndex++];
                    if (msg.Contains("EVILBOSS")) msg = msg.Replace("EVILBOSS", WorldGen.crimson ? "mind" : "devourer");

                    Speak(msg);
                }
                else
                {
                    string msg = assurance[Main.rand.Next(3, 6)];
                    int r = Main.rand.Next(9);
                    if (ModContent.GetInstance<VerdantWorld>().apotheosisEvilDown && r == 1) msg = Main.rand.Next(boss3Assurance).Replace("EVENT", WorldGen.crimson ? " digging" : "ir thoughts");
                    if (ModContent.GetInstance<VerdantWorld>().apotheosisSkelDown && r == 2) msg = Main.rand.Next(skeleAssurance);

                    if (r == 3)
                    {
                        if (NPC.downedBoss1)
                            msg = "We feel an evil presense finally resting...";
                        if (NPC.downedSlimeKing)
                            msg = "Ah, the King of Slimes has been slain, wonderful...";
                    }

                    Mod spiritMod = ModLoader.GetMod("SpiritMod");
                    if (r == 4 && spiritMod != null) //shoutout to spirit mod developer GabeHasWon!! he helped a lot with this project
                    {
                        if ((bool)spiritMod.Call("downed", "Scarabeus"))
                            msg = "The desert sands feel calmer now.";
                        if ((bool)spiritMod.Call("downed", "Moon Jelly Wizard"))
                            msg = "Ah, I love the critters of the glowing sky.1It seems you've met some as well.";
                        if ((bool)spiritMod.Call("downed", "Vinewrath Bane"))
                            msg = "The flowers feel more relaxed now. Thank you.";
                        if ((bool)spiritMod.Call("downed", "Ancient Avian"))
                            msg = "The skies are more at peace now, well done.";
                        if ((bool)spiritMod.Call("downed", "Starplate Raider"))
                            msg = "We always had a soft spot for that glowing mechanism, but alas...";
                    }

                    string[] split = msg.Split('1');
                    foreach (var item in split)
                        Speak(msg);
                }
            }
            return true;
        }

        private void Speak(string msg)
        {
            var speechType = ModContent.GetInstance<VerdantServerConfig>().ApothTextSetting;
            if (speechType == VerdantServerConfig.ApotheosisInteraction.World || speechType == VerdantServerConfig.ApotheosisInteraction.Both)
            {
                int c = CombatText.NewText(new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 60, 20), new Color(88, 188, 24), msg, true);
                Main.combatText[c].lifeTime *= 2;
            }
            if (speechType == VerdantServerConfig.ApotheosisInteraction.Chat || speechType == VerdantServerConfig.ApotheosisInteraction.Both)
            {
                string chat = $"The Apotheosis: [c/509128:\"{msg}\"]";
                if (Main.netMode == NetmodeID.Server) //MP compat :)
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(chat), Color.White);
                else if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(chat, Color.White);
            }
            _timer = 0;
        }

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.cursorItemIconText = "Speak";
            Main.LocalPlayer.cursorItemIconEnabled = false;
            Main.LocalPlayer.cursorItemIconID = -1;
        }
    }
}