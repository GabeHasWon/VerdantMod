using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Utilities;

namespace Verdant.Items.Verdant.Blocks.MusicBoxes;

[Sacrifice(1)]
public class BaseMusicBoxItem<T> : ModItem where T : ModTile
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.CanGetPrefixes[Type] = false;
        ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
    }

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.autoReuse = true;
        Item.consumable = true;
        Item.createTile = ModContent.TileType<T>();
        Item.width = 32;
        Item.height = 24;
        Item.rare = ItemRarityID.LightRed;
        Item.value = 100000;
        Item.accessory = true;
        Item.maxStack = 1;
    }

    public override bool? PrefixChance(int pre, UnifiedRandom rand)
    {
        return base.PrefixChance(pre, rand);
    }
}
