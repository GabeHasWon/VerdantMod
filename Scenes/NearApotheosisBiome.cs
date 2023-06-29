using Terraria;
using Terraria.ModLoader;

namespace Verdant.Scenes;

internal class NearApotheosisBiome : ModBiome
{
	public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ApotheosisLullaby");
	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
    public override bool IsBiomeActive(Player player) => VerdantSystem.NearApotheosis;
}
