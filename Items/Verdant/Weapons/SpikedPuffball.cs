﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Throwing;

namespace Verdant.Items.Verdant.Weapons
{
    internal class SpikedPuffball : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiked Puffball");
            Tooltip.SetDefault("Floats in the air for half a minute, then drops");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.ThrowingKnife);
            Item.damage = 12;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SpikedPuffballProj>();
            Item.shootSpeed = 6f;
        }
    }
}