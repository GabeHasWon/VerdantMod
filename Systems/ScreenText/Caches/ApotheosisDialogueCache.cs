using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using Verdant.Items.Verdant.Equipables;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Tools;
using Verdant.Systems.ScreenText.Animations;

namespace Verdant.Systems.ScreenText.Caches
{
    internal class ApotheosisDialogueCache : IDialogueCache
    {
        [Cache(nameof(ApotheosisDialogueCache) + ".Intro")]
        public static ScreenText IntroDialogue()
        {
            return new ScreenText("Hello, traveller.", 100) 
            { 
                shader = ModContent.Request<Effect>("Verdant/Effects/Text/TextWobble"), 
                color = Color.White * 0.6f, 
                shaderParams = new ScreenTextEffectParameters(0.02f, 0.01f, 30) 
            }.With(new ScreenText("It's been a long time since I've seen a new face.", 200, 0.8f), false).
                With(new ScreenText("Call me the Apotheosis.", 80, 1f), false).
                With(new ScreenText("Find us at the center of our verdant plants,", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime * 0.6f }, false).
                With(new ScreenText("and we might have some gifts to help you along.", 160, 0.8f)).
                FinishWith(new ScreenText("Farewell, for now.", 140, anim: new FadeAnimation(), dieAutomatically: false), (self) => { ModContent.GetInstance<VerdantSystem>().apotheosisGreeting = true; });
        }

        [Cache(nameof(ApotheosisDialogueCache) + ".Greeting")]
        public static ScreenText GreetingDialogue()
        {
            return new ScreenText("Remember to breathe,", 100, 0.9f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime
            }.With(new ScreenText("keep the plants thriving,", 60, 0.8f)).
                With(new ScreenText("and return to me once you've slain the great eye.", 140, 0.8f)).
                FinishWith(new ScreenText("May we find each other in good spirits soon.", 100, 0.9f));
        }

        [Cache(nameof(ApotheosisDialogueCache) + ".Idle")]
        public static ScreenText IdleDialogue()
        {
            List<string> randomLines = new List<string>()
            {
                "I'm particularly proud of those bouncy sprouts.",
                "We seek to harbor arbour; has it worked?",
                "Where's my quill - hm?",
                "\"I\", \"we\"; it's all the same.",
                "I've nothing more to add.",
                "Go out and smell the flowers.",
                "Run along, now."
            };
            ScreenText randomDialogue = new ScreenText(Main.rand.Next(randomLines), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime
            };

            List<string> randomThoughts = new List<string>()
            {
                "...where nature is most plain and pure...",
                "Hmm...what to do...",
                "...pest control...", //shoutout to To The Grave, good band
            };
            ScreenText randomThoughtDialogue = new ScreenText(Main.rand.Next(randomThoughts), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime * 0.45f,
                color = Color.Gray * 0.45f,
                shader = ModContent.Request<Effect>("Verdant/Effects/Text/TextWobble"),
                shaderParams = new ScreenTextEffectParameters(0.01f, 0.01f, 30)
            };

            ScreenText eocDialogue = new ScreenText("Finally, the eye is felled.", 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime,
            };

            List<string> evilBossLines = new List<string>()
            {
                "May grace befall you.",
                $"The {(WorldGen.crimson ? "brain" : "worm")} is no more.",
                "A presence lifted from the infestation..."
            };
            ScreenText evilDialogue = new ScreenText(Main.rand.Next(evilBossLines), 120, 0.8f)
            {
                speaker = "Apotheosis",
                speakerColor = Color.Lime,
            };

            List<string> skeleLines = new List<string>()
            {
                "That skeleton was...a confusing one.",
                "The poor man's freedom is obtained..."
            };

            ScreenText skeleDialogue = new ScreenText(Main.rand.Next(skeleLines), 120, 0.8f)
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
            return texts;
        }

        private static void AddAdditionalIdleDialogue(WeightedRandom<ScreenText> texts)
        {
            List<string> miscBossLines = new List<string>();

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
                ScreenText miscBossDialogue = new ScreenText(Main.rand.Next(miscBossLines), 120, 0.8f)
                {
                    speaker = "Apotheosis",
                    speakerColor = Color.Lime,
                };

                texts.Add(miscBossDialogue, 0.3f);
            }
        }

        [Cache(nameof(ApotheosisDialogueCache) + ".Eye")]
        public static ScreenText EoCDownDialogue()
        {
            return new ScreenText("The eye is felled. Thank you.", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                FinishWith(new ScreenText($"Take this trinket. Return to me once you've beaten the {(WorldGen.crimson ? "brain" : "eater")}.", 180, 0.7f), (self) =>
                {
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<PermVineWand>(), 1);
                    ModContent.GetInstance<VerdantSystem>().apotheosisEyeDown = true;
                });
        }

        [Cache(nameof(ApotheosisDialogueCache) + ".Evil")]
        public static ScreenText EvilDownDialogue()
        {
            return new ScreenText($"Our gratitude for defeating the {(WorldGen.crimson ? "Brain" : "Eater")}...", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                With(new ScreenText("Our penultimate request; fell the great skeleton near the dungeon. Anyhow -", 160, 0.7f)).
                FinishWith(new ScreenText("- here's some of my old gear...with some changes.", 100, 0.8f), (self) =>
                {
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<SproutInABoot>(), 1);
                    ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown = true;
                });
        }

        [Cache(nameof(ApotheosisDialogueCache) + ".Skeletron")]
        public static ScreenText SkeletronDownDialogue()
        {
            return new ScreenText("The dungeon's souls are...partially freed.", 120, 0.8f) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                FinishWith(new ScreenText("You're deserving - take these. Our favourites.", 60, 0.9f), (self) =>
                {
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<YellowBulb>(), 10);
                    ModContent.GetInstance<VerdantSystem>().apotheosisSkelDown = true;
                });
        }

        [Cache(nameof(ApotheosisDialogueCache) + ".WoF")]
        public static ScreenText WoFDownDialogue()
        {
            return new ScreenText("A powerful spirit has been released...", 120) { speaker = "Apotheosis", speakerColor = Color.Lime }.
                FinishWith(new ScreenText("Take this. I don't need it anymore...", 60), (self) =>
                {
                    Helper.SyncItem(Main.LocalPlayer.GetSource_GiftOrReward("Apotheosis"), Main.LocalPlayer.Center, ModContent.ItemType<Items.Verdant.Misc.HeartOfGrowth>(), 1);
                    ModContent.GetInstance<VerdantSystem>().apotheosisWallDown = true;
                });
        }
    }
}
