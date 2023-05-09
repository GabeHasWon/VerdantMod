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
    public override bool IsLoadingEnabled(Mod mod) => false;

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
        //Item.createTile = ModContent.TileType<MysteriaTree>();
    }

    public override bool? UseItem(Player player)
    {
        //var pos = Main.MouseWorld.ToTileCoordinates();
        //int groundCount = Helper.TileRectangle(pos.X, pos.Y + 6, 6, 5, ModContent.TileType<VerdantGrassLeaves>(), ModContent.TileType<LushSoil>());
        //if (Helper.NoTileRectangle(pos.X, pos.Y, 6, 6) > 4 && groundCount > 25)
        //    StructureHelper.Generator.GenerateStructure("World/Structures/SnailStatue", new Point16(pos.X, pos.Y), VerdantMod.Instance);

        //if (!ForegroundManager.Items.Any(x => x is MysteriaDrapes drape && drape.position == pos.ToWorldCoordinates()))
        //    ForegroundManager.AddItem(new MysteriaDrapes(pos), true);

        //Tile tile = Main.tile[pos];
        //tile.TileFrameX = 0;
        //tile.TileFrameY = 0;
        //Main.NewText(tile.TileFrameX + " " + tile.TileFrameY);

        //var gen = ModContent.GetInstance<RealtimeGen>();
        //gen.CurrentActions.Add(new(MysteriaTree.RealtimeGenerate(pos.X, pos.Y, player.Center.X / 16 > pos.X ? -1 : 1, Main.rand), 0.3f));

        //return true;
        //if (!RealtimeGen.HasStructure("Testing"))
        //    Spawn(pos);
        //Main.NewText(Main.MouseWorld.ToTileCoordinates());
        
        DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".TRAILER");
        return true;
    }
}