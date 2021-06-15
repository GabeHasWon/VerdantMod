using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Verdant.Items.Verdant.Materials;
using Terraria.Localization;

namespace Verdant.Tiles.Verdant.Decor
{
    internal class Apotheosis : ModTile
    {
        public override void SetDefaults() => QuickTile.SetMulti(this, 16, 12, DustID.Stone, SoundID.Dig, false, new Color(142, 120, 124), false, false, false, "Apotheosis");

        public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;

        public override bool CanExplode(int i, int j) => false;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Framing.GetTileSafely(i, j).frameX == 126 && Framing.GetTileSafely(i, j).frameY == 36)
            {
                Vector2 p = (new Vector2(i, j) * 16);
                float LightMult = (float)((Math.Sin(Main.time * 0.03f) * 0.6) + 0.7);
                if (World.VerdantWorld.apotheosisEvilDown) LightMult *= 1.3f;
                if (World.VerdantWorld.apotheosisSkelDown) LightMult *= 1.6f;
                Lighting.AddLight(p, new Vector3(0.44f, 0.17f, 0.28f) * 2f * LightMult);
                Lighting.AddLight(p, new Vector3(0.1f, 0.03f, 0.06f));
            }
        }

        private readonly string[] assurance = new[] { "Remember to breathe...", "Keep the plants alive...", "Return to me once you've slain the EVILBOSS...", "...", "There is nothing more to say.", "We have no more to add." };
        private readonly string[] boss3Assurance = new[] { "We no longer hear theEVENT, peace at last...", "The mind is calm now...", "Silence, finally..." };
        private readonly string[] skeleAssurance = new[] { "The poor man's freedom is obtained, thank you.", "We can still hear some screams..." };

        public override bool NewRightClick(int i, int j)
        {
            Point adjPos = (Main.MouseWorld / 16f).ToPoint();
            Point adjOff = new Point(Framing.GetTileSafely(adjPos.X, adjPos.Y).frameX / 18, Framing.GetTileSafely(adjPos.X, adjPos.Y).frameY / 18);
            Vector2 realPos = new Vector2((adjPos.X - adjOff.X) * 16, (adjPos.Y - adjOff.Y) * 16);

            if (NPC.downedBoss2 && !World.VerdantWorld.apotheosisEvilDown) //BoC/EoW text
            {
                string msg = "Our gratitude for defeating the " + (WorldGen.crimson ? "mind" : "devourer") + ", here...";
                Speak(msg);

                Item.NewItem(new Rectangle((int)realPos.X, (int)realPos.Y, 288, 216), ModContent.ItemType<RedPetal>(), 8); //temp ID
                World.VerdantWorld.apotheosisEvilDown = true;
                return true;
            }

            if (NPC.downedBoss3 && !World.VerdantWorld.apotheosisSkelDown) //Skeleton boss text
            {
                Speak("Our blessings for slaying the skeleton, take this...");

                Item.NewItem(new Rectangle((int)realPos.X, (int)realPos.Y, 288, 216), ModContent.ItemType<YellowBulb>(), 8);
                World.VerdantWorld.apotheosisSkelDown = true;
                return true;
            }

            if (World.VerdantWorld.apotheosisDialogueIndex < 3) //Boss text
            {
                string msg = assurance[World.VerdantWorld.apotheosisDialogueIndex++];
                if (msg.Contains("EVILBOSS")) msg = msg.Replace("EVILBOSS", WorldGen.crimson ? "mind" : "devourer");
                Speak(msg);
            }
            else
            {
                string msg = assurance[Main.rand.Next(3, 6)];
                int r = Main.rand.Next(5);
                if (World.VerdantWorld.apotheosisEvilDown && r == 1) msg = Main.rand.Next(boss3Assurance).Replace("EVENT", WorldGen.crimson ? " digging" : "ir thoughts");
                if (World.VerdantWorld.apotheosisSkelDown && r == 2) msg = Main.rand.Next(skeleAssurance);

                Mod spiritMod = ModLoader.GetMod("SpiritMod");
                if (r == 3 && spiritMod != null) //shoutout to spirit mod developer GabeHasWon!! he helped a lot with this project
                {
                    if ((bool)spiritMod.Call("downed", "Scarabeus"))
                        msg = "The desert sands feel calmer now";
                    if ((bool)spiritMod.Call("downed", "Moon Jelly Wizard"))
                        msg = "Ah, I love the critters of the glowing sky. It seems you've met some as well.";
                    if ((bool)spiritMod.Call("downed", "Vinewrath Bane"))
                        msg = "The flowers feel more relaxed now. Thank you.";
                    if ((bool)spiritMod.Call("downed", "Ancient Avian"))
                        msg = "The skies are more now, spectactular.";
                    if ((bool)spiritMod.Call("downed", "Starplate Raider"))
                        msg = "We always had a soft spot for that glowing worm, but alas...";
                }

                if (r == 4)
                {
                    if (NPC.downedBoss1)
                        msg = "We feel an evil presense finally resting...";
                    if (NPC.downedSlimeKing)
                        msg = "Ah, the King of Slimes has been slain, wonderful...";
                }
                Speak(msg);
            }
            return true;
        }

        private void Speak(string msg)
        {
            int speechType = ModContent.GetInstance<VerdantServerConfig>().ApothTextSetting;
            if (speechType == 0 || speechType == 2)
            {
                int c = CombatText.NewText(new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 60, 20), new Color(88, 188, 24), msg, true);
                Main.combatText[c].lifeTime *= 2;
            }
            if (speechType == 1 || speechType == 2)
            {
                if (Main.netMode == NetmodeID.Server) //MP compat :)
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("\"" + msg + "\""), new Color(88, 188, 24));
                else if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText("\"" + msg + "\"", new Color(88, 188, 24));
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player p = Main.player[Main.myPlayer];
            p.showItemIconText = "Speak";
            p.showItemIcon = false;
        }
    }
}