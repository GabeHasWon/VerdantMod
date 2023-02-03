using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Verdant.Effects;
using Verdant.Items.Verdant.Equipables;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Tools;
using Verdant.Systems.ScreenText.Animations;

namespace Verdant.Systems.ScreenText.Caches
{
    internal class ApotheosisDialogueCache : IDialogueCache
    {
        private static bool UseCustomSystem => ModContent.GetInstance<VerdantClientConfig>().CustomDialogue;

        [DialogueCacheKey(nameof(ApotheosisDialogueCache) + ".Intro")]
        public static ScreenText IntroDialogue(bool forServer)
        {
            ModContent.GetInstance<VerdantSystem>().apotheosisIntro = true;

            if (forServer)
                return null;

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.WorldData);

            static string Key(int index) => "$Mods.Verdant.ScreenText.Apotheosis.Intro." + index;

            if (!UseCustomSystem)
            {
                for (int i = 0; i < 6; ++i)
                    Chat(Key(i), i > 2);

                return null;
            }

            return new ScreenText(Key(0), 100) 
            { 
                shader = ModContent.Request<Effect>(EffectIDs.TextWobble), 
                color = Color.White * 0.6f, 
                shaderParams = new ScreenTextEffectParameters(0.02f, 0.01f, 30) 
            }.With(new ScreenText(Key(1), 200, 0.8f), false).
                With(new ScreenText(Key(2), 80, 1f), false).
                With(new ScreenText(Key(3), 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime * 0.6f }, false).
                With(new ScreenText(Key(4), 160, 0.8f)).
                FinishWith(new ScreenText(Key(5), 140, anim: new FadeAnimation(), dieAutomatically: false));
        }

        [DialogueCacheKey(nameof(ApotheosisDialogueCache) + ".Greeting")]
        public static ScreenText GreetingDialogue(bool forServer)
        {
            ModContent.GetInstance<VerdantSystem>().apotheosisGreeting = true;

            if (forServer)
                return null;

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.WorldData);

            static string Key(int index) => "$Mods.Verdant.ScreenText.Apotheosis.Greeting." + index;

            if (!UseCustomSystem)
            {
                for (int i = 0; i < 4; ++i)
                    Chat(Key(i));

                return null;
            }

