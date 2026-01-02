using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ModLoader;

namespace Verdant.Systems.TearRain;

internal class TearRainSystem : ModSystem
{
    public static bool Raining = true;
    public static float RainStrength = 0f;
    public static float RainStrengthTarget = 0f;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanRain(int y) => y > Main.worldSurface && y < Main.maxTilesY - 201;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool RainingAt(int y) => y > Main.worldSurface && y < Main.maxTilesY - 201 && Raining;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AnyRainingAt(int y) => y > Main.worldSurface && y < Main.maxTilesY - 201 && Raining || y < Main.worldSurface && Main.raining;

    public override void Load()
    {
        On_Main.UpdateTime_StartDay += StartDayAddRain;
    }

    private void StartDayAddRain(On_Main.orig_UpdateTime_StartDay orig, ref bool stopEvents)
    {
        orig(ref stopEvents);

        if (!Raining)
            RainStrength = 0;

        Raining = Main.rand.NextBool(6);
        RainStrengthTarget = Main.rand.NextFloat(0f, 1f);
    }

    public override void PostUpdateWorld()
    {
        RainStrength = MathHelper.Lerp(RainStrength, RainStrengthTarget, 0.002f);
    }
}
