using NetEasy;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Systems.Foreground;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Verdant.Systems.Syncing.Foreground;

[Serializable]
public class CloudbloomModule : Module
{
    public enum Data
    {
        None,
        Kill,
        PlacePuff
    }

    public readonly byte fromWho = 0;
    public readonly float X;
    public readonly float Y;
    public readonly Data data;

    /// <summary>
    /// Places a Cloudbloom entity.
    /// </summary>
    /// <param name="myPlayer">Which player sent the packet.</param>
    /// <param name="x">X position of the bloom.</param>
    /// <param name="y">Y position of the bloom.</param>
    /// <param name="puff">Whether the new Cloudbloom is or isn't a puff.</param>
    public CloudbloomModule(byte myPlayer, float x, float y, bool puff)
    {
        fromWho = myPlayer;
        X = x;
        Y = y;
        data = puff ? Data.PlacePuff : Data.None;
    }

    /// <summary>
    /// Kills a Cloudbloom entity.
    /// </summary>
    /// <param name="myPlayer">Which player sent the packet.</param>
    /// <param name="killIndex">Which <see cref="ForegroundManager.PlayerLayerItems"/> slot to kill.</param>
    public CloudbloomModule(byte myPlayer, int killIndex)
    {
        fromWho = myPlayer;
        X = killIndex;
        data = Data.Kill;
    }

    protected override void Receive()
    {
        if (Main.myPlayer == fromWho)
            return;

        if (data != Data.Kill)
            ForegroundManager.AddItem(new CloudbloomEntity(new Vector2(X, Y), data == Data.PlacePuff), true, true);
        else
            ForegroundManager.PlayerLayerItems.ElementAt((int)X).killMe = true;

        if (Main.netMode == NetmodeID.Server)
            Send(-1, fromWho, false);
    }
}
