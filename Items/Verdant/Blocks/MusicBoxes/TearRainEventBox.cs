using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Decor.MusicBox;

namespace Verdant.Items.Verdant.Blocks.MusicBoxes;

[Sacrifice(1)]
public class TearRainEventBox : BaseMusicBoxItem<TearRainEventBoxTile>
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TearRainEvent"), ModContent.ItemType<TearRainEventBox>(), ModContent.TileType<TearRainEventBoxTile>());
    }
}