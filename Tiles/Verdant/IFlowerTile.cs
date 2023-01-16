using Microsoft.Xna.Framework;
using Verdant.NPCs.Passive;

namespace Verdant.Tiles.Verdant
{
    internal interface IFlowerTile
    {
        public Vector2[] GetOffsets();
        public bool IsFlower(int i, int j);
        public Vector2[] OffsetAt(int i, int j);

        /// <summary>Runs when pollinated by a <see cref="Bumblebee"/>. Return false if nothing happened. Returns false by default.</summary>
        public bool OnPollenate(int i, int j) => false;
    }
}
