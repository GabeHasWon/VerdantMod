using Microsoft.Xna.Framework;

namespace Verdant.Tiles.Verdant
{
    internal interface IFlowerTile
    {
        public Vector2[] GetOffsets();
        public bool IsFlower(int i, int j);
        public Vector2[] OffsetAt(int i, int j);
    }
}
