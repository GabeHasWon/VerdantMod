﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Players;

namespace Verdant.Items.Verdant.Equipables;

class Mudsquid : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Mudsquid");
        Tooltip.SetDefault("Allows quick travel directly through some solid tiles,\nincluding all natural Verdant tiles");
    }

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 3);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<MudsquidPlayer>().hasSquid = true;
}