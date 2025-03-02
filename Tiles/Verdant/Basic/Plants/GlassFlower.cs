using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants;

class GlassFlower : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<LushSoil>()];
        TileObjectData.newTile.ExpandValidAnchors([.. VerdantGrassLeaves.VerdantGrassTypes]);
        TileObjectData.newTile.RandomStyleRange = 1;
        TileObjectData.newTile.StyleHorizontal = true;

        QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Shatter, true, new Color(143, 21, 193));
    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (!closer)
            GlassFlowerSystem.LocallyNearGlassFlower = true;
    }

    public Vector2[] GetOffsets() => [new Vector2(16, 13)];
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}

[Sacrifice(3)]
public class GlassFlowerItem : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BlueDyeBulb>()] = Type;
        ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PinkDyeBulb>()] = Type;
        ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<WhiteDyeBulb>()] = Type;
        ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<RedDyeBulb>()] = Type;
    }

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<GlassFlower>());
        Item.rare = ItemRarityID.Green;
    }
}

public class GlassFlowerSystem : ModSystem
{
    internal static bool LocallyNearGlassFlower = false;

    public override void ResetNearbyTileEffects() => LocallyNearGlassFlower = false;
}