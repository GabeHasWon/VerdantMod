using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Tools
{
    class RedPetalWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Petal Wand");
            Tooltip.SetDefault("Places red petal blocks");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LivingWoodWand); //probably dont need this lol

            Item.Size = new Vector2(34, 34);
            Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Blocks.VerdantRedPetal>(); //Place type
            Item.tileWand = ModContent.ItemType<RedPetal>(); //"Ammo"
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Red;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
        }
    }
}
