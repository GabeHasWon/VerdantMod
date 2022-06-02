using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Materials
{
    class YellowBulb : ModItem
    {
        public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 28, ItemRarityID.Yellow, 999, false, Item.buyPrice(0, 1, 0, 0));
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Yellow Bulb", "'So rare...'");
        public override void Update(ref float gravity, ref float maxFallSpeed) => maxFallSpeed = 0.9f;
    }
}