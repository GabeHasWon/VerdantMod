using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Systems.RealtimeGeneration;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Trees;
using Verdant.World;

namespace Verdant.Tiles.Verdant.Basic.Mysteria;

public class MysteriaSprout : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileID.Sets.CommonSapling[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
        TileObjectData.newTile.Width = 1;
        TileObjectData.newTile.Height = 2;
        TileObjectData.newTile.Origin = new Point16(0, 1);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.AnchorValidTiles = new[] { ModContent.TileType<LushSoil>() };
        TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassTypes.ToList());
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.DrawFlipHorizontal = true;
        TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.newTile.RandomStyleRange = 3;
        TileObjectData.newTile.StyleMultiplier = 3;
        TileObjectData.addTile(Type);

        LocalizedText name = CreateMapEntryName();
        // name.SetDefault("Sprout");
        AddMapEntry(new Color(99, 63, 45), name);

        DustType = DustID.Grass;
        AdjTiles = new int[] { TileID.Saplings };
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;

    public override void RandomUpdate(int i, int j)
    {
        if (WorldGen.genRand.NextBool(12))
        {
            if (Framing.GetTileSafely(i, j).TileFrameY == 0)
                j++;

            if (WorldGen.PlayerLOS(i, j))
            {
                var gen = ModContent.GetInstance<RealtimeGen>();
                var queue = MysteriaTree.RealtimeGenerate(i, j, 0, Main.rand);

                if (queue.Any())
                    gen.CurrentActions.Add(new(queue, 0.3f));
            }
            else //Instantly generate tree if not near player, might as well not take extra power
                MysteriaTree.Generate(i, j, 0, Main.rand);
        }
    }

    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
}