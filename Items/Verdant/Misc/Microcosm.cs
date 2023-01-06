using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.World;
using Verdant.Systems.RealtimeGeneration;

namespace Verdant.Items.Verdant.Misc;

[Sacrifice(1)]
class Microcosm : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Microcosm");
        Tooltip.SetDefault("Creates a miniscule Verdant biome anywhere\nCaution! This overrides a LARGE area (minimum 60x60 tiles on a small world)\nMake sure you use this in a place you don't care about!\nCan only be used once per world");
    }

    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.buyPrice(gold: 30);
        Item.consumable = true;
        Item.width = 24;
        Item.height = 34;
        Item.useAnimation = Item.useTime = 20;
        Item.useStyle = ItemUseStyleID.HoldUp;
    }

    public override bool CanUseItem(Player player) => !ModContent.GetInstance<VerdantSystem>().microcosmUsed;

    public override bool? UseItem(Player player)
    {
        ModContent.GetInstance<VerdantSystem>().microcosmUsed = true;

        var gen = ModContent.GetInstance<RealtimeGen>();
        gen.CurrentAction = new(MicroVerdantGen.MicroVerdant(), 12f);
        return true;
    }
}