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
public class SyncZipvinesModule : Module
{
    public readonly short fromWho = 0;

    public SyncZipvinesModule(short myPlayer)
    {
        fromWho = myPlayer;
    }

    protected override void Receive()
    {
        if (Main.netMode == NetmodeID.Server) //Play on server
        {
            foreach (var item in ForegroundManager.PlayerLayerItems.Where(x => x is ZipvineEntity))
            {
                ZipvineEntity zip = item as ZipvineEntity;
                short? index = zip.priorVine is null ? null : (short)ForegroundManager.PlayerLayerItems.IndexOf(zip.priorVine);
                new ZipvineModule(zip.position.X, zip.position.Y, index, zip.VineLength, -1, (byte)fromWho).Send(fromWho, -1, false);
            } 
        }
    }
}
