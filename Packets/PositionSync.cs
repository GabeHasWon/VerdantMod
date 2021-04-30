using Microsoft.Xna.Framework;
using NetEasy;
using System;
using Terraria;

namespace Verdant.Packets
{
    [Serializable]
    public class PositionSync : Module
    {
        public PositionSync(int fromNpc, Vector2 syncPos)
        {
            this.fromNpc = fromNpc;
            this.syncPos = syncPos;
        }

        private readonly int fromNpc;
        private readonly Vector2 syncPos;

        protected override void Receive()
        {
            if (Main.netMode == Terraria.ID.NetmodeID.Server)
            {
                Send(-1, -1, true);
                return;
            }

            NPC npc = Main.npc[fromNpc];
            npc.position = syncPos;
        }
    }
}