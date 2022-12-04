using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items;

public class ProbablyDelete : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => false;

    public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("eg");
		Tooltip.SetDefault("@ me if you see this");
	}

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
	}

    public override bool? UseItem(Player player)
    {
        ScreenTextManager.CurrentText = ApotheosisDialogueCache.IntroDialogue(false);
        //var pos = Main.MouseWorld.ToTileCoordinates();
        //RandomUpdating.Auto(pos.X, pos.Y, false, 3);
        return true;
    }
}