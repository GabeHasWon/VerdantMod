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
                pool[NPCID.CaveBat] = 0; //on second thought, no
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
            if (npc.type == NPCID.Guide) //no one loves him but me </3
            {
                if (NPC.downedBoss2 && !World.VerdantWorld.apotheosisEvilDown && Main.rand.NextBool(4))
                    chat = "I hear a faint voice coming from those lush caves deep underground. Maybe you should check it out?";
                if (NPC.downedBoss3 && !World.VerdantWorld.apotheosisSkelDown && Main.rand.NextBool(4))
                    chat = "I hear another faint voice coming from the lush caves. Take a look there.";
            }
            else if (npc.type == NPCID.Dryad)
            {
                if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.rand.NextBool(4))
                    chat = $"I love this overgrown land. It might even let me forget about this world's {(WorldGen.crimson ? "crimson" : "corruption")}.";
            }
        }
    }
}
