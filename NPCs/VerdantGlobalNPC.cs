using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Scenes;

namespace Verdant.NPCs
{
    class VerdantGlobalNPC : GlobalNPC
    {
        public override void SetStaticDefaults()
        {
            NPCHappiness.Get(NPCID.Dryad).SetBiomeAffection<VerdantBiome>(AffectionLevel.Love).SetBiomeAffection<VerdantUndergroundBiome>(AffectionLevel.Love);
            NPCHappiness.Get(NPCID.TaxCollector).SetBiomeAffection<VerdantBiome>(AffectionLevel.Dislike).SetBiomeAffection<VerdantUndergroundBiome>(AffectionLevel.Dislike);
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant)
            {
                pool[0] = 0;

                pool[NPCID.GreenDragonfly] = 0.05f;
                pool[NPCID.RedDragonfly] = 0.05f;
                pool[NPCID.BlueDragonfly] = 0.05f;
                pool[NPCID.YellowDragonfly] = 0.05f;

                if (!Main.hardMode)
                    pool[NPCID.Firefly] = 0.15f;
                else
                    pool[NPCID.LightningBug] = 0.2f;
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == NPCID.Guide) //no one loves him but me </3 - neutral on verdant
            {
                if (NPC.downedBoss2 && !ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown && Main.rand.NextBool(4))
                    chat = "I hear a faint voice coming from those lush caves deep underground. Maybe you should check it out?";
                if (NPC.downedBoss3 && !ModContent.GetInstance<VerdantSystem>().apotheosisSkelDown && Main.rand.NextBool(4))
                    chat = "I hear another, stronger, voice coming from the lush caves. Take a look there.";
            }
            else if (npc.type == NPCID.Dryad) //loves the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = "I love this overgrown land. The world is powerful here.";
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = "I cannot believe I've never seen this place before...";
            }
            else if (npc.type == NPCID.Stylist) //hates the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = "The air is WAY too humid here, my hair's frizzing up so much.";
            }
            else if (npc.type == NPCID.WitchDoctor) //likes the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                {
                    int rand = Main.rand.Next(6);
                    if (rand == 0)
                        chat = "The energy in in the flowers here...it flows powerfully.";
                    else if (rand == 1)
                        chat = "I sense an ancient power within the leaves.";
                    else if (rand == 2)
                    {
                        int whoAmI = NPC.FindFirstNPC(NPCID.Dryad);

                        if (whoAmI != -1)
                            chat = $"{Main.npc[whoAmI].GivenName}'s been giving me some of her stock...care to take a look? It's quite nice.";
                    }
                }
            }
            else if (npc.type == NPCID.DyeTrader) //loves the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                {
                    int rand = Main.rand.Next(6);
                    if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && rand == 0)
                        chat = "Now THIS is a sight! What beautiful colours these flowers have!";
                    else if (rand == 1)
                        chat = "The glow of these bulbs...spectacular...";
                }
            }
            else if (npc.type == NPCID.TaxCollector) //hates the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                {
                    if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(5))
                        chat = "Bah! These leaves keep on getting in the house. Have someone clean them up for me!";
                    else if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(5))
                        chat = "Those bright flowers are getting on my nerve! Keep your blasted light out of my sight!";
                }
            }
            else if (npc.type == NPCID.Painter) //likes the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = "These tones are gorgeous! What a wonderful landscape!";
            }
            else if (npc.type == NPCID.Princess)
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = "There's so many pretty colours here!";
            }
        }
    }
}