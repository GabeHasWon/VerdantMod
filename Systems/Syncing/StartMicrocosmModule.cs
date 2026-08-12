using NetEasy;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Verdant.Items.Verdant.Misc;

namespace Verdant.Systems.Syncing;

[Serializable]
public class StartMicrocosmModule(Point16 pos, short myPlayer, bool glassless) : Module
{
    public readonly int x = pos.X;
    public readonly int y = pos.Y;
    public readonly short fromWho = myPlayer;
    public readonly bool glassless = glassless;

    protected override void Receive()
    {
        VerdantMod.DebugLogMessage(GetType());

        if (Main.netMode == NetmodeID.Server) //Spawn on server
            Microcosm.SpawnMicrocosm(new(x, y), glassless);
    }
}
