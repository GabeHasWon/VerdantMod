using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Minion;

namespace Verdant.Items.Verdant.Weapons;

class Propellerpad : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Propellerpad", "Summons an odd lily pad.\nHang onto the pad's vine to fly around.", true);

    public override void SetDefaults()
    {
        QuickItem.SetStaff(this, 48, 48, ModContent.ProjectileType<PropellerpadProjectile>(), 9, 60, 24, 0, 0, ItemRarityID.Green);

        Item.accessory = true;
    }

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<PropellerpadProjectile>()] == 0;

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (player.ownedProjectileCounts[ModContent.ProjectileType<PropellerpadProjectile>()] == 0)
            Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.One, ModContent.ProjectileType<PropellerpadProjectile>(), 0, 0, player.whoAmI);
    }
}
