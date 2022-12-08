using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace Verdant.Systems.RealtimeGeneration.Old
{
    [Obsolete("Does not function perfectly; implementation has been moved to UndoableStep & RealtimeAction.ReplaceStructure.")]
    internal class ReplacementStep : RealtimeStep
    {
        public static Dictionary<string, HashSet<TileState>> SavedTiles = new();

        private readonly string _name = string.Empty;

        public ReplacementStep(Point16 pos, TileAction.TileActionDelegate action, string name) : base(pos, action)
        {
            _name = name;
        }

        public override void Invoke(int x, int y, ref bool success)
        {
            TileState newState = Main.tile[x, y].GetState(x, y, _name);

            base.Invoke(x, y, ref success);

            if (success)
                TryAdd(newState);

        }

        private void TryRemove(TileState newState)
        {
            SavedTiles[_name].Remove(newState);

            if (SavedTiles[_name].Count == 0)
                SavedTiles.Remove(_name);
        }

        private void TryAdd(TileState newState)
        {
            if (!SavedTiles.ContainsKey(_name))
                SavedTiles.Add(_name, new());
            SavedTiles[_name].Add(newState);
        }

        public static RealtimeAction GenerateReplacement(string name, float tickRate)
        {
            Queue<RealtimeStep> queue = new();
            List<RealtimeStep> steps = new List<RealtimeStep>();

            var set = SavedTiles[name].Reverse().ToList();
            var removeSet = new List<TileState>();

            foreach (var item in set)
            {
                TileObjectData data = TileObjectData.GetTileData(item.TileType, 0, 0);
                bool oneByOne = data is null || data.Width == 1 && data.Height == 1;

                queue.Enqueue(new RealtimeStep(item.Position, TileAction.FullReplace(item, oneByOne, false)));
            }

            foreach (var item in removeSet)
                set.Remove(item);

            foreach (var item in set)
                queue.Enqueue(new RealtimeStep(item.Position, TileAction.Reframe(item, false)));

            SavedTiles.Remove(name);

            //foreach (var item in steps)
            //    queue.Enqueue(item);

            return new RealtimeAction(queue, tickRate);
        }

        private static RealtimeStep GetUncertainReplacement(TileState item)
        {
            TileAction.TileActionDelegate action = TileAction.PlaceTile(item.TileType, true, true, true);

            if (item.TileType == TileID.Trees || TileID.Sets.IsATreeTrunk[item.TileType])
                action = TileAction.FullReplace(item, true);

            return new RealtimeStep(item.Position, action);
        }
    }
}
