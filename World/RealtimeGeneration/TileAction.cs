using Terraria;
using Terraria.ObjectData;

namespace Verdant.World.RealtimeGeneration
{
    internal class TileAction
    {
        public delegate void TileActionDelegate(int x, int y);

        public static TileActionDelegate PlaceTileAction(int type, bool multitile = false, bool force = true, bool mute = true)
        {
            return (x, y) =>
            {
                if (!multitile)
                {
                    if (force)
                        WorldGen.KillTile(x, y, false, false, true);
                    WorldGen.PlaceTile(x, y, type, mute);
                }
                else
                {
                    if (force)
                    {
                        TileObjectData data = TileObjectData.GetTileData(type, 0);

                        if (data is not null)
                        {
                            x -= data.Width;
                            y -= data.Height;

                            for (int i = x; i < x + data.Width; ++i)
                                for (int j = y; j < y + data.Height; ++j)
                                    WorldGen.KillTile(x, y, false, false, true);
                        }
                    }

                    WorldGen.PlaceObject(x, y, type, mute);
                }
            };
        }

        public static TileActionDelegate KillWall(bool fail = false) => (x, y) => WorldGen.KillWall(x, y, fail);
        public static TileActionDelegate PlaceWall(int type, bool mute = true) => (x, y) => WorldGen.PlaceWall(x, y, type, mute);
    }
}
