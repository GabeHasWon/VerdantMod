using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Equipables;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Tools;
using Verdant.Items.Verdant.Weapons;

namespace Verdant.Items.Verdant.Misc
{
    class PassionflowerBulb : ModItem
    {
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Passionflower Bulb", "Right click to open\n'Smells of citrus and sweetness'");
        public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 22, ItemRarityID.Lime, 1);
        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            var mainLoot = new (int, int)[]
            {
                (ModContent.ItemType<VerdantStaff>(), 1), (ModContent.ItemType<Lightbloom>(), 1), (ModContent.ItemType<ExpertPlantGuide>(), 1), (ModContent.ItemType<Halfsprout>(), WorldGen.genRand.Next(20, 31))
            };

            var subLoot = new (int, int)[] 
            {
                (ItemID.IronskinPotion, WorldGen.genRand.Next(1, 3)), (ItemID.ThornsPotion, WorldGen.genRand.Next(1, 3)), (ItemID.ThrowingKnife, WorldGen.genRand.Next(3, 7)),
                (ModContent.ItemType<PinkPetal>(), WorldGen.genRand.Next(9, 14)), (ModContent.ItemType<RedPetal>(), WorldGen.genRand.Next(9, 14)), (ModContent.ItemType<Lightbulb>(), WorldGen.genRand.Next(1, 3)),
                (ItemID.Dynamite, 1), (ItemID.Glowstick, WorldGen.genRand.Next(3, 8)), (ItemID.Glowstick, WorldGen.genRand.Next(3, 8)), (ItemID.Bomb, WorldGen.genRand.Next(2, 4)),
                (ItemID.NightOwlPotion, WorldGen.genRand.Next(2, 4)), (ItemID.HealingPotion, WorldGen.genRand.Next(2, 4)), (ItemID.MoonglowSeeds, WorldGen.genRand.Next(2, 4)),
                (ItemID.DaybloomSeeds, WorldGen.genRand.Next(2, 4)), (ItemID.BlinkrootSeeds, WorldGen.genRand.Next(2, 4))
            };

            (int mainType, int mainStack) = Main.rand.Next(mainLoot);
            player.QuickSpawnItem(player.GetSource_OpenItem(Type), mainType, mainStack);

            int subReps = Main.rand.Next(6, 9);
            for (int i = 0; i < subReps; ++i)
            {
                (int subType, int subStack) = Main.rand.Next(subLoot);
                player.QuickSpawnItem(player.GetSource_OpenItem(Type), subType, subStack);
            }
        }
    }
}
