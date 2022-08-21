using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Tools
{
    class PinkPetalWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pink Petal Wand");
            Tooltip.SetDefault("Places pink petal blocks");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LivingWoodWand);

            Item.Size = new Vector2(34, 34);
            Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Blocks.VerdantPinkPetal>(); //Place type
            Item.tileWand = ModContent.ItemType<PinkPetal>(); //"Ammo"
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Pink;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
        }
    }
}