            return new ScreenText(Key(0), 100, 0.9f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime
            }.With(new ScreenText(Key(1), 60, 0.8f)).
                With(new ScreenText(Key(2), 140, 0.8f)).
                FinishWith(new ScreenText(Key(3), 100, 0.9f));
        }

        [DialogueCacheKey(nameof(ApotheosisDialogueCache) + ".Idle")]
        public static ScreenText IdleDialogue(bool forServer)
        {
            if (forServer) //Can't actually happen atm, but good to double check
                return null;

            List<string> evilBossLines = new()
            {
                "$Mods.Verdant.ScreenText.Apotheosis.Idle.EvilBoss.0",
                Language.GetTextValue("Mods.Verdant.ScreenText.Apotheosis.Idle.EvilBoss.1", WorldGen.crimson ? "brain" : "worm"),
                "$Mods.Verdant.ScreenText.Apotheosis.Idle.EvilBoss.2",
            };

            ScreenText randomDialogue = new("$Mods.Verdant.ScreenText.Apotheosis.Idle.Normal." + Main.rand.Next(7), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime
            };

            ScreenText randomThoughtDialogue = new("$Mods.Verdant.ScreenText.Apotheosis.Idle.Thoughts." + Main.rand.Next(4), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime * 0.45f,
                color = Color.Gray * 0.45f,
                shader = ModContent.Request<Effect>(EffectIDs.TextWobble),
                shaderParams = new ScreenTextEffectParameters(0.01f, 0.01f, 30)
            };

            ScreenText eocDialogue = new("$Mods.Verdant.ScreenText.Apotheosis.Idle.EoC", 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime,
            };

            ScreenText evilDialogue = new(Main.rand.Next(evilBossLines), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime,
            };

            ScreenText skeleDialogue = new("$Mods.Verdant.ScreenText.Apotheosis.Idle.Skeletron." + Main.rand.Next(2), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime,
            };

            WeightedRandom<ScreenText> texts = new();
            texts.Add(randomDialogue, 1f);
            texts.Add(randomThoughtDialogue, 0.7f);

            if (NPC.downedBoss1)
                texts.Add(eocDialogue, 0.4f);

            if (NPC.downedBoss2)
                texts.Add(evilDialogue, 0.4f);

            if (NPC.downedBoss3)
                texts.Add(skeleDialogue, 0.4f);

            AddAdditionalIdleDialogue(texts);

            ScreenText result = texts;

            if (!UseCustomSystem)
            {
                Chat(result.text);
                return null;
            }
            return texts;
        }

        private static void AddAdditionalIdleDialogue(WeightedRandom<ScreenText> texts)
        {
            const string Key = "$Mods.Verdant.ScreenText.Apotheosis.Idle.MiscBosses.";

            List<string> miscBossLines = new();

            if (NPC.downedSlimeKing)
                miscBossLines.Add(Key + "KingSlime");

            if (NPC.downedQueenBee)
                miscBossLines.Add(Key + "QueenBee");

            if (ModLoader.TryGetMod("SpiritMod", out Mod spiritMod)) //shoutout to spirit mod developer GabeHasWon!! he helped a lot with this project
            {
                if ((bool)spiritMod.Call("downed", "Scarabeus"))
                    miscBossLines.Add(Key + "Scarabeus");

                if ((bool)spiritMod.Call("downed", "Moon Jelly Wizard"))
                    miscBossLines.Add(Key + "MJW");

                if ((bool)spiritMod.Call("downed", "Vinewrath Bane"))
                    miscBossLines.Add(Key + "VinewrathBane");

                if ((bool)spiritMod.Call("downed", "Ancient Avian"))
                    miscBossLines.Add(Key + "AncientAvian");

                if ((bool)spiritMod.Call("downed", "Starplate Raider"))
                    miscBossLines.Add(Key + "StarplateRaider");
            }

            if (miscBossLines.Count > 0)
            {
                ScreenText miscBossDialogue = new(Main.rand.Next(miscBossLines), 120, 0.8f)
                {
                    speaker = "Apotheosis",
                    speakerColor = Color.Lime,
                };

                texts.Add(miscBossDialogue, 0.3f + (miscBossLines.Count * 0.05f));
            }
        }

        [DialogueCacheKey(nameof(ApotheosisDialogueCache) + ".Eye")]
        public static ScreenText EoCDownDialogue(bool forServer)
        {
            ModContent.GetInstance<VerdantSystem>().apotheosisEyeDown = true;

            if (forServer)
                return null;

            if (!UseCustomSystem)
            {
                Chat("$Mods.Verdant.ScreenText.Apotheosis.Downed.EoC.0");
                Chat(Language.GetTextValue("Mods.Verdant.ScreenText.Apotheosis.Downed.EoC.1", WorldGen.crimson ? "brain" : "worm"));

                Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<PermVineWand>(), 1);
                return null;
            }

            return new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.Downed.EoC.0", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                FinishWith(new ScreenText(Language.GetTextValue("Mods.Verdant.ScreenText.Apotheosis.Downed.EoC.1", WorldGen.crimson ? "brain" : "worm"), 180, 0.7f), (self) =>
                {
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<PermVineWand>(), 1);
                });
        }

        [DialogueCacheKey(nameof(ApotheosisDialogueCache) + ".Evil")]
        public static ScreenText EvilDownDialogue(bool forServer)
        {
            ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown = true;

            if (forServer)
                return null;

            if (!UseCustomSystem)
            {
                Chat(Language.GetTextValue("Mods.Verdant.ScreenText.Apotheosis.Downed.Evil.0", WorldGen.crimson ? "Brain" : "Eater"));
                Chat("$Mods.Verdant.ScreenText.Apotheosis.Downed.Evil.1");
                Chat("$Mods.Verdant.ScreenText.Apotheosis.Downed.Evil.2");

                Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<SproutInABoot>(), 1);
                return null;
            }

            return new ScreenText(Language.GetTextValue("Mods.Verdant.ScreenText.Apotheosis.Downed.Evil.0", WorldGen.crimson ? "Brain" : "Eater"), 120, 0.8f) 
            { 
                speaker = "Apotheosis", speakerColor = Color.Lime 
            }.
                With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.Downed.Evil.1", 160, 0.7f)).
                FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.Downed.Evil.2", 100, 0.8f), (self) =>
                {
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<SproutInABoot>(), 1);
                });
        }

        [DialogueCacheKey(nameof(ApotheosisDialogueCache) + ".Skeletron")]
        public static ScreenText SkeletronDownDialogue(bool forServer)
        {
            ModContent.GetInstance<VerdantSystem>().apotheosisSkelDown = true;

            if (forServer)
                return null;

            if (!UseCustomSystem)
            {
                Chat("$Mods.Verdant.ScreenText.Apotheosis.Downed.Skeletron.0");
                Chat("$Mods.Verdant.ScreenText.Apotheosis.Downed.Skeletron.1");

                Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<YellowBulb>(), 10);
                return null;
            }

            return new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.Downed.Skeletron.0", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.Downed.Skeletron.1", 100, 0.9f), (self) =>
                {
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<YellowBulb>(), 10);
                });
        }

        [DialogueCacheKey(nameof(ApotheosisDialogueCache) + ".WoF")]
        public static ScreenText WoFDownDialogue(bool forServer)
        {
            ModContent.GetInstance<VerdantSystem>().apotheosisWallDown = true;

            if (forServer)
                return null;

            if (!UseCustomSystem)
            {
                for (int i = 0; i < 4; ++i)
                    Chat("$Mods.Verdant.ScreenText.Apotheosis.Downed.WoF." + i);

                Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<Items.Verdant.Misc.HeartOfGrowth>(), 1);
                return null;
            }

            return new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.Downed.WoF.0", 120) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.Downed.WoF.1", 120, 0.8f)).
                With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.Downed.WoF.2", 120, 0.9f)).
                FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.Downed.WoF.3", 60, 0.9f), (self) =>
                {
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<Items.Verdant.Misc.HeartOfGrowth>(), 1);
                });
        }

        private static void Chat(string text, bool useName = true)
        {
            string useText = "[c/32cd32:The Apotheosis:] " + text;

            if (!useName)
                useText = text;

            Main.NewText(VerdantLocalization.ScreenTextLocalization(useText), Color.White);
        }
    }
}
