using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Equipables;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Tools;
using Verdant.Items.Verdant.Weapons;

namespace Verdant.Items.Verdant.Fishing
{
    [Sacrifice(10)]
    public class LushWoodCrateItem : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lush Wood Crate", "Right click to open");
        public override void SetDefaults() => QuickItem.SetBlock(this, 32, 32, ModContent.TileType<Tiles.Verdant.Decor.LushFurniture.LushWoodCrate>(), maxStack: 99);
        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            var halfSproutRule = ItemDropRule.Common(ModContent.ItemType<Halfsprout>(), 5, 20, 31);
            halfSproutRule.OnFailedRoll(ItemDropRule.OneFromOptions(1, ModContent.ItemType<VerdantStaff>(), ModContent.ItemType<Lightbloom>(), ModContent.ItemType<ExpertPlantGuide>(), ModContent.ItemType<HealingFlowerItem>(), ModContent.ItemType<LushLeafWand>()));

            itemLoot.Add(halfSproutRule);

            int[] itemIDArray = new int[] { ItemID.IronskinPotion, ItemID.ThornsPotion, ItemID.ThrowingKnife, ModContent.ItemType<PinkPetal>(), ModContent.ItemType<RedPetal>(), ModContent.ItemType<Lightbulb>(),
                ItemID.Dynamite, ItemID.Glowstick, ItemID.Bomb, ItemID.NightOwlPotion, ItemID.HealingPotion, ItemID.MoonglowSeeds, ItemID.DaybloomSeeds, ItemID.BlinkrootSeeds };
            (int, int)[] itemStackArray = new (int, int)[] { (1, 3), (1, 3), (3, 7), (9, 14), (9, 14), (1, 3), (1, 1), (3, 8), (2, 4), (2, 4), (2, 4), (2, 4), (2, 4), (2, 4) };

            itemLoot.Add(new LootPoolDrop(itemStackArray, 4, 1, 1, itemIDArray));
        }
    }
}