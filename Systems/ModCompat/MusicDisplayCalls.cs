using Terraria.Localization;
using Terraria.ModLoader;

namespace Verdant.Systems.ModCompat;

internal class MusicDisplayCalls : ModSystem
{
	public override void PostAddRecipes()
	{
		if (!ModLoader.TryGetMod("MusicDisplay", out Mod display))
			return;

        LocalizedText modName = Language.GetText("Mods.Verdant.MusicDisplay.ModName");

		void AddMusic(string path, string name)
        {
            LocalizedText author = Language.GetText("Mods.Verdant.MusicDisplay." + name + ".Author");
            LocalizedText displayName = Language.GetText("Mods.Verdant.MusicDisplay." + name + ".DisplayName");
            display.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, path), displayName, author, modName);
        }

		AddMusic("Sounds/Music/TearRain", "TearRain");
		AddMusic("Sounds/Music/ApotheosisLullaby", "ApotheosisLullaby");
        AddMusic("Sounds/Music/PetalsFall", "PetalsFall");
		AddMusic("Sounds/Music/VibrantHorizon", "VibrantHorizon");
	}
}
