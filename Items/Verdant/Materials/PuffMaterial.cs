using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Materials
{
    class PuffMaterial : ModItem
    {
        public override void SetDefaults() => QuickItem.SetMaterial(this, 28, 26, ItemRarityID.Blue, 999, false, Item.buyPrice(0, 0, 0, 10));
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Puff", "Light and soft to the touch\nTastes bad");
        public override void Update(ref float gravity, ref float maxFallSpeed) => maxFallSpeed = 0.8f;
    }
}