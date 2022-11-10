using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Verdant.Items.Verdant.Materials;
using Terraria.Localization;
using Verdant.Items.Verdant.Misc;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Systems.Foreground;
using Verdant.Items.Verdant.Tools;
using Terraria.DataStructures;
using System.Linq;
using Verdant.Items.Verdant.Equipables;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Tiles.Verdant.Decor;

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
            if (ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown) LightMult *= 1.3f;
            if (ModContent.GetInstance<VerdantSystem>().apotheosisSkelDown) LightMult *= 1.6f;
            Lighting.AddLight(p, new Vector3(0.44f, 0.17f, 0.28f) * 2f * LightMult);
            Lighting.AddLight(p, new Vector3(0.1f, 0.03f, 0.06f));
        }

        if ((j > Main.worldSurface || !Main.dayTime) && Main.netMode != NetmodeID.Server && Main.rand.NextBool(ApotheosisParticle.SpawnChance) && !Main.gamePaused && Main.hasFocus)
        {
            Vector2 pos = (new Vector2(i, j) * 16) - new Vector2(Main.rand.Next(-Main.screenWidth, Main.screenWidth), 
                Main.rand.Next(-(int)(Main.screenHeight * 2f), (int)(Main.screenHeight * 2f)));
            ForegroundManager.AddItem(new ApotheosisParticle(pos));
        }
    }

    public override bool RightClick(int i, int j)
    {
        Vector2 realPos = TileHelper.GetTopLeft(new(i, j)).ToWorldCoordinates() + new Vector2(0, 60);

        int playerCount = Main.CurrentFrameFlags.ActivePlayersCount;

        if (_timer > 3000)
        {
            if (ScreenTextManager.CurrentText is not null)
                return false;

            if (NPC.downedBoss1 && !ModContent.GetInstance<VerdantSystem>().apotheosisEyeDown) //EoC text
            {
                ScreenTextManager.CurrentText = new ScreenText("The eye is felled. Thank you.", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                    FinishWith(new ScreenText($"Take this trinket. Return to me once you've beaten the {(WorldGen.crimson ? "brain" : "eater")}.", 180, 0.7f), (self) =>
                    {
                        for (int k = 0; k < playerCount; ++k)
                            Helper.SyncItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Rectangle((int)realPos.X, (int)realPos.Y, 288, 216), ModContent.ItemType<PermVineWand>(), 1);
                        ModContent.GetInstance<VerdantSystem>().apotheosisEyeDown = true;
                    });
                return true;
            }

            if (NPC.downedBoss2 && !ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown) //BoC/EoW text
            {
                ScreenTextManager.CurrentText = new ScreenText($"Our gratitude for defeating the {(WorldGen.crimson ? "Brain" : "Eater")}...", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                    With(new ScreenText($"If you're willing, we'd love to see the great skeleton removed. But...", 160, 0.7f)).
                    FinishWith(new ScreenText("Here's some of my old gear - with some added strength, of course.", 100, 0.8f), (self) =>
                    {
                        for (int k = 0; k < playerCount; ++k)
                            Helper.SyncItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Rectangle((int)realPos.X, (int)realPos.Y, 288, 216), ModContent.ItemType<SproutInABoot>(), 1);
                        ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown = true;
                    });
                return true;
            }

            if (NPC.downedBoss3 && !ModContent.GetInstance<VerdantSystem>().apotheosisSkelDown) //Skeleton boss text
            {
                ScreenTextManager.CurrentText = new ScreenText("The dungeon's souls are...partially freed.", 120) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                    FinishWith(new ScreenText("You're deserving - take these. Our favourites.", 60), (self) =>
                    {
                        Helper.SyncItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Rectangle((int)realPos.X, (int)realPos.Y, 288, 216), ModContent.ItemType<YellowBulb>(), 12 * playerCount);
                        ModContent.GetInstance<VerdantSystem>().apotheosisSkelDown = true;
                    });
                return true;
            }

            if (Main.hardMode && !ModContent.GetInstance<VerdantSystem>().apotheosisWallDown) //WoF boss text
            {
                ScreenTextManager.CurrentText = new ScreenText("A powerful spirit has been released...", 120) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                    FinishWith(new ScreenText("Take this. I don't need it anymore...", 60), (self) =>
                    {
                        Helper.SyncItem(new EntitySource_TileInteraction(Main.LocalPlayer, i, j), new Rectangle((int)realPos.X, (int)realPos.Y, 288, 216), ModContent.ItemType<HeartOfGrowth>(), 1);
                        ModContent.GetInstance<VerdantSystem>().apotheosisWallDown = true;
                    });
                return true;
            }

            if (!ModContent.GetInstance<VerdantSystem>().apotheosisGreeting) //Greeting
            {
                ScreenTextManager.CurrentText = ApotheosisDialogueCache.GreetingDialogue();
                ModContent.GetInstance<VerdantSystem>().apotheosisGreeting = true;
            }
            else
                ScreenTextManager.CurrentText = ApotheosisDialogueCache.IdleDialogue();
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