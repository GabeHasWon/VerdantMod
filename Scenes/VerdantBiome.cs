using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace Verdant.Scenes;

internal class VerdantBiome : ModBiome
{
    public override void SetStaticDefaults() => DisplayName.SetDefault("Verdant");
    public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("Verdant/VerdantWaterStyle");
    public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("Verdant/VerdantSurfaceBgStyle");
    public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;
    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

    public override int Music => GetMusic();

    private int GetMusic()
    {
        if (Main.raining)
            return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PetalsFall");
        return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/VibrantHorizon");
    }

    public override string BestiaryIcon => base.BestiaryIcon;
    public override string BackgroundPath => MapBackground;
    public override Color? BackgroundColor => base.BackgroundColor;
    public override string MapBackground => "Verdant/Backgrounds/VerdantMap";

    public override bool IsBiomeActive(Player player)
    {
        bool surface = player.ZoneSkyHeight || player.ZoneOverworldHeight;
        return VerdantSystem.InVerdant && surface;
    }
}