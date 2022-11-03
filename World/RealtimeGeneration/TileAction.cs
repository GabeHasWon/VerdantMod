using Terraria;
using Terraria.ObjectData;

namespace Verdant.World.RealtimeGeneration
{
    internal class TileAction
    {
        public delegate void TileActionDelegate(int x, int y, ref bool success);

        public static TileActionDelegate PlaceTile(int type, bool multitile = false, bool force = true, bool mute = true)
        {
            return (int x, int y, ref bool success) =>
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

        public static TileActionDelegate KillTile(bool fail = false, bool noItem = true) => (int x, int y, ref bool success) =>
        {
            WorldGen.KillTile(x, y, fail, false, noItem);
            success = true;
        };

        public static TileActionDelegate KillWall(bool fail = false) => (int x, int y, ref bool success) =>
        {
            WorldGen.KillWall(x, y, fail);
            success = true;
        };

        public static TileActionDelegate PlaceWall(int type, bool mute = true, bool force = false) => (int x, int y, ref bool success) =>
        {
            if (force)
                WorldGen.KillWall(x, y, false);
            WorldGen.PlaceWall(x, y, type, mute);
            success = true;
        };
    }
}
