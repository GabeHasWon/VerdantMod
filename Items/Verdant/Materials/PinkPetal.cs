using System;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Items.Verdant.Materials
{
    class PinkPetal : ModItem
    {
        int updateCounter = 0;

        public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 24, ItemRarityID.White, 999, false);
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Pink Petal", "'They're very soft'");
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantPinkPetalWallItem>(), 1));

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (item.velocity.Y > 0.10f)
                item.velocity.X = (float)-Math.Sin(updateCounter++ * 0.03f) * 1.7f * item.velocity.Y * (1 - (item.stack / 999f));

            gravity = 0.02f;
            maxFallSpeed = 1f;
        }
    }
}