using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class LightbulbSeeds : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lightbulb Seeds", "'These emenate the faintest light already'");

        public override void SetDefaults()
        {
            QuickItem.SetBlock(this, 22, 28, ModContent.TileType<Tiles.Verdant.Basic.Plants.VerdantLightbulb>());
            Item.value = Item.buyPrice(0, 0, 0, 50);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed) => Lighting.AddLight(Item.Center, new Vector3(0.25f, 0.09f, 0.18f));
    }
}