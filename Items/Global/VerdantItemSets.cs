using Terraria.ID;

namespace Verdant.Items.Global;

internal class VerdantItemSets
{
    public static readonly bool[] Gemstone = ItemID.Sets.Factory.CreateNamedSet(nameof(Gemstone)).Description("Whether this item is a gem").RegisterBoolSet(ItemID.Amethyst, ItemID.Topaz, 
        ItemID.Sapphire, ItemID.Ruby, ItemID.Emerald, ItemID.Diamond, ItemID.Amber);
}
