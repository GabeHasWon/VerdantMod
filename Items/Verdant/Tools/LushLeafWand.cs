using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Tools
{
    [Sacrifice(1)]
    class LushLeafWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lush Leaf Wand");
            Tooltip.SetDefault("Places lush leaf blocks\nRight click to switch to Red Petals");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LivingWoodWand);

            Item.Size = new Vector2(36, 28);
            Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Blocks.VerdantGrassLeaves>();
            Item.tileWand = ModContent.ItemType<LushLeaf>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                Item.SetDefaults(ModContent.ItemType<RedPetalWand>());
            return player.altFunctionUse != 2;
        }
    }
}
