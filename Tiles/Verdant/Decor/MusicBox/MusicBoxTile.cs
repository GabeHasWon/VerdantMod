using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using Terraria;
using Microsoft.Xna.Framework;

namespace Verdant.Tiles.Verdant.Decor.MusicBox;

public abstract class MusicBoxTile : ModTile
{
    public abstract string MusicPath { get; }
    public abstract int ItemType { get; }

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileObsidianKill[Type] = true;

        TileID.Sets.HasOutlines[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.CoordinateHeights = [16, 18];
        TileObjectData.newTile.Origin = new Point16(0, 1);
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.addTile(Type);

        RegisterItemDrop(ItemType); //Register this drop for all styles
        AddMapEntry(new Color(191, 142, 111), Language.GetText("ItemName.MusicBox"));
        DustType = -1;
    }

    public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData) //Spawn music notes
    {
        bool chance = (int)Main.timeForVisualEffects % 7 == 0 && Main.rand.NextBool(3);
        if (Lighting.UpdateEveryFrame && new FastRandom(Main.TileFrameSeed).WithModifier(i, j).Next(4) != 0 || !chance)
            return;

        var tile = Framing.GetTileSafely(i, j);
        if (!TileDrawing.IsVisible(tile) || tile.TileFrameX != 36 || tile.TileFrameY % 36 != 0)
            return;

        int goreType = Main.rand.Next(570, 573);
        var position = new Vector2(i, j) * 16 + new Vector2(8, -8);
        var velocity = new Vector2(Main.WindForVisuals * 2f, -0.5f) * new Vector2(Random(), Random());
        var gore = Gore.NewGoreDirect(new EntitySource_TileUpdate(i, j), position, velocity, goreType, .8f);
        gore.position.X -= gore.Width / 2;

        return;

        static float Random() => Main.rand.NextFloat(.5f, 1.5f);
    }

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;
        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = ItemType;
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
}
