using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class LightbulbSeeds : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lightbulb Seeds", "'These emenate the faintest light already'");
        public override void SetDefaults() => QuickItem.SetBlock(this, 22, 28, ModContent.TileType<Tiles.Verdant.Basic.Plants.VerdantLightbulb>());

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight(item.Center, new Vector3(0.2f, 0.06f, 0.12f));
        }
    }
}