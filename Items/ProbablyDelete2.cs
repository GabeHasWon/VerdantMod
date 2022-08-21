using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
			Item.damage = 120;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
            Item.maxStack = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            //item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Plants.OceanKelp>();
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            int i = Helper.MouseTile().X;
            int j = Helper.MouseTile().Y;

            int height = Main.rand.Next(9, 24);
            GenHelper.GenBezierDirect(new double[] {
                i, j,
                i + 30, j + height,
                i + 60, j,
            }, 200, ModContent.TileType<VerdantLeaves>(), true, 1);
            GenHelper.GenBezierDirect(new double[] {
                i, j,
                i + 30, j + height - 1,
                i + 60, j - 1,
            }, 200, ModContent.TileType<VerdantLeaves>(), true, 1);
            return true;
        }
    }
}