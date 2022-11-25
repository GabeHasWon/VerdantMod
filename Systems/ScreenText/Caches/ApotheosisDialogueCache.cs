using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
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

            if (!UseCustomSystem)
            {
                Chat("Hello, traveller.", false);
                Chat("It's been a long time since I've seen a new face.", false);
                Chat("Call me the Apotheosis.", false);
                Chat("Find us at the center of our verdant plants,");
                Chat("and we might have some gifts to help you along.");
                Chat("Farewell, for now.");

                return null;
            }

            return new ScreenText("Hello, traveller.", 100) 
            { 
                shader = ModContent.Request<Effect>(EffectIDs.TextWobble), 
                color = Color.White * 0.6f, 
                shaderParams = new ScreenTextEffectParameters(0.02f, 0.01f, 30) 
            }.With(new ScreenText("It's been a long time since I've seen a new face.", 200, 0.8f), false).
                With(new ScreenText("Call me the Apotheosis.", 80, 1f), false).
                With(new ScreenText("Find us at the center of our verdant plants,", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime * 0.6f }, false).
                With(new ScreenText("and we might have some gifts to help you along.", 160, 0.8f)).
                FinishWith(new ScreenText("Farewell, for now.", 140, anim: new FadeAnimation(), dieAutomatically: false));
        }

        [DialogueCacheKey(nameof(ApotheosisDialogueCache) + ".Greeting")]
        public static ScreenText GreetingDialogue(bool forServer)
        {
            ModContent.GetInstance<VerdantSystem>().apotheosisGreeting = true;

            if (forServer)
                return null;

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.WorldData);

            if (!UseCustomSystem)
            {
                Chat("Remember to breathe,");
                Chat("keep the plants thriving,");
                Chat("and return to me once you've slain the great eye.");
                Chat("May we find each other in good spirits soon.");

                return null;
            }

            return new ScreenText("Remember to breathe,", 100, 0.9f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime
            }.With(new ScreenText("keep the plants thriving,", 60, 0.8f)).
                With(new ScreenText("and return to me once you've slain the great eye.", 140, 0.8f)).
                FinishWith(new ScreenText("May we find each other in good spirits soon.", 100, 0.9f));
        }

        [DialogueCacheKey(nameof(ApotheosisDialogueCache) + ".Idle")]
        public static ScreenText IdleDialogue(bool forServer)
        {
            if (forServer) //Can't actually happen atm, but good to double check
                return null;

            List<string> randomLines = new()
            {
                "I'm particularly proud of those bouncy sprouts.",
                "We seek to harbor arbour; has it worked?",
                "Where's my quill - hm?",
                "\"I\", \"we\"; it's all the same.",
                "I've nothing more to add.",
                "Go out and smell the flowers.",
                "Run along, now."
            };

            List<string> evilBossLines = new()
            {
                "May grace befall you.",
                $"The {(WorldGen.crimson ? "brain" : "worm")} is no more.",
                "A presence lifted from the infestation..."
            };

            List<string> randomThoughts = new()
            {
                "...where nature is most plain and pure...",
                "Hmm...what to do...",
                "...pest control...", //shoutout to To The Grave, good band
            };

            List<string> skeleLines = new()
            {
                "That skeleton was...a confusing one.",
                "The poor man's freedom is obtained..."
            };

            ScreenText randomDialogue = new(Main.rand.Next(randomLines), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime
            };

            ScreenText randomThoughtDialogue = new(Main.rand.Next(randomThoughts), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime * 0.45f,
                color = Color.Gray * 0.45f,
                shader = ModContent.Request<Effect>(EffectIDs.TextWobble),
                shaderParams = new ScreenTextEffectParameters(0.01f, 0.01f, 30)
            };

            ScreenText eocDialogue = new("Finally, the eye is felled.", 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime,
            };

            ScreenText evilDialogue = new(Main.rand.Next(evilBossLines), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime,
            };

            ScreenText skeleDialogue = new(Main.rand.Next(skeleLines), 120, 0.8f)
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
            List<string> miscBossLines = new();

            if (NPC.downedSlimeKing)
                miscBossLines.Add("Ah, the King of Slimes has been slain, wonderful...");

            if (NPC.downedQueenBee)
                miscBossLines.Add("Hopefully you're having a nice time with our bees.");

            if (ModLoader.TryGetMod("SpiritMod", out Mod spiritMod)) //shoutout to spirit mod developer GabeHasWon!! he helped a lot with this project
            {
                if ((bool)spiritMod.Call("downed", "Scarabeus"))
                    miscBossLines.Add("The desert sands feel calmer now.");

                if ((bool)spiritMod.Call("downed", "Moon Jelly Wizard"))
                    miscBossLines.Add("Ah, I love the critters of the glowing sky.\nIt seems you've met some as well.");

                if ((bool)spiritMod.Call("downed", "Vinewrath Bane"))
                    miscBossLines.Add("The flowers feel more relaxed now. Thank you.");

                if ((bool)spiritMod.Call("downed", "Ancient Avian"))
                    miscBossLines.Add("The skies are more at peace now, well done.");

                if ((bool)spiritMod.Call("downed", "Starplate Raider"))
                    miscBossLines.Add("We were curious about that glowing mech, but alas...");
            }

            if (miscBossLines.Count > 0)
            {
                ScreenText miscBossDialogue = new(Main.rand.Next(miscBossLines), 120, 0.8f)
                {
                    speaker = "Apotheosis",
                    speakerColor = Color.Lime,
                };

                texts.Add(miscBossDialogue, 0.3f);
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
                Chat("The eye is felled. Thank you.");
                Chat($"Take this trinket. Return to me once you've beaten the {(WorldGen.crimson ? "brain" : "eater")}.");

                Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<PermVineWand>(), 1);
                return null;
            }

            return new ScreenText("The eye is felled. Thank you.", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                FinishWith(new ScreenText($"Take this trinket. Return to me once you've beaten the {(WorldGen.crimson ? "brain" : "eater")}.", 180, 0.7f), (self) =>
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
                Chat($"Our gratitude for defeating the {(WorldGen.crimson ? "Brain" : "Eater")}...");
                Chat("Our penultimate request; fell the great skeleton near the dungeon. Anyhow -");
                Chat("- here's some of my old gear...with some changes.");

                Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<SproutInABoot>(), 1);
                return null;
            }

            return new ScreenText($"Our gratitude for defeating the {(WorldGen.crimson ? "Brain" : "Eater")}...", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                With(new ScreenText("Our penultimate request; fell the great skeleton near the dungeon. Anyhow -", 160, 0.7f)).
                FinishWith(new ScreenText("- here's some of my old gear...with some changes.", 100, 0.8f), (self) =>
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
                Chat("The dungeon's souls are...partially freed.");
                Chat("You're deserving - take these. Our favourites.");

                Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<YellowBulb>(), 10);
                return null;
            }

            return new ScreenText("The dungeon's souls are...partially freed.", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                FinishWith(new ScreenText("You're deserving - take these. Our favourites.", 60, 0.9f), (self) =>
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
                Chat("A powerful spirit has been released...");
                Chat("Take this. I...don't need it anymore.");

                Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<Items.Verdant.Misc.HeartOfGrowth>(), 1);
                return null;
            }

            return new ScreenText("A powerful spirit has been released...", 120) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                FinishWith(new ScreenText("Take this. I...don't need it anymore.", 60), (self) =>
                {
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<Items.Verdant.Misc.HeartOfGrowth>(), 1);
                });
        }

        private static void Chat(string text, bool useName = true)
        {
            string useText = "[c/32cd32:The Apotheosis:] " + text;
            if (!useName)
                useText = text;

            Main.NewText(useText, Color.White);
        }
    }
}
