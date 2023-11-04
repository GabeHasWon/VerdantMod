using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Plants;

public class PassionflowerBlock : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;

    public override void SetDefaults()
    {
        QuickItem.SetBlock(this, 22, 28, ModContent.TileType<Tiles.Verdant.Basic.LootPlant>(), rarity: ItemRarityID.Green);
        Item.placeStyle = 1;
    }
}