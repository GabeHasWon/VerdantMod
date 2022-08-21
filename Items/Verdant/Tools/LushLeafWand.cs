using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

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
            Item.CloneDefaults(ItemID.LivingWoodWand);

            Item.Size = new Vector2(36, 28);
            Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Blocks.VerdantGrassLeaves>(); //Place type
            Item.tileWand = ModContent.ItemType<LushLeaf>(); //"Ammo"
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
        }
    }
}
