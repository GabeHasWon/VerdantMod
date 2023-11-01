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
    public readonly bool glassless;

    public StartMicrocosmModule(Point16 pos, short myPlayer, bool glassless)
    {
        x = pos.X;
        y = pos.Y;
        fromWho = myPlayer;
        this.glassless = glassless;
    }

    protected override void Receive()
    {
        if (Main.netMode == NetmodeID.Server) //Spawn on server
            Microcosm.SpawnMicrocosm(new(x, y), glassless);
    }
}
