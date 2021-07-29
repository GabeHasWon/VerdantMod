using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Misc
{
    class OvergrownScroll : ModItem
    {
        public override void SetDefaults() => QuickItem.SetMaterial(this, 12, 12, ItemRarityID.Lime, 1);
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Overgrown Scroll", "The scroll reads:\n\"It's been months since I've seen the surface,\nand all I ask for is some food -\nbut alas, there seems to be only moss and mushroom down here.\nPerhaps, somehow, I can find some lush vegetation, somewhere...\"");
    }
}
