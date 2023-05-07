using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Buffs.Minion;
using Verdant.Projectiles.Minion;

namespace Verdant.Items.Verdant.Equipables;

class Propellerpad : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Propellerpad", "Summons an odd lily pad.\nHang onto the pad's vine to fly around.", true);

    public override void SetDefaults()
    {
        QuickItem.SetStaff(this, 48, 48, ModContent.ProjectileType<PropellerpadProjectile>(), 9, 0, 24, 0, 0, ItemRarityID.Green);

        Item.accessory = true;
        Item.buffType = ModContent.BuffType<PropellerpadBuff>();
        Item.buffTime = 2;
    }

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<PropellerpadProjectile>()] == 0;

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (!hideVisual)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PropellerpadProjectile>()] == 0)
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.One, ModContent.ProjectileType<PropellerpadProjectile>(), 0, 0, player.whoAmI);

            player.AddBuff(ModContent.BuffType<PropellerpadBuff>(), 2);
        }
        else if (player.HasBuff<PropellerpadBuff>())
            player.ClearBuff(ModContent.BuffType<PropellerpadBuff>());
    }
}
