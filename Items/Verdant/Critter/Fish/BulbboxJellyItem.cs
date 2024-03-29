﻿using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter.Fish;

[Sacrifice(3)]
class BulbboxJellyItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 22, 20, ModContent.NPCType<NPCs.Passive.Fish.BulbboxJelly>(), 1, 20, Item.buyPrice(0, 0, 20));
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
