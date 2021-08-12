using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static Terraria.WorldGen;
using Verdant.Backgrounds.BGItem;
using Microsoft.Xna.Framework;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor.LushFurniture;
using Verdant.Backgrounds.BGItem.Verdant;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.World;
using Verdant.Walls.Verdant;
using System.Collections.ObjectModel;
using Terraria.DataStructures;
using Verdant.Tiles.Verdant.Decor;
using Verdant.Items.Verdant.Equipables;
using Verdant.Items.Verdant.Weapons;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Tools;
using Verdant.Items.Verdant.Blocks;

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
			item.damage = 120;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 2;
			item.useAnimation = 2;
            item.maxStack = 50;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
            item.createTile = TileType<Tiles.Verdant.Decor.Apotheosis>();
        }

        public override void OnCraft(Recipe recipe)
        {
        }

        public override bool UseItem(Player player)
        {
            int i = Helper.MouseTile().X;
            int j = Helper.MouseTile().Y;
            Tile t = Framing.GetTileSafely(i, j);

            //Tiles.Verdant.Trees.VerdantTree.Spawn(i, j, -1, Main.rand, 12, 19, true, -1, true);

            //Foreground.ForegroundManager.AddItem(new Foreground.Parallax.LushLeafFG(Main.MouseWorld));

            //Main.NewText($"RightSlope: {t.rightSlope()}");
            //Main.NewText($"LeftSlope: {t.leftSlope()}");
            //Main.NewText($"TopSlope: {t.topSlope()}");
            //Main.NewText($"BottomSlope: {t.bottomSlope()}");
            //Main.NewText("---");

            //BackgroundItemManager.AddItem(new LushBushBG(Main.MouseWorld));
            //Point16 pos = new Point16(i, j);

            //int index = Main.rand.Next(2);

            //Point[] offsets = new Point[2] { new Point(5, 5), new Point(6, 5) };
            //int[] invalids = new int[] { TileID.LihzahrdBrick, TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick };
            //int[] valids = new int[] { TileType<VerdantSoilGrass>(), TileType<LushSoil>() };

            //StructureHelper.StructureHelper.GenerateMultistructureSpecific("World/Structures/Flowers", pos, mod, index);

            ////GenHelper.KillRectangle(pos.X + offsets[index].X, pos.Y + offsets[index].Y, 2, 2);

            //if (false) //NORMAL chests
            //{
            //    //WorldGen.PlaceTile(pos.X + offsets[index].X, pos.Y + offsets[index].Y, TileID.Meteorite);
            //    bool c = GenHelper.PlaceChest(pos.X + offsets[index].X, pos.Y + offsets[index].Y + 1, TileType<VerdantYellowPetalChest>(), new (int, int)[]
            //    {
            //                (ItemType<VerdantStaff>(), 1), (ItemType<VerdantSnailStaff>(), 1), (ItemType<Lightbloom>(), 1)
            //    }, new (int, int)[] {
            //                (ItemID.IronskinPotion, genRand.Next(1, 3)), (ItemID.ThornsPotion, genRand.Next(1, 3)), (ItemID.ThrowingKnife, genRand.Next(3, 7)),
            //                (ItemType<PinkPetal>(), genRand.Next(3, 7)), (ItemType<RedPetal>(), genRand.Next(3, 7)), (ItemType<Lightbulb>(), genRand.Next(1, 3)),
            //                (ItemID.Dynamite, 1), (ItemID.Glowstick, genRand.Next(3, 8)), (ItemID.Glowstick, genRand.Next(3, 8)), (ItemID.Bomb, genRand.Next(2, 4)),
            //                (ItemID.NightOwlPotion, genRand.Next(2, 4)), (ItemID.HealingPotion, genRand.Next(2, 4)), (ItemID.MoonglowSeeds, genRand.Next(2, 4)),
            //                (ItemID.DaybloomSeeds, genRand.Next(2, 4)), (ItemID.BlinkrootSeeds, genRand.Next(2, 4))
            //    }, true, genRand, genRand.Next(4, 7), 0, true);

            //    if (!c)
            //        Main.NewText("Failed to place Verdant Yellow Petal Chest.");
            //}
            //else //WAND chest
            //{
            //    bool c = GenHelper.PlaceChest(pos.X + offsets[index].X, pos.Y + offsets[index].Y + 1, TileType<VerdantYellowPetalChest>(), 0, false,
            //        (ItemType<LushLeafWand>(), 1), (ItemType<PinkPetalWand>(), 1), (ItemType<RedPetalWand>(), 1), (ItemType<LushLeaf>(), genRand.Next(20, 34)),
            //        (ItemType<PinkPetal>(), genRand.Next(19, 24)), (ItemType<RedPetal>(), genRand.Next(15, 23)), (ItemType<VerdantFlowerBulb>(), genRand.Next(12, 22)));

            //    if (!c)
            //        Main.NewText("Failed to place Verdant Yellow Petal Chest. [WAND]");
            //}
            return true;
        }
    }
}