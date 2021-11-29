using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.NPCs
{
    class VerdantGlobalNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<VerdantPlayer>().ZoneVerdant)
            {
                pool[NPCID.MotherSlime] = 0f; //no
                pool[NPCID.CaveBat] = 0f; //also no
                int[] skeletons = new int[] { NPCID.Skeleton, NPCID.HeadacheSkeleton, NPCID.SkeletonAlien, NPCID.SkeletonAstonaut, NPCID.SkeletonTopHat, NPCID.MisassembledSkeleton, NPCID.PantlessSkeleton,
                    NPCID.BoneThrowingSkeleton, NPCID.BoneThrowingSkeleton2, NPCID.BoneThrowingSkeleton3, NPCID.BoneThrowingSkeleton4 };
                foreach (var i in skeletons) pool[i] = 0f; //stupid fr*cking skeletons
                pool[NPCID.BlueJellyfish] = 0f; //NO
                pool[NPCID.GreenJellyfish] = 0f; //I SAID NO
                for (int i = NPCID.Salamander; i <= NPCID.Salamander9; ++i) //NO SALAMANDERS EITHER
                    pool[i] = 0f;
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == NPCID.Guide) //no one loves him but me </3 - neutral on verdant
            {
                if (NPC.downedBoss2 && !World.VerdantWorld.apotheosisEvilDown && Main.rand.NextBool(4))
                    chat = "I hear a faint voice coming from those lush caves deep underground. Maybe you should check it out?";
                if (NPC.downedBoss3 && !World.VerdantWorld.apotheosisSkelDown && Main.rand.NextBool(4))
                    chat = "I hear another, stronger, voice coming from the lush caves. Take a look there.";
            }
            else if (npc.type == NPCID.Dryad) //loves the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = "I love this overgrown land. The world is powerful here.";
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
                        chat = "I sense an ancient power within the leaves and vines.";
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
                        chat = "The glow of these bulbs...spectacular....";
                }
            }
            else if (npc.type == NPCID.TaxCollector) //hates the verdant
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                {
                    if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(5))
                        chat = "Bah! These leaves keep on getting in the house. Have someone clean them up for me!";
                    else if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(5))
                        chat = "Those bright flowers are getting on my nerve! Keep your blasted glow out of my sight!";
                }
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
            {
                if (type == NPCID.DyeTrader) //sells pink/red dye
                {
                    shop.item[nextSlot++].SetDefaults(ItemID.BrightPinkDye);
                    shop.item[nextSlot++].SetDefaults(ItemID.BrightRedDye);
                }
            }
        }
    }
}