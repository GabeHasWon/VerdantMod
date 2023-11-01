using Terraria;
using Terraria.ModLoader;
using Verdant.Projectiles.Minion;

namespace Verdant.Buffs.Minion
{
    public class HealingFlowerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<VerdantHealingMinion>()] <= 0)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
                return;
            }

            player.buffTime[buffIndex] = 18000;
        }
    }
}