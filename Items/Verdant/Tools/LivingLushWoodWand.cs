using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;


namespace Verdant.Items.Verdant.Tools
{
    [Sacrifice(1)]
    class LivingLushWoodWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Living Lush Wood Wand");
            // Tooltip.SetDefault("Places living lush wood blocks\nRight click to switch to Lush Leaves");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LivingWoodWand);

            Item.Size = new Vector2(36, 28);
            Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Blocks.LivingLushWood>();
            Item.tileWand = ModContent.ItemType<VerdantWoodBlock>();
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
                Item.SetDefaults(ModContent.ItemType<LushLeafWand>());
            return player.altFunctionUse != 2;
        }
    }
}
