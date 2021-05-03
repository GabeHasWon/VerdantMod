using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Items.Verdant.Tools
{
    class PinkPetalWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pink Petal Wand");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.LivingWoodWand);

            item.Size = new Vector2(34, 34);
            item.createTile = TileType<Tiles.Verdant.Basic.Blocks.VerdantPinkPetal>(); //Place type
            item.tileWand = ItemType<PinkPetal>(); //"Ammo"
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.Pink;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 15;
            item.autoReuse = true;
        }
    }
}
