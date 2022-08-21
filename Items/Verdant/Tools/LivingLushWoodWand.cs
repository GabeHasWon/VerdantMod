using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.LushWood;


namespace Verdant.Items.Verdant.Tools
{
    class LivingLushWoodWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Living Lush Wood Wand");
            Tooltip.SetDefault("Places living lush wood blocks");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LivingWoodWand);

            Item.Size = new Vector2(36, 28);
            Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Blocks.LivingLushWood>(); //Place type
            Item.tileWand = ModContent.ItemType<VerdantWoodBlock>(); //"Ammo"
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
        }
    }
}
