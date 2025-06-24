using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor.MusicBox;

namespace Verdant.Items.Verdant.Blocks.MusicBoxes;

[Sacrifice(1)]
public class PetalsFallBox : BaseMusicBoxItem<PetalsFallBoxTile>
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PetalsFall"), ModContent.ItemType<PetalsFallBox>(), ModContent.TileType<PetalsFallBoxTile>());
    }
}