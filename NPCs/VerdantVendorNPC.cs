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
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Dryad || entity.type == NPCID.WitchDoctor;

    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Dryad || shop.NpcType == NPCID.WitchDoctor)
        {
            shop.Add(new Item(ModContent.ItemType<MysteriaAcorn>())
            {
                shopCustomPrice = Item.buyPrice(0, 0, 1, 0),
            }, Condition.Hardmode);

            shop.Add(new Item(ModContent.ItemType<WaterberryBushItem>())
            {
                shopCustomPrice = Item.buyPrice(0, 0, 15, 0),
            }, Condition.Hardmode);

            shop.Add(ModContent.ItemType<Microcosm>());
            shop.Add(ModContent.ItemType<LightbulbSeeds>());

            shop.Add(new Item(ModContent.ItemType<LushGrassSeeds>())
            {
                shopCustomPrice = Item.buyPrice(0, 0, 0, 50)
            });

            var inVerdant = new Condition("Mods.Verdant.Condition.InVerdant", () => Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant);
            shop.Add(new Item(ModContent.ItemType<ApotheoticPaintingItem>())
            {
                shopCustomPrice = Item.buyPrice(0, 5, 0, 0),
            }, inVerdant);

            shop.Add(new Item(ModContent.ItemType<LightbulbPaintingItem>())
            {
                shopCustomPrice = Item.buyPrice(0, 5, 0, 0),
            }, inVerdant);
        }
    }
}
