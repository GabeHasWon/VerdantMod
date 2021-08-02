using System;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;

namespace Verdant.Items.Verdant.Materials
{
    class RedPetal : ModItem
    {
        int updateCounter = 0;

        public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 28, ItemRarityID.White, 999);
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Red Petal", "'They're very smooth'");
        public override void AddRecipes() => QuickItem.AddRecipe(this, mod, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantRedPetalWallItem>(), 1));

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (item.velocity.Y > 0.10f)
                item.velocity.X = (float)-Math.Sin(updateCounter++ * 0.03f) * 1.3f * item.velocity.Y * (1 - (item.stack / 999f));

            gravity = 0.05f;
            maxFallSpeed = 0.9f;
        }
    }
}