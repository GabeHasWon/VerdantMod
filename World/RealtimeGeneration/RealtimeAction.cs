using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Verdant.World.RealtimeGeneration
{
    internal class RealtimeAction
    {
        readonly int TickRate = 0;
        readonly Queue<(Point, TileAction.TileActionDelegate)> TileActions = new();

        private int _timer = 0;

        public RealtimeAction(Queue<(Point, TileAction.TileActionDelegate)> tileActions, int tickRate)
        {
            TileActions = tileActions;
            TickRate = tickRate;
        }

        public void Play()
        {
            _timer++;

            if (_timer == TickRate && TileActions.Count > 0)
            {
                (var position, var action) = TileActions.Dequeue();
                action.Invoke(position.X, position.Y);
                _timer = 0;
            }
        }
    }
}
