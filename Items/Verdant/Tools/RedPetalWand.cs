using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Tools
{
    [Sacrifice(1)]
    class RedPetalWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Petal Wand");
            Tooltip.SetDefault("Places red petal blocks\nRight click to switch to Pink Petals");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.LivingWoodWand);

            Item.Size = new Vector2(34, 34);
            Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Blocks.VerdantRedPetal>();
            Item.tileWand = ModContent.ItemType<RedPetal>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Red;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                Item.SetDefaults(ModContent.ItemType<PinkPetalWand>());
            return player.altFunctionUse != 2;
        }
    }
}
