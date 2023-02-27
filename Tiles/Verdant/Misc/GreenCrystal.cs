using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Misc;

namespace Verdant.Tiles.Verdant.Misc;

public class GreenCrystal : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileLighted[Type] = true;

		ModTranslation name = CreateMapEntryName();
		name.SetDefault("Green Crystal");
		AddMapEntry(new Color(20, 145, 41), name);

		QuickTile.CrystalAnchoringData(Type, 3);

		HitSound = SoundID.Grass;
		DustType = DustID.DungeonGreen;
		ItemDrop = ModContent.ItemType<GreenCrystalItem>();
	}

	public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => TileHelper.CrystalSetSpriteEffects(i, j, ref spriteEffects);

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) => (r, g, b) = (0.1f, 0.5f, 0.2f);
    public override bool IsTileSpelunkable(int i, int j) => true;
}