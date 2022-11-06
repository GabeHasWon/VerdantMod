using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Verdant.Systems.RealtimeGeneration
{
    internal class RealtimeAction
    {
        readonly float TickRate = 0;
        readonly Queue<RealtimeStep> TileActions = new();

        private float _timer = 0;
        private float _surpassedValues = 0;

        public RealtimeAction(Queue<RealtimeStep> tileActions, float tickRate)
        {
            TileActions = tileActions;
            TickRate = tickRate;
        }

        public void Play()
        {
            _surpassedValues = (float)Math.Floor(_timer);
            _timer += TickRate;

            if (TileActions.Count <= 0)
                return;

            int repeats = (int)(Math.Floor(_timer) - _surpassedValues);
            for (int i = 0; i < repeats; ++i)
            {
                if (TileActions.Count <= 0)
                    return;

                var step = TileActions.Dequeue();
                bool success = false;
                step.Invoke(step.Position.X, step.Position.Y, ref success);
                
                if (!success)
                    i--;

                _timer = 0;
            }
        }
    }
}
