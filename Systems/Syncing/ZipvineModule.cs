using Microsoft.Xna.Framework;
using NetEasy;
using System;
using Terraria;
using Terraria.ID;
using Verdant.Items.Verdant.Tools;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;

namespace Verdant.Systems.Syncing;

[Serializable]
public class ZipvineModule : Module
{
    public readonly float x;
    public readonly float y;
    public readonly short? slotInList;
    public readonly short fromWho = 0;

    public ZipvineModule(float x, float y, short? slotInList, short myPlayer)
    {
        this.x = x;
        this.y = y;
        this.slotInList = slotInList;
        fromWho = myPlayer;
    }

    protected override void Receive()
    {
        if (Main.netMode != NetmodeID.Server) //Spawn on client
        {
            var vine = slotInList is null ? null : ForegroundManager.PlayerLayerItems[slotInList.Value] as ZipvineEntity;
            VineWandCommon.BuildVine(fromWho, vine, new Vector2(x, y), true);
        }
        else if (fromWho != -1) //Send to other clients
            Send(-1, fromWho, false);
    }
}
