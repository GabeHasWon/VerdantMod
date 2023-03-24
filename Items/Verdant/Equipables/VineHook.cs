using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Equipables;

internal class VineHook : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Living Vine Hook");
        Tooltip.SetDefault("Unhooks automatically when close enough\nA nicer friend than most hooks");
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.AmethystHook);
        Item.shootSpeed = 14f;
        Item.shoot = ModContent.ProjectileType<VineHookProjectile>();
        Item.rare = ItemRarityID.Purple;
        Item.value = Item.buyPrice(0, 5, 0, 0);
    }
}