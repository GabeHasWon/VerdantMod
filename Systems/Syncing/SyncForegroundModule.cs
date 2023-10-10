using NetEasy;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;

namespace Verdant.Systems.Syncing;

/// <summary>
/// Purpose-made 
/// </summary>
[Serializable]
public class SyncForegroundModule : Module
{
    public readonly short fromWho = 0;

    public SyncForegroundModule(short myPlayer)
    {
        fromWho = myPlayer;
    }

    protected override void Receive()
    {
        if (Main.netMode == NetmodeID.Server) //Play on server
        {
            foreach (var item in ForegroundManager.PlayerLayerItems)
            {
                if (item is ZipvineEntity zip)
                {
                    short? index = zip.priorVine is null ? null : (short)ForegroundManager.PlayerLayerItems.IndexOf(zip.priorVine);
                    new ZipvineModule(zip.position.X, zip.position.Y, index, zip.VineLength, -1, (byte)fromWho).Send(fromWho, -1, false);
                }
                else if (item is CloudbloomEntity cloud)
                    new CloudbloomModule(Main.maxPlayers, cloud.position.X, cloud.position.Y, cloud.puff).Send(fromWho, -1, false);
            } 
        }
    }
}
