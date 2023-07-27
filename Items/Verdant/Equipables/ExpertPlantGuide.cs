using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Equipables
{
    class ExpertPlantGuide : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Expert Guide to Plant Fiber Cordage");
            // Tooltip.SetDefault("Allows mastery of a very niche element\nVines can be harvested from the Verdant\nVines harvested from the Verdant place significantly faster\nVines climb upwards when there's no space below");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<VerdantPlayer>().expertPlantGuide = true;
    }
}
