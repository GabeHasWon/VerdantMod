using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Dusts;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Tiles.Verdant.Decor.PuffFurniture;

public class PuffPiano : ModTile
{
    [Sacrifice(3)]
    public class PuffPianoItem : ModItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 40, 30, ModContent.TileType<PuffPiano>());
        public override void AddRecipes() 
            => QuickItem.AddRecipe(this, TileID.Sawmill, 1, (ModContent.ItemType<LushLeaf>(), 7), (ModContent.ItemType<PuffMaterial>(), 8), (ItemID.Book, 1), (ItemID.Bone, 4));
    }

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileTable[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileID.Sets.DisableSmartCursor[Type] = true;
        TileID.Sets.HasOutlines[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.Height = 2;
        TileObjectData.newTile.CoordinateHeights = [16, 18];
        TileObjectData.addTile(Type);

        DustType = ModContent.DustType<PuffDust>();

        AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
        AddMapEntry(new Color(124, 93, 68), Language.GetText("ItemName.Piano"));
        RegisterItemDrop(ModContent.ItemType<PuffPianoItem>());
    }

    public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
    public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;

    public override bool RightClick(int i, int j)
    {
        int rand = Main.rand.Next(2);
        SoundEngine.PlaySound(new SoundStyle(rand == 0 ? "Verdant/Sounds/Arpiano" : "Verdant/Sounds/SoftMelodyPiano") with { PitchVariance = 0.05f, Volume = 0.8f }, new Vector2(i, j) * 16);
        return true;
    }

    public override void MouseOver(int i, int j)
    {
        Main.LocalPlayer.cursorItemIconText = "Play";
        Main.LocalPlayer.cursorItemIconID = -1;
        Main.LocalPlayer.noThrow = 2;
        Main.LocalPlayer.cursorItemIconEnabled = true;
    }
}