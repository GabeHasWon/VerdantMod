using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Global;

[ReinitializeDuringResizeArrays]
internal class VerdantItemSets : ModSystem
{
    public static readonly bool[] Gemstone = ItemID.Sets.Factory.CreateNamedSet(nameof(Gemstone)).Description("Whether this item is a gem").RegisterBoolSet(ItemID.Amethyst, ItemID.Topaz, 
        ItemID.Sapphire, ItemID.Ruby, ItemID.Emerald, ItemID.Diamond, ItemID.Amber);

    public static readonly bool[] HeldProjectile = ProjectileID.Sets.Factory.CreateNamedSet(VerdantMod.Instance, "HeldProjectile")
    .Description("If a projectile is a held projectile.").RegisterBoolSet(false);

    public static readonly bool[] SkipAutoHeldCheck = ProjectileID.Sets.Factory.CreateNamedSet(VerdantMod.Instance, nameof(SkipAutoHeldCheck))
        .Description("Whether this projectile ID skips the auto-check for Reforged's automatic held projectile detection system.")
        .RegisterBoolSet(false);

    public override void PostSetupContent()
    {

    }
}
