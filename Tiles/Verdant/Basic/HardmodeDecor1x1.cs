using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Metadata;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic;

internal class HardmodeDecor1x1 : ModTile, IFlowerTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.CrystalAnchoringData(Type, 7, VerdantGrassLeaves.VerdantGrassList().ToArray());

        Main.tileCut[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileObsidianKill[Type] = true;

        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);
        TileID.Sets.SwaysInWindBasic[Type] = true;
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
    public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => TileHelper.CrystalSetSpriteEffects(i, j, ref spriteEffects);

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(8) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();
}