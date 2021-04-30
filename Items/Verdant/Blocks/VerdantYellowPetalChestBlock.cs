using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Blocks
{
    public class VerdantYellowPetalChestBlock : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Yellow Petal Chest", "");
        public override void SetDefaults() => QuickItem.SetBlock(this, 16, 16, TileType<VerdantYellowPetalChest>());
    }
}
