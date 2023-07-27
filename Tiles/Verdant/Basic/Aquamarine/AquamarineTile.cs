using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Aquamarine;

namespace Verdant.Tiles.Verdant.Basic.Aquamarine;

public class AquamarineTile : OmnidirectionalAnchorTile
{
    protected override int StyleRange => 3;

    protected override void StaticDefaults()
    {
        Main.tileLighted[Type] = true;

        LocalizedText name = CreateMapEntryName();
        // name.SetDefault("Aquamarine");
        AddMapEntry(new Color(56, 144, 170), name);

        HitSound = SoundID.Shatter;
        DustType = DustID.Water;
    }

    public override bool IsTileSpelunkable(int i, int j) => true;
}