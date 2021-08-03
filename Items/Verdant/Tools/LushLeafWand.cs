using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Tools
{
    class LushLeafWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lush Leaf Wand");
            Tooltip.SetDefault("Places lush leaf blocks");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LivingWoodWand);

            item.Size = new Vector2(36, 28);
            item.createTile = TileType<Tiles.Verdant.Basic.Blocks.VerdantSoilGrass>(); //Place type
            item.tileWand = ItemType<LushLeaf>(); //"Ammo"
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.autoReuse = true;
            item.rare = ItemRarityID.Green;
        }
    }
}
