using System;
using Terraria.ID;
using Terraria;
using Verdant.Systems.Foreground.Tiled;
using Verdant.Systems.Foreground;
using NetEasy;
using Microsoft.Xna.Framework;

namespace Verdant.Systems.Syncing.Foreground;

[Serializable]
public class DrapesModule(byte myPlayer, int x, int y, bool grow, short length = 1) : Module
{
    public enum Data
    {
        None,
        Kill,
        PlacePuff
    }

    /// <summary>
    /// Property to make code more readable for the case where this packet is for growing.
    /// </summary>
    private int WhoAmI => X;

    public readonly byte FromWho = myPlayer;
    public readonly int X = x;
    public readonly int Y = y;
    public readonly bool Grow = grow;
    public readonly short Length = length;

    protected override void Receive()
    {
        VerdantMod.DebugLogMessage(GetType());

        if (Main.myPlayer == FromWho)
            return;

        if (!Grow)
        {
            var drapes = new MysteriaDrapes(new Point(X, Y));

            if (Length > 1)
                for (int i = 1; i < Length; ++i)
                    drapes.Grow();

            ForegroundManager.AddItem(drapes, true);
        }
        else
            (ForegroundManager.Items[WhoAmI] as MysteriaDrapes).Grow();

        if (Main.netMode == NetmodeID.Server)
            Send(-1, FromWho, false);
    }
}