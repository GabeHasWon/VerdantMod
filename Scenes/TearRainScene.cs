using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.TearRain;

namespace Verdant.Scenes;

internal class TearRainScene : ModSceneEffect
{
    public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TearRainEvent");
    public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
    public override bool IsSceneEffectActive(Player player) => TearRainSystem.Raining && player.InModBiome<VerdantUndergroundBiome>();
}
