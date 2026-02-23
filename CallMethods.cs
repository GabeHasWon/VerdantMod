using Microsoft.Xna.Framework;
using System;
using Terraria;
using Verdant.Systems.TearRain;
using Verdant.World;

namespace Verdant;

internal class CallMethods
{
    internal static bool InVerdant(object[] args)
    {
        if (args.Length == 1)
            return VerdantSystem.InVerdant;
        else
        {
            if (args[1] is Player player)
                return player.GetModPlayer<VerdantPlayer>().ZoneVerdant;

            throw new ArgumentException($"[Verdant] Second argument of {nameof(InVerdant)} must be a Player!");
        }
    }

    internal static bool NearApotheosis(object[] args)
    {
        if (args.Length == 1)
            return VerdantSystem.NearApotheosis;
        else
        {
            if (args[1] is Player player)
                return player.GetModPlayer<VerdantPlayer>().ZoneApotheosis;

            throw new ArgumentException($"[Verdant] Second argument of {nameof(NearApotheosis)} must be a Player!");
        }
    }

    internal static void SetVerdantArea(object[] args)
    {
        if (args.Length == 2)
        {
            if (args[1] is Rectangle rectangle)
                VerdantGenSystem.VerdantArea = rectangle;
            else
                throw new ArgumentException($"[Verdant] First argument of {nameof(SetVerdantArea)} must be a Rectangle!");
        }
        else if (args.Length == 3)
        {
            if (args[1] is Point position && args[2] is Point size)
                VerdantGenSystem.VerdantArea = new Rectangle(position.X, position.Y, size.X, size.Y);
            else
                throw new ArgumentException($"[Verdant] First and second arguments of {nameof(SetVerdantArea)} must be Points!");
        }
        else if (args.Length == 5)
        {
            if (args[1] is int x && args[2] is int y && args[3] is int width && args[4] is int height)
                VerdantGenSystem.VerdantArea = new Rectangle(x, y, width, height);
            else
                throw new ArgumentException($"[Verdant] First, second, third and fourth arguments of {nameof(SetVerdantArea)} must be ints!");
        }
        throw new ArgumentException($"[Verdant] {nameof(SetVerdantArea)} call matched no valid override!");
    }

    internal static bool AnyRainAt(object[] args)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("[Verdant] AnyRainAt takes exactly one parameter: int y");
        }

        if (args[0] is not IConvertible)
        {
            throw new ArgumentException("[Verdant] AnyRainAt parameter must be an int or convertible to int!");
        }

        int y = Convert.ToInt32(args[0]);
        return TearRainSystem.AnyRainingAt(y);
    }

    internal static bool TearRainAt(object[] args)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("[Verdant] TearRainAt takes exactly one parameter: int y");
        }

        if (args[0] is not IConvertible)
        {
            throw new ArgumentException("[Verdant] TearRainAt parameter must be an int or convertible to int!");
        }

        int y = Convert.ToInt32(args[0]);
        return TearRainSystem.RainingAt(y);
    }
}
