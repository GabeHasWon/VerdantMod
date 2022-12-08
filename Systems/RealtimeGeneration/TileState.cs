using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Verdant.Systems.RealtimeGeneration
{
    public readonly struct TileState
    {
        public readonly Point16 Position;
        public readonly bool Active;
        public readonly ushort TileType;
        public readonly short FrameX;
        public readonly short FrameY;
        public readonly short WallFrameX;
        public readonly short WallFrameY;
        public readonly ushort Wall;
        public readonly byte LiquidType;
        public readonly byte LiquidAmount;
        public readonly string FromAction;
        public readonly SlopeType Slope;

        public TileState(Point16 position, bool active, ushort type, short frameX, short frameY, ushort wall, short wallFrameX, short wallFrameY, byte liquidType, byte liquidAmount, string from, SlopeType slope)
        {
            Position = position;
            Active = active;
            TileType = type;
            FrameX = frameX;
            FrameY = frameY;
            Wall = wall;
            WallFrameX = wallFrameX;
            WallFrameY = wallFrameY;
            LiquidType = liquidType;
            LiquidAmount = liquidAmount;
            FromAction = from;
            Slope = slope;
        }

        public void SaveAs(TagCompound tag)
        {
            TagCompound tileState = new()
            {
                { "pos", Position },
                { "active", Active },
                { "type", TileType },
                { "frameX", FrameX },
                { "frameY", FrameY },
                { "wall", Wall },
                { "wallX", WallFrameX },
                { "wallY", WallFrameY },
                { "liquidType", LiquidType },
                { "liquidAmount", LiquidAmount },
                { "from", FromAction },
                { "slope", (byte)Slope }
            };

            tag.Add($"SavedTile{Position.GetHashCode()}", tileState);
        }

        public static TileState Load(TagCompound tag)
        {
            TileState tileState = new(
                tag.Get<Point16>("pos"),
                tag.GetBool("active"),
                (ushort)tag.GetShort("type"),
                tag.GetShort("frameX"),
                tag.GetShort("frameY"),
                (ushort)tag.GetShort("wall"),
                tag.GetShort("wallX"),
                tag.GetShort("wallY"),
                tag.GetByte("liquidType"),
                tag.GetByte("liquidAmount"),
                tag.GetString("from"),
                (SlopeType)tag.GetByte("slope")
            );

            return tileState;
        }
    }
}
