using NetEasy;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Misc;

namespace Verdant.Systems.Syncing;

[Serializable]
public class StartMicrocosmModule : Module
{
    public readonly int x;
    public readonly int y;
    public readonly short fromWho = 0;

    public StartMicrocosmModule(Point16 pos, short myPlayer)
    {
        x = pos.X;
        y = pos.Y;
        fromWho = myPlayer;
    }

    protected override void Receive()
    {
        if (Main.netMode != NetmodeID.Server) //Spawn on client
            Microcosm.SpawnMicrocosm(new(x, y));
        else if (fromWho != -1) //Spawn on server
        {
            Microcosm.SpawnMicrocosm(new(x, y));
            Send(-1, fromWho, false);
        }
    }
}
