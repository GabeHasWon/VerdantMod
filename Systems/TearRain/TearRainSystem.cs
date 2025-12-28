using Terraria;
using Terraria.ModLoader;

namespace Verdant.Systems.TearRain;

internal class TearRainSystem : ModSystem
{
    internal bool TearRain = false;

    public override void Load()
    {
        On_Main.UpdateTime_StartDay += StartDayAddRain;
    }

    private void StartDayAddRain(On_Main.orig_UpdateTime_StartDay orig, ref bool stopEvents)
    {
        orig(ref stopEvents);

        TearRain = Main.rand.NextBool(8);
    }
}
