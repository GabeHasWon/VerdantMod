using NetEasy;
using System;
using Terraria;
using Terraria.ID;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Systems.Foreground;
using System.Linq;

namespace Verdant.Systems.Syncing.Foreground;

[Serializable]
public class KillZipvineModule(short myPlayer, short slot) : Module
{
    public readonly short fromWho = myPlayer;
    public readonly int slot = slot;

    protected override void Receive()
    {
        VerdantMod.DebugLogMessage(GetType());

        if (Main.netMode == NetmodeID.Server) //Play on server
            Send(-1, fromWho, false);

        var item = ForegroundManager.PlayerLayerItems.FirstOrDefault(x => x is ZipvineEntity zip && ForegroundManager.PlayerLayerItems.IndexOf(zip) == slot);

        if (item is ZipvineEntity zip)
            zip.Kill();
    }
}
