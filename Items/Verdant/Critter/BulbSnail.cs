﻿using Terraria;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Critter;

[Sacrifice(5)]
class BulbSnail : ModItem
{
    public override void SetDefaults() => QuickItem.SetCritter(this, 34, 34, ModContent.NPCType<NPCs.Passive.VerdantBulbSnail>(), 1, 15);
    public override bool CanUseItem(Player player) => QuickItem.CanCritterSpawnCheck();
}
