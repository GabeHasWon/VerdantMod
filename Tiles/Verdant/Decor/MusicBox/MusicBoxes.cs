using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.MusicBoxes;

namespace Verdant.Tiles.Verdant.Decor.MusicBox;

public class LullabyBoxTile : MusicBoxTile
{
    public override string MusicPath => "Verdant/Sounds/Music/ApotheosisLullaby";
    public override int ItemType => ModContent.ItemType<LullabyBox>();
}

public class PetalsFallBoxTile : MusicBoxTile
{
    public override string MusicPath => "Verdant/Sounds/Music/PetalsFall";
    public override int ItemType => ModContent.ItemType<PetalsFallBox>();
}

public class TearRainBoxTile : MusicBoxTile
{
    public override string MusicPath => "Verdant/Sounds/Music/TearRain";
    public override int ItemType => ModContent.ItemType<TearRainBox>();
}

public class VibrantHorizonBoxTile : MusicBoxTile
{
    public override string MusicPath => "Verdant/Sounds/Music/VibrantHorizon";
    public override int ItemType => ModContent.ItemType<VibrantHorizonBox>();
}

public class TearRainEventBoxTile : MusicBoxTile
{
    public override string MusicPath => "Verdant/Sounds/Music/TearRainEvent";
    public override int ItemType => ModContent.ItemType<VibrantHorizonBox>();
}