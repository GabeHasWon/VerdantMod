using Terraria.ModLoader;

namespace Verdant.Systems.ModCompat;

internal class MusicDisplayCalls : ModSystem
{
	public override void PostSetupContent()
	{
		if (!ModLoader.TryGetMod("MusicDisplay", out Mod display))
			return;

		void AddMusic(string path, string name) => display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, path), name, "Verdant");

		AddMusic("Sounds/Music/ApotheosisLullaby", "GabeHasWon, Liz - Apotheosis Lullaby (Apotheosis Theme)");
		AddMusic("Sounds/Music/PetalsFall", "GabeHasWon, Liz - Petals Fall (Raining Verdant Theme)");
		AddMusic("Sounds/Music/TearRain", "GabeHasWon - Tear Rain (Underground Verdant Theme)");
		AddMusic("Sounds/Music/VibrantHorizon", "Heltonyan - Vibrant Horizon (Verdant Day Theme)");
	}
}
