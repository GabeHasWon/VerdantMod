using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Tiles.Verdant.Basic.Aquamarine;

public class EmbeddedAquamarine : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.Dirt, SoundID.Dig, new Color(10, 36, 65));
        RegisterItemDrop(ModContent.ItemType<Items.Verdant.Blocks.Aquamarine.AquamarineItem>());
        Main.tileBrick[Type] = true;
    }
}

public class EmbeddedStoneAquamarine : ModTile
{
    public override void SetStaticDefaults()
    {
        QuickTile.SetAll(this, 0, DustID.Stone, SoundID.Dig, new Color(10, 36, 65));
        RegisterItemDrop(ModContent.ItemType<Items.Verdant.Blocks.Aquamarine.AquamarineItem>());
        Main.tileBrick[Type] = true;
    }
}