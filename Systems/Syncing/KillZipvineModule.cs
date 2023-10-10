using NetEasy;
using System;
using Terraria;
using Terraria.ID;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Systems.Foreground;
using System.Linq;

namespace Verdant.Systems.Syncing;

[Serializable]
public class KillZipvineModule : Module
{
    public readonly short fromWho = 0;
    public readonly int slot = 0;

    public KillZipvineModule(short myPlayer, short slot)
    {
        fromWho = myPlayer;
        this.slot = slot;
    }

    protected override void Receive()
    {
        if (Main.netMode == NetmodeID.Server) //Play on server
            Send(-1, fromWho, false);

        var item = ForegroundManager.PlayerLayerItems.FirstOrDefault(x => x is ZipvineEntity zip && ForegroundManager.PlayerLayerItems.IndexOf(zip) == slot);

        if (item is ZipvineEntity zip)
            zip.Kill();
    }
}
