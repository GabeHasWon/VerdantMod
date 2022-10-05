using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items;

public class ProbablyDelete : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => false;

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
		Item.autoReuse = false;
        Item.createTile = ModContent.TileType<Tiles.Verdant.Basic.LootPlant>();
        //Item.createWall = ModContent.WallType<VerdantVineWall_Unsafe>();
        Item.placeStyle = 0;
	}

    public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
    {
        var j = Main.MouseWorld.ToTileCoordinates16();
        //WorldGen.GrowTree(j.X, j.Y);
        //Point[] offsets = new Point[3] { new Point(7, -1), new Point(3, 0), new Point(3, 0) }; //ruler in-game is ONE HIGHER on both planes

        //int index = Main.rand.Next(offsets.Length);
        //Point16 pos = j;

        //StructureHelper.Generator.GenerateMultistructureSpecific("World/Structures/Flowers", pos, Mod, index);

        //int x = pos.X + offsets[index].X;
        //int y = pos.Y + offsets[index].Y + 2;

        //if (!WorldGen.genRand.NextBool(4)) //NORMAL chests
        //{
        //    bool c = GenHelper.PlaceChest(x, y, ModContent.TileType<VerdantYellowPetalChest>(), new (int, int)[]
        //    {
        //            (ModContent.ItemType<VerdantStaff>(), 1), (ModContent.ItemType<Lightbloom>(), 1), (ModContent.ItemType<ExpertPlantGuide>(), 1), (ModContent.ItemType<Halfsprout>(), WorldGen.genRand.Next(20, 31))
        //    }, new (int, int)[] {
        //            (ItemID.IronskinPotion, WorldGen.genRand.Next(1, 3)), (ItemID.ThornsPotion, WorldGen.genRand.Next(1, 3)), (ItemID.ThrowingKnife, WorldGen.genRand.Next(3, 7)),
        //            (ModContent.ItemType<PinkPetal>(), WorldGen.genRand.Next(3, 7)), (ModContent.ItemType<RedPetal>(), WorldGen.genRand.Next(3, 7)), (ModContent.ItemType<Lightbulb>(), WorldGen.genRand.Next(1, 3)),
        //            (ItemID.Dynamite, 1), (ItemID.Glowstick, WorldGen.genRand.Next(3, 8)), (ItemID.Glowstick, WorldGen.genRand.Next(3, 8)), (ItemID.Bomb, WorldGen.genRand.Next(2, 4)),
        //            (ItemID.NightOwlPotion, WorldGen.genRand.Next(2, 4)), (ItemID.HealingPotion, WorldGen.genRand.Next(2, 4)), (ItemID.MoonglowSeeds, WorldGen.genRand.Next(2, 4)),
        //            (ItemID.DaybloomSeeds, WorldGen.genRand.Next(2, 4)), (ItemID.BlinkrootSeeds, WorldGen.genRand.Next(2, 4))
        //    }, true, WorldGen.genRand, WorldGen.genRand.Next(4, 7), 0, false);

        //    if (!c)
        //        Mod.Logger.Warn("Failed to place Verdant Yellow Petal Chest.");
        //}
        //else //WAND chest
        //{
        //    bool c = GenHelper.PlaceChest(x, y, ModContent.TileType<VerdantYellowPetalChest>(), 0, false,
        //        Helper.ItemStack<LushLeafWand>(), Helper.ItemStack<PinkPetalWand>(), Helper.ItemStack<RedPetalWand>(), Helper.ItemStack<RedPetal>(WorldGen.genRand.Next(19, 24)),
        //        (ModContent.ItemType<PinkPetal>(), WorldGen.genRand.Next(19, 24)), (ModContent.ItemType<VerdantFlowerBulb>(), WorldGen.genRand.Next(12, 22)));

        //    if (!c)
        //        Mod.Logger.Warn("Failed to place Verdant Yellow Petal Chest (wand).");
        //}
        return true;
    }
}