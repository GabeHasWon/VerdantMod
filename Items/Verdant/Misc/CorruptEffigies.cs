using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Misc;

[Sacrifice(1)]
class CorruptEffigy : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => false;

    public override void SetStaticDefaults()
    {
        // Tooltip.SetDefault("Feels wrong in your hands");

        ItemID.Sets.ItemNoGravity[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Quest;
        Item.value = 0;
        Item.consumable = false;
        Item.width = 40;
        Item.height = 42;
    }
}

[Sacrifice(1)]
class CrimsonEffigy : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => false;

    public override void SetStaticDefaults()
    {
        // Tooltip.SetDefault("Feels off in your hands");

        ItemID.Sets.ItemNoGravity[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Quest;
        Item.value = 0;
        Item.consumable = false;
        Item.width = 40;
        Item.height = 38;
    }
}