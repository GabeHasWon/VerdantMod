using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Misc;

namespace Verdant.Tiles.Verdant.Misc;

public class GreenCrystal : OmnidirectionalAnchorTile
{
    protected override int StyleRange => 3;

    protected override void StaticDefaults()
	{
		Main.tileLighted[Type] = true;

		LocalizedText name = CreateMapEntryName();
		// name.SetDefault("Green Crystal");
		AddMapEntry(new Color(20, 145, 41), name);

		HitSound = SoundID.Shatter;
		DustType = DustID.DungeonGreen;
	}

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) => (r, g, b) = (0.1f, 0.5f, 0.2f);
    public override bool IsTileSpelunkable(int i, int j) => true;
}