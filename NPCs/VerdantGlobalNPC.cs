using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
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
            static string Dialogue(string key) => Language.GetTextValue("Mods.Verdant.NPCDialogue." + key);

            if (npc.type == NPCID.Guide) //no one loves him but me </3 - neutral on verdant
            {
                if (NPC.downedBoss2 && !ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown && Main.rand.NextBool(4))
                    chat = Dialogue("Guide.ApotheosisEvilDown");

                if (NPC.downedBoss3 && !ModContent.GetInstance<VerdantSystem>().apotheosisSkelDown && Main.rand.NextBool(4))
                    chat = Dialogue("Guide.ApotheosisSkeletronDown");
            }
            else if (npc.type == NPCID.Dryad) //loves the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(5))
                    chat = Dialogue("Dryad.0");

                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(5))
                    chat = Dialogue("Dryad.1");

                if (NPC.downedBoss1 && Main.rand.NextBool(5))
                    chat = Dialogue("Dryad.GreenCrystal");
            }
            else if (npc.type == NPCID.Stylist) //hates the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = Dialogue("Stylist");
            }
            else if (npc.type == NPCID.WitchDoctor) //likes the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                {
                    int rand = Main.rand.Next(6);
                    if (rand == 0)
                        chat = Dialogue("WitchDoctor.0");
                    else if (rand == 1)
                        chat = Dialogue("WitchDoctor.1");
                    else if (rand == 2)
                    {
                        int whoAmI = NPC.FindFirstNPC(NPCID.Dryad);

                        if (whoAmI != -1)
                            chat = Language.GetText("Mods.Verdant.NPCDialogue.WitchDoctor.Dryad").Format(Main.npc[whoAmI].GivenName);
                    }
                }
            }
            else if (npc.type == NPCID.DyeTrader) //loves the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                {
                    int rand = Main.rand.Next(6);

                    if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && rand == 0)
                        chat = Dialogue("DyeTrader.0");
                    else if (rand == 1)
                        chat = Dialogue("DyeTrader.1");
                }
            }
            else if (npc.type == NPCID.TaxCollector) //hates the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                {
                    if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(5))
                        chat = Dialogue("TaxCollector.0");
                    else if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(5))
                        chat = Dialogue("TaxCollector.1");
                }
            }
            else if (npc.type == NPCID.Painter) //likes the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = Dialogue("Painter");
            }
            else if (npc.type == NPCID.Princess)
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = Dialogue("Princess");
            }
        }
    }
}