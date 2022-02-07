using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Verdant.Tiles.Verdant.Basic.Plants;
using Terraria.GameContent.Biomes;

using static Terraria.ModLoader.ModContent;
using static Terraria.WorldGen;

namespace Verdant.Items
{
    public class ProbablyDelete : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("@ me if you see this LOL");
		}

        public override void SetDefaults() 
		{
			item.damage = 120;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 2;
			item.useAnimation = 2;
            item.maxStack = 50;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;

            item.createTile = TileType<VerdantLightbulb>();
            item.placeStyle = 0;
        }

        public override bool UseItem(Player player)
        {
            int i = Helper.MouseTile().X;
            int j = Helper.MouseTile().Y;
            Tile t = Framing.GetTileSafely(i, j);
            //for (int n = -10; n < 10; ++n)
            //    for (int b = -10; b < 10; ++b)
            //        Framing.GetTileSafely(i + n, j + b).liquid = 255;
            return false;
        }
    }
}