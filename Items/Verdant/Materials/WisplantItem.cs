using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Materials
{
    class WisplantItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Wisplant");
        public override void SetDefaults() => QuickItem.SetMaterial(this, 16, 24, ItemRarityID.White, 999, false, Item.buyPrice(0, 0, 0, 5));
    }
}