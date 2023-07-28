using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Blocks.Plants
{
    public class LightbulbSeeds : ModItem
    {
        public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;

        public override void SetDefaults()
        {
            QuickItem.SetBlock(this, 22, 28, ModContent.TileType<Tiles.Verdant.Basic.Plants.VerdantLightbulb>());
            Item.value = Item.buyPrice(0, 0, 0, 50);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed) => Lighting.AddLight(Item.Center, new Vector3(0.25f, 0.09f, 0.18f));
    }
}