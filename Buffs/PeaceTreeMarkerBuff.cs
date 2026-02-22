using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Buffs;

internal class PeaceTreeMarkerBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
        Main.buffNoSave[Type] = true;

        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }
}
