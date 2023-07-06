using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ObjectData;
using Verdant.Effects;
using Verdant.Projectiles.Magic;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Tiled;
using Verdant.Systems.RealtimeGeneration;
using Verdant.Systems.RealtimeGeneration.CaptureRendering;
using Verdant.Systems.RealtimeGeneration.Old;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor;
using Verdant.Tiles.Verdant.Misc;
using Verdant.Tiles.Verdant.Trees;
using Verdant.Walls;
using Verdant.World;

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
        //Item.createWall = ModContent.WallType<BluescreenWall>();
        Item.createTile = ModContent.TileType<WaterberryBush>();
    }

    readonly static int[] InvalidTypes = new int[] { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.LihzahrdBrick };
    readonly static int[] InvalidWalls = new int[] { WallID.BlueDungeonSlabUnsafe, WallID.BlueDungeonUnsafe, WallID.BlueDungeonTileUnsafe, WallID.GreenDungeonSlabUnsafe, WallID.GreenDungeonTileUnsafe,
            WallID.GreenDungeonUnsafe, WallID.PinkDungeonUnsafe, WallID.PinkDungeonTileUnsafe, WallID.PinkDungeonSlabUnsafe };

    public override bool? UseItem(Player player)
    {
        return true;
        Point apothPos = Main.MouseWorld.ToTileCoordinates();
        AquamarineGen.SingleAquamarine(apothPos.X, apothPos.Y);
        return true;
    }
}