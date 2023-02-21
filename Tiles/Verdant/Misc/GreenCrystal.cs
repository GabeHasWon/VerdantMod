using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Misc;

namespace Verdant.Tiles.Verdant.Misc;

public class GreenCrystal : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;
		Main.tileObsidianKill[Type] = true;
		Main.tileNoFail[Type] = true;
		Main.tileLighted[Type] = true;

		ModTranslation name = CreateMapEntryName();
		name.SetDefault("Green Crystal");
		AddMapEntry(new Color(20, 145, 41), name);

		TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
		TileObjectData.newTile.RandomStyleRange = 3;
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
		TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
		TileObjectData.newAlternate.RandomStyleRange = 3;
		TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
		TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.WoodenBeam };
		TileObjectData.addAlternate(1);
		TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
		TileObjectData.newAlternate.RandomStyleRange = 3;
		TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.AlternateTile, TileObjectData.newTile.Height, 0);
		TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.WoodenBeam };
		TileObjectData.addAlternate(2);
		TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
		TileObjectData.newAlternate.RandomStyleRange = 3;
		TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);
		TileObjectData.newAlternate.AnchorAlternateTiles = new[] { (int)TileID.WoodenBeam };
		TileObjectData.addAlternate(3);
		TileObjectData.newAlternate.CopyFrom(TileObjectData.StyleTorch);
		TileObjectData.addAlternate(0);
		TileObjectData.addTile(Type);

		HitSound = SoundID.Grass;
		DustType = DustID.DungeonGreen;
		ItemDrop = ModContent.ItemType<GreenCrystalItem>();
	}

	public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
	{
		int frameX = Main.tile[i, j].TileFrameX;
		spriteEffects = i % 2 == 0 && (frameX == 0 || frameX == 66) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
	}

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) => (r, g, b) = (0.1f, 0.5f, 0.2f);
    public override bool IsTileSpelunkable(int i, int j) => true;
}