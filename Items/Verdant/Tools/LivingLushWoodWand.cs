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
            item.CloneDefaults(ItemID.LivingWoodWand);

            item.Size = new Vector2(36, 28);
            item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Blocks.LivingLushWood>(); //Place type
            item.tileWand = ModContent.ItemType<VerdantWoodBlock>(); //"Ammo"
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.autoReuse = true;
            item.rare = ItemRarityID.Green;
        }
    }
}
