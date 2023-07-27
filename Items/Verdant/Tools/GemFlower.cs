using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Blocks.Misc;
using Verdant.World;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(1)]
class GemFlower : ModItem
{
    public override void SetDefaults() => QuickItem.SetMaterial(this, 30, 30, ItemRarityID.Green, 1, false, Item.buyPrice(0, 0, 50, 0), true);
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<YellowBulb>(), 2), (ModContent.ItemType<GreenCrystalItem>(), 5));

    public override bool? UseItem(Player player)
    {
        var raw = ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation;

        if (raw is null)
            Main.NewText("Talk to the Apotheosis first!");
        else
            player.Teleport(raw.Value.ToWorldCoordinates(), TeleportationStyleID.MagicConch, 0);
        return true;
    }
}
