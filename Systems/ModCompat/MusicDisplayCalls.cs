using Terraria.ModLoader;

namespace Verdant.Systems.ModCompat;

internal class MusicDisplayCalls : ModSystem
{
	public override void PostSetupContent()
	{
		if (!ModLoader.TryGetMod("MusicDisplay", out Mod display))
			return;

		void AddMusic(string path, string name, string author) => display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, path), name, "by " + author, "Verdant");

		AddMusic("Sounds/Music/TearRain", "Tear Rain (Underground Verdant Theme)", "GabeHasWon");
		AddMusic("Sounds/Music/ApotheosisLullaby", "Apotheosis Lullaby (Apotheosis Theme)", "GabeHasWon & Liz");
        AddMusic("Sounds/Music/PetalsFall", "Petals Fall (Raining Verdant Theme)", "GabeHasWon & Liz");
		AddMusic("Sounds/Music/VibrantHorizon", "Vibrant Horizon (Verdant Day Theme)", "Heltonyan");
	}
}
