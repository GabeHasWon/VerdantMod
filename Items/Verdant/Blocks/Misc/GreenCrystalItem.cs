﻿using Terraria;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items.Verdant.Blocks.Misc;

[Sacrifice(100)]
public class GreenCrystalItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetBlock(this, 14, 18, ModContent.TileType<GreenCrystal>());

	public override bool? UseItem(Player player)
	{
		Item.placeStyle = Main.rand.Next(3);
		return null;
	}
}
