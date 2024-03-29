﻿using Terraria;
using Terraria.ModLoader;

namespace Verdant.Buffs.Minion;

public class PropellerpadBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) => player.buffTime[buffIndex] = 2;
}