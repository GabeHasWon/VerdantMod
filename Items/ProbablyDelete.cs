using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;

namespace Verdant.Items;

public class ProbablyDelete : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => true;

    public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("");
		Tooltip.SetDefault("@ me if you see this");
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
		Item.autoReuse = false;
        Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.Beehive>();
        //Item.createWall = ModContent.WallType<VerdantVineWall_Unsafe>();
        Item.placeStyle = 0;
	}

    public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
    {
		ScreenTextManager.CurrentText = new ScreenText("Hello, traveller.", 50).
			With(new ScreenText("It's been a long time since I've seen a new face.", 100, 0.8f)).
			With(new ScreenText("Find us at the center of this land,", 60, 0.9f)).
			FinishWith(new ScreenText("and we might have some gifts to help you along.", 80));
        return true;
    }
}