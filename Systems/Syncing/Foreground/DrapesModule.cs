using System;
using Terraria.ID;
using Terraria;
using Verdant.Systems.Foreground.Tiled;
using Verdant.Systems.Foreground;
using NetEasy;
using Microsoft.Xna.Framework;

namespace Verdant.Systems.Syncing.Foreground;

[Serializable]
public class DrapesModule : Module
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

    public readonly byte FromWho = 0;
    public readonly int X;
    public readonly int Y;
    public readonly bool Grow;
    public readonly short Length;

    public DrapesModule(byte myPlayer, int x, int y, bool grow, short length = 1)
    {
        FromWho = myPlayer;
        X = x;
        Y = y;
        Grow = grow;
        Length = length;
    }

    protected override void Receive()
    {
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