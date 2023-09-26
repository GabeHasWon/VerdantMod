using Microsoft.Xna.Framework;
using NetEasy;
using System;
using Terraria;
using Terraria.ID;
using Verdant.Items.Verdant.Armour.ApotheoticArmor;
using Verdant.Items.Verdant.Tools;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;

namespace Verdant.Systems.Syncing;

[Serializable]
public class SyncTreebandModule : Module
{
    public readonly FruitType[] fruits;
    public readonly int fruitTimer;
    public readonly short fromWho = 0;

    public SyncTreebandModule(FruitType[] fruits, int fruitTimer, short myPlayer)
    {
        this.fruits = fruits;
        this.fruitTimer = fruitTimer;
        fromWho = myPlayer;
    }

    protected override void Receive()
    {
        if (Main.netMode != NetmodeID.Server) //Set on client
        {
            if (fromWho < 0 || fromWho >= Main.maxPlayers)
                return;

            Main.player[fromWho].GetModPlayer<TreeHelmetPlayer>().fruits = fruits;
            Main.player[fromWho].GetModPlayer<TreeHelmetPlayer>().fruitTimer = fruitTimer;
        }
        else if (fromWho != -1) //Send to other clients
        {
            if (fromWho < 0 || fromWho >= Main.maxPlayers)
                return;

            Send(-1, fromWho, false);
        }
    }
}
