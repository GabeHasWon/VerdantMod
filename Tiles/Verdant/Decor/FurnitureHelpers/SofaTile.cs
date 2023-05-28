using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor;

internal abstract class SofaTile<T> : TileBlueprint<T> where T : ModItem
{
    protected sealed override StaticTileInfo StaticInfo => new("ItemName.Sofa", TileID.Benches);

    public sealed override void Defaults()
    {
        Main.tileSolidTop[Type] = false;
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileID.Sets.HasOutlines[Type] = true;
        TileID.Sets.CanBeSatOnForNPCs[Type] = true;
        TileID.Sets.CanBeSatOnForPlayers[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
        TileObjectData.addTile(Type);

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<T>());

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => FurnitureHelper.ChairInteract(i, j, settings);
    public override bool RightClick(int i, int j) => FurnitureHelper.RightClick(i, j);
    public override void MouseOver(int i, int j) => FurnitureHelper.MouseOver(i, j, ModContent.ItemType<T>());

    public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
    {
        info.TargetDirection = info.RestingEntity.direction;
        info.AnchorTilePosition.X = i;
        info.AnchorTilePosition.Y = j;
        info.DirectionOffset = info.RestingEntity is Player ? 0 : 2; // Default to 6 for players, 2 for NPCs
    }
}
