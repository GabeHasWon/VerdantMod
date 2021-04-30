using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Verdant.Backgrounds.BGItem;
using Microsoft.Xna.Framework;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor.LushFurniture;
using Verdant.Backgrounds.BGItem.Verdant;
using Verdant.Tiles;
using Verdant.World;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items
{
    public class ProbablyDelete2 : ModItem
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
			item.useTime = 10;
			item.useAnimation = 10;
            item.maxStack = 50;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
            //item.createTile = TileType<Tiles.Verdant.Basic.Plants.OceanKelp>();
        }

        public override bool UseItem(Player player)
        {
            int i = Helper.MouseTile().X;
            int j = Helper.MouseTile().Y;

            int height = Main.rand.Next(9, 24);
            GenHelper.GenBezierDirect(new double[] {
                i, j,
                i + 30, j + height,
                i + 60, j,
            }, 200, TileType<VerdantLeaves>(), true, 1);
            GenHelper.GenBezierDirect(new double[] {
                i, j,
                i + 30, j + height - 1,
                i + 60, j - 1,
            }, 200, TileType<VerdantLeaves>(), true, 1);
            return true;
        }
    }
}