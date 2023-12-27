using Microsoft.Xna.Framework;
using NetEasy;
using rail;
using System;
using Terraria;
using Terraria.ID;
using Verdant.Items.Verdant.Tools;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using static Humanizer.In;

namespace Verdant.Systems.Syncing.Foreground;

[Serializable]
public class ZipvineModule : Module
{
    public readonly float x;
    public readonly float y;
    public readonly short? slotInList;
    public readonly short fromWho = 0;
    public readonly byte length;
    public readonly byte toPlayer;
    public readonly byte vineId = 0;

    public ZipvineModule(float x, float y, short? slotInList, byte length, short myPlayer, byte toPlayer = Main.maxPlayers, byte vineId = 0)
    {
        this.x = x;
        this.y = y;
        this.slotInList = slotInList;
        this.length = length;
        this.toPlayer = toPlayer;
        this.vineId = vineId;

        fromWho = myPlayer;
    }

    protected override void Receive()
    {
        if (Main.netMode != NetmodeID.Server && Main.myPlayer != fromWho) //Spawn on client
        {
            var vine = slotInList is null ? null : ForegroundManager.PlayerLayerItems[slotInList.Value] as ZipvineEntity;

            if (vineId == 0)
                VineWandCommon.BuildVine(length, vine, new Vector2(x, y), true);
            else
                VineWandCommon.BuildChlorovine(length, vine as ChlorovineEntity, new Vector2(x, y), true);
        }
        else if (Main.netMode == NetmodeID.Server) //Send to other clients
        {
            Send(toPlayer == Main.maxPlayers ? -1 : toPlayer, toPlayer == Main.maxPlayers ? fromWho : Main.maxPlayers, false);

            var vine = slotInList is null ? null : ForegroundManager.PlayerLayerItems[slotInList.Value] as ZipvineEntity;

            if (vineId == 0)
                VineWandCommon.BuildVine(length, vine, new Vector2(x, y), true);
            else
                VineWandCommon.BuildChlorovine(length, vine as ChlorovineEntity, new Vector2(x, y), true);
        }
    }
}
