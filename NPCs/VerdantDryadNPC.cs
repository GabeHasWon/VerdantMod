using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.NPCs
{
    class VerdantDryadNPC : GlobalNPC
    {
        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == NPCID.Dryad && NPC.downedMechBossAny && !ModContent.GetInstance<VerdantNPCWorld>().yellowPetalDialogue)
            {
                ModContent.GetInstance<VerdantNPCWorld>().yellowPetalDialogue = true;

                chat = "Welcome. You've beaten some of those awful machines, yes? An energy has been released...these yellow petals are beautiful. I'll have some in stock from now on.";
                return;
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Dryad && NPC.downedMechBossAny)
                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<YellowBulb>());
        }
    }
}
