using NetEasy;
using System;
using Terraria;
using Terraria.ID;
using Verdant.Systems.TearRain;

namespace Verdant.Systems.Syncing;

[Serializable]
public class SyncTearRainModule(bool isRaining, float targetStrength, int daysSinceArtificial = -1) : Module
{
    public readonly bool IsRaining = isRaining;
    public readonly float TargetStrength = targetStrength;
    public readonly int DaysSinceArtifiical = daysSinceArtificial;

    protected override void Receive()
    {
        VerdantMod.DebugLogMessage(GetType());

        if (Main.netMode == NetmodeID.Server)
        {
            TearRainSystem.Raining = IsRaining;
            TearRainSystem.RainStrengthTarget = TargetStrength;

            if (DaysSinceArtifiical != -1)
                TearRainSystem.DaysSinceArtificialRain = DaysSinceArtifiical;

            NetMessage.SendData(MessageID.WorldData);
        }
    }
}