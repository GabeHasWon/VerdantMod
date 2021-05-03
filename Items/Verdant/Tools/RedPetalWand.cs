using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Tools
{
    class RedPetalWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Petal Wand");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LivingWoodWand); //probably dont need this lol

            item.Size = new Vector2(34, 34);
            item.createTile = TileType<Tiles.Verdant.Basic.Blocks.VerdantRedPetal>(); //Place type
            item.tileWand = ItemType<RedPetal>(); //"Ammo"
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.Red;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.autoReuse = true;
        }
    }
}
