using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Equipables
{
    class Blight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blightlight");
            Tooltip.SetDefault("An old growth that suffered inside of a chest\nSpreads glowing infection to nearby enemies");
        }

        public override void SetDefaults()
        {
            QuickItem.SetLightPet(this, 38, 32);
        }

        public override bool CanUseItem(Player player) => player.miscEquips[1].IsAir;
    }
}
