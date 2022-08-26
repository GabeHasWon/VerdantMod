using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Verdant.Effects;

namespace Verdant.Scenes
{
    internal class VerdantBiome : ModBiome
    {
        private float _steamIntensity = 1f;
        private float _steamOpacity = 0f;
        private float _steamProgress = 0f;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Verdant");
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("Verdant/VerdantWaterStyle");
		public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.Find<ModSurfaceBackgroundStyle>("Verdant/VerdantSurfaceBgStyle");
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
		public override string MapBackground => "Verdant/Backgrounds/VerdantMap";

		public override bool IsBiomeActive(Player player)
		{
			bool surface = player.ZoneSkyHeight || player.ZoneOverworldHeight;
			return VerdantSystem.InVerdant && surface;
		}

		public override void OnEnter(Player player) => player.GetModPlayer<VerdantPlayer>().ZoneVerdant = true;
		public override void OnLeave(Player player) => player.GetModPlayer<VerdantPlayer>().ZoneVerdant = false;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (!Filters.Scene[EffectIDs.BiomeSteam].Active && ModContent.GetInstance<VerdantClientConfig>().EnableSteam)
            {
                if (player.GetModPlayer<VerdantPlayer>().ZoneVerdant && player.position.Y / 16f > Main.worldSurface)
                {
                    Filters.Scene.Activate(EffectIDs.BiomeSteam, Vector2.Zero); //idk why I need to use UseImage twice but it works so I aint gonna complain
                    Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImage(Mod.Assets.Request<Texture2D>("Effects/Screen/Steam", ReLogic.Content.AssetRequestMode.AsyncLoad).Value, 0);
                    Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImage(Mod.Assets.Request<Texture2D>("Effects/Screen/Steam", ReLogic.Content.AssetRequestMode.AsyncLoad).Value, 1);
                    _steamIntensity = 1f;
                }
            }
            else
            {
                bool validArea = player.GetModPlayer<VerdantPlayer>().ZoneVerdant && player.position.Y / 16f > Main.worldSurface && ModContent.GetInstance<VerdantClientConfig>().EnableSteam;
                float baseIntensity = validArea ? 0.94f : 1f;
                float opacity = validArea ? 0.94f : 0f;

                _steamIntensity = MathHelper.Lerp(_steamIntensity, baseIntensity, 0.02f);
                _steamOpacity = MathHelper.Lerp(_steamOpacity, opacity, 0.02f);

                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseTargetPosition(player.Center + (Vector2.UnitY * player.gfxOffY));
                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseIntensity(_steamIntensity);
                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseProgress(_steamProgress += 0.004f);
                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImageScale(new Vector2(Main.screenWidth, Main.screenHeight), 0);
                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImageScale(new Vector2(512, 512), 1);
                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseOpacity(_steamOpacity);

                if (!validArea && _steamIntensity > 0.99f)
                {
                    Filters.Scene[EffectIDs.BiomeSteam].Deactivate();

                    _steamProgress = 0;
                    _steamIntensity = 1f;
                }
            }
        }
    }
}
