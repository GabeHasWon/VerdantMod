using Terraria;
using Terraria.ObjectData;

namespace Verdant.Systems.RealtimeGeneration
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

                    success = true;
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
                    success = true;
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

        public static TileActionDelegate FullReplace(TileState state, bool reframe = false, bool skipMe = false) => (int x, int y, ref bool success) =>
        {
            Tile tile = Main.tile[x, y];
            tile.HasTile = state.Active;

            if (state.Active)
            {
                tile.TileType = state.TileType;
                tile.TileFrameX = state.FrameX;
                tile.TileFrameY = state.FrameY;
                tile.WallFrameX = state.WallFrameX;
                tile.WallFrameY = state.WallFrameY;
                tile.LiquidType = state.LiquidType;
                tile.LiquidAmount = state.LiquidAmount;
                tile.Slope = state.Slope;
            }

            if (reframe)
                WorldGen.TileFrame(x, y, true, true);

            success = !skipMe;
        };

        public static TileActionDelegate Reframe(TileState state, bool justReframe = false, bool skipMe = false) => (int x, int y, ref bool success) =>
        {
            if (!justReframe)
            {
                Tile tile = Main.tile[x, y];

                if (tile.HasTile)
                {
                    tile.TileFrameX = state.FrameX;
                    tile.TileFrameY = state.FrameY;
                    tile.WallFrameX = state.WallFrameX;
                    tile.WallFrameY = state.WallFrameY;
                }
            }

            WorldGen.TileFrame(x, y, true, true);
            success = !skipMe;
        };
    }
}
