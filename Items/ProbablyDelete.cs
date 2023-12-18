using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items;

public class ProbablyDelete : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) =>
#if DEBUG
        true;
#else
        false;
#endif

    public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;

    public override void SetDefaults()
	{
		Item.DamageType = DamageClass.Melee;
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
        Item.placeStyle = 0;
        //Item.shoot = ModContent.ProjectileType<HealPlants>();
        //Item.createWall = ModContent.WallType<BluescreenWall>();
        Item.createTile = ModContent.TileType<LilyPad>();
    }

    public override bool? UseItem(Player player)
    {
        Tile tile = Main.tile[Main.MouseWorld.ToTileCoordinates()];
        tile.HasTile = false;
        return true;
    }
}