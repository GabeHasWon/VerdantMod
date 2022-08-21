using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using Verdant.World;

namespace Verdant.Scenes
{
    internal class VerdantBiome : ModBiome
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Briar");
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("SpiritMod/ReachWaterStyle");
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("SpiritMod/ReachSurfaceBgStyle");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		public override int Music => GetMusic();

        private int GetMusic()
        {
            if (Main.LocalPlayer.position.Y / 16 < Main.worldSurface) //petals fall
            {
                if (Main.raining)
                    return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PetalsFall");
            }

            if (Main.LocalPlayer.position.Y / 16f > Main.worldSurface) //tear rain
				return MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TearRain");
			return -1;
		}

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => MapBackground;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => "SpiritMod/Backgrounds/BriarMapBG";

		public override bool IsBiomeActive(Player player)
		{
			bool surface = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return VerdantWorld.VerdantTiles > 40 && surface;
		}

		public override void OnEnter(Player player) => player.GetModPlayer<VerdantPlayer>().ZoneVerdant = true;
		public override void OnLeave(Player player) => player.GetModPlayer<VerdantPlayer>().ZoneVerdant = false;
	}
}
