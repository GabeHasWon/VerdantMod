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
using Verdant.Tiles.Verdant.Trees;
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
		Item.autoReuse = false;
        Item.placeStyle = 0;
        //Item.shoot = ModContent.ProjectileType<HealPlants>();
        //Item.createWall = ModContent.WallType<BluescreenWall>();
        //Item.createTile = ModContent.TileType<MysteriaTree>();
    }

    public override bool? UseItem(Player player)
    {
        //ScreenTextManager.CurrentText = ApotheosisDialogueCache.IntroDialogue(false);
        var pos = Main.MouseWorld.ToTileCoordinates();
        GenerateMysteriaTree(pos.X, pos.Y);
        //Tile tile = Main.tile[pos];
        //tile.TileFrameX = 0;
        //tile.TileFrameY = 0;
        //Main.NewText(tile.TileFrameX + " " + tile.TileFrameY);

        //return true;
        //if (!RealtimeGen.HasStructure("Testing"))
        //    Spawn(pos);
        //Main.NewText(Main.MouseWorld.ToTileCoordinates());
        return true;
    }

    public void GenerateMysteriaTree(int x, int y)
    {
        var random = Main.rand;
        int height = random.Next(3, 8);

        int[] widths = new int[7] { random.Next(4, 7), random.Next(3, 5), random.Next(2, 4), random.Next(1, 3), 1, 1, 1 };
        int dir = random.NextBool(2) ? -1 : 1;
        int index = 0;

        for (int j = y; j > y - height; --j)
        {
            int width = index >= height - 2 ? 1 : widths[index];

            for (int i = 0; i < width; ++i)
            {
                WorldGen.PlaceTile(x + (i * dir), j, ModContent.TileType<MysteriaTree>(), true, false);
                WorldGen.PlaceTile(x + (i * dir), j + 1, ModContent.TileType<MysteriaTree>(), true, false);
            }

            if (index == height - 1)
                WorldGen.PlaceTile(x, j - 1, ModContent.TileType<MysteriaTreeTop>(), true, false);

            x += width * dir;
            index++;
        }
    } 
}