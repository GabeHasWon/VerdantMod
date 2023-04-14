using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Effects;
using Verdant.Projectiles.Magic;
using Verdant.Systems.RealtimeGeneration;
using Verdant.Systems.RealtimeGeneration.CaptureRendering;
using Verdant.Systems.RealtimeGeneration.Old;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor;
using Verdant.Tiles.Verdant.Misc;
using Verdant.Walls;

namespace Verdant.Items;

public class ProbablyDelete : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => true;

    public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Tester Tool");
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
        //Item.shoot = ModContent.ProjectileType<HealPlants>();
        Item.createWall = ModContent.WallType<BluescreenWall>();
	}

    public override bool? UseItem(Player player)
    {
        //ScreenTextManager.CurrentText = ApotheosisDialogueCache.IntroDialogue(false);
        var pos = Main.MouseWorld.ToTileCoordinates();

        Tile tile = Main.tile[pos];
        //tile.TileFrameX = 0;
        //tile.TileFrameY = 0;
        //Main.NewText(tile.TileFrameX + " " + tile.TileFrameY);

        //return true;
        //if (!RealtimeGen.HasStructure("Testing"))
        //    Spawn(pos);
        //Main.NewText(Main.MouseWorld.ToTileCoordinates());
        return null;
    }
}