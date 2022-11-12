using Microsoft.Xna.Framework;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Tools
{
    [Sacrifice(1)]
    class PinkPetalWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pink Petal Wand");
            Tooltip.SetDefault("Places pink petal blocks\nRight click to switch to Living Lush Wood");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LivingWoodWand);

            Item.Size = new Vector2(34, 34);
            Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Blocks.VerdantPinkPetal>();
            Item.tileWand = ModContent.ItemType<PinkPetal>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Pink;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                Item.SetDefaults(ModContent.ItemType<LivingLushWoodWand>());
            return player.altFunctionUse != 2;
        }
    }
}
