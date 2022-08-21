using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;

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
			Item.damage = 120;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 2;
			Item.useAnimation = 2;
            Item.maxStack = 50;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.createTile = ModContent.TileType<VerdantLightbulb>();
            Item.placeStyle = 0;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
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