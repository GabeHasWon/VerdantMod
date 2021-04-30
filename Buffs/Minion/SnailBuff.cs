using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Buffs.Minion
{
    public class SnailBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Snail Minion");
            Description.SetDefault("The grassy snail will fight for you");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 10;
        }
    }
}