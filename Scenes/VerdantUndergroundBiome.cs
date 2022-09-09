using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using Verdant.World;

namespace Verdant.Scenes
{
    internal class VerdantUndergroundBiome : ModBiome
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Verdant");
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("Verdant/VerdantWaterStyle");
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("Verdant/VerdantUGBackground");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		public override int Music => GetMusic();

        private int GetMusic()
        {
			return -1;
		}

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => MapBackground;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => "Verdant/Backgrounds/VerdantMap";

		public override bool IsBiomeActive(Player player)
		{
			bool underground = player.position.Y > Main.worldSurface * 16;
			return VerdantSystem.InVerdant && underground;
		}

		public override void OnEnter(Player player) => player.GetModPlayer<VerdantPlayer>().ZoneVerdant = true;
		public override void OnLeave(Player player) => player.GetModPlayer<VerdantPlayer>().ZoneVerdant = false;
	}
}
