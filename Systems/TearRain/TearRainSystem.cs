using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Verdant.Systems.Syncing;

namespace Verdant.Systems.TearRain;

internal class TearRainSystem : ModSystem
{
    public static bool Raining = true;
    public static float RainStrength = 0f;
    public static float RainStrengthTarget = 0f;
    public static int DaysSinceArtificialRain = 0;

    /// <summary>
    /// Determines if the given <paramref name="y"/> coordinate is 'underground', and thus can have tear rain.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanRain(int y) => y > Main.worldSurface && y < Main.maxTilesY - 201;

    /// <summary>
    /// Determines if the Y coordinate is underground and there is tear rain underground. 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool RainingAt(int y) => CanRain(y) && Raining;

    /// <summary>
    /// Determines if the Y coordinate is either underground and it's tear raining, or if it's aboveground and it's normal raining.
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AnyRainingAt(int y) => RainingAt(y) || (y < Main.worldSurface && Main.raining);

    /// <summary>
    /// Has a <paramref name="randomStartChance"/> to start rain with either a random 0-1 (if <paramref name="randomStrength"/> == -1) or <paramref name="randomStrength"/> strength.
    /// </summary>
    internal static void StartRain(float randomStartChance = 1f, float randomStrength = -1, bool isArtificial = false)
    {
        Raining = Main.rand.NextFloat() < randomStartChance;
        RainStrengthTarget = randomStrength == -1 ? Main.rand.NextFloat(0f, 1f) : randomStrength;

        if (isArtificial)
            DaysSinceArtificialRain = 0;

        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.WorldData);
        else if (Main.netMode == NetmodeID.MultiplayerClient)
            new SyncTearRainModule(Raining, RainStrengthTarget, isArtificial ? 0 : -1).Send();
    }

    public override void Load() => On_Main.UpdateTime_StartDay += StartDayAddRain;

    private void StartDayAddRain(On_Main.orig_UpdateTime_StartDay orig, ref bool stopEvents)
    {
        orig(ref stopEvents);

        DaysSinceArtificialRain++;

        if (!Raining)
            RainStrength = 0;

        StartRain(1 / 6f);
    }

    public override void PostUpdateWorld() => RainStrength = MathHelper.Lerp(RainStrength, RainStrengthTarget, 0.003f);

    public override void SaveWorldData(TagCompound tag)
    {
        tag.Add("raining", Raining);
        tag.Add("strength", RainStrength);
        tag.Add("target", RainStrengthTarget);
        tag.Add("artificial", DaysSinceArtificialRain);
    }

    public override void LoadWorldData(TagCompound tag)
    {
        Raining = tag.GetBool("raining");
        RainStrength = tag.GetFloat("strength");
        RainStrengthTarget = tag.GetFloat("target");
        DaysSinceArtificialRain = tag.GetInt("artificial");
    }

    public override void NetSend(BinaryWriter writer)
    {
        writer.Write(Raining);
        writer.Write((Half)RainStrengthTarget);
        writer.Write((short)DaysSinceArtificialRain);
    }

    public override void NetReceive(BinaryReader reader)
    {
        Raining = reader.ReadBoolean();
        RainStrengthTarget = (float)reader.ReadHalf();
        DaysSinceArtificialRain = reader.ReadInt16();
    }
}
