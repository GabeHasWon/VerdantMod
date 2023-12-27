using NetEasy;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Systems.Foreground.Tiled;

namespace Verdant.Systems.Syncing.Foreground;

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

            foreach (var item in ForegroundManager.Items)
            {
                if (item is MysteriaDrapes drapes)
                    new DrapesModule(Main.maxPlayers, (int)(drapes.position.X / 16f), (int)(drapes.position.Y / 16f), false, drapes.length).Send(fromWho, -1, false);
            }
        }
    }
}
