using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Misc;

namespace Verdant.NPCs
{
    class VerdantDryadNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Dryad;

        public override void GetChat(NPC npc, ref string chat)
        {
            if (NPC.downedMechBossAny && !ModContent.GetInstance<VerdantNPCWorld>().yellowPetalDialogue)
            {
                ModContent.GetInstance<VerdantNPCWorld>().yellowPetalDialogue = true;

                chat = "Welcome. You've beaten some of those awful machines, yes? An energy has been released...these yellow petals are beautiful. I'll have some in stock from now on.";
                return;
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type != NPCID.Dryad)
                return;

            if (!ModContent.GetInstance<VerdantSystem>().microcosmUsed)
                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Microcosm>());

            if (NPC.downedMechBossAny)
                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<YellowBulb>());
        }
    }
}
