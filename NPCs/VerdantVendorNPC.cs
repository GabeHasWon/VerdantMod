using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Misc;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Misc;

namespace Verdant.NPCs;

class VerdantVendorNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Dryad;

    public override void SetupShop(int type, Chest shop, ref int nextSlot)
    {
        if (type != NPCID.Dryad && type != NPCID.WitchDoctor)
            return;

        if (Main.hardMode)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<MysteriaAcorn>());
            shop.item[nextSlot++].shopCustomPrice = Item.buyPrice(0, 0, 1, 0);
        }

        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Microcosm>());

        shop.item[nextSlot].SetDefaults(ModContent.ItemType<WaterberryBushItem>());
        shop.item[nextSlot++].shopCustomPrice = Item.buyPrice(0, 0, 15, 0);

        if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant && ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown)
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<LightbulbSeeds>());

        if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
        {
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ApotheoticPaintingItem>());
            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<LightbulbPaintingItem>());
        }
    }
}
