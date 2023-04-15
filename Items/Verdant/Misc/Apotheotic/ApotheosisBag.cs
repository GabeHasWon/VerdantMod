using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Tools;

namespace Verdant.Items.Verdant.Misc.Apotheotic;

[Sacrifice(10)]
class ApotheosisBag : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Researcher's Bag", "Right click to open");
    public override void SetDefaults() => QuickItem.SetMaterial(this, 38, 34, ItemRarityID.Lime, 1);
    public override bool CanRightClick() => true;

    public override void ModifyItemLoot(ItemLoot itemLoot)
    {
        itemLoot.AddCommon(ModContent.ItemType<ApotheosisBook>(), 6, 20, 31);
        itemLoot.AddCommon(ModContent.ItemType<Halfsprout>(), 6, 20, 31);
    }
}
