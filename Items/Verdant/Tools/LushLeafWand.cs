using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Tools
{
    class LushLeafWand : ModItem
    {
        public override void SetDefaults()
        {
            QuickItem.SetBlock(this, 36, 28, TileType<Tiles.Verdant.Basic.Blocks.VerdantSoilGrass>(), false);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lush Leaf Wand");
            Tooltip.SetDefault("Uses lush leaves\nPlaces leaves\nWIP: Does not consume leaves on use yet");
        }
    }
}
