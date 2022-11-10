using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using Verdant.Effects;
using Microsoft.Xna.Framework.Graphics;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Scenes
{
    internal class VerdantUndergroundBiome : ModBiome
    {
        private float _steamOpacity = 0f;
        private float _steamProgress = 0f;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Verdant");
		public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("Verdant/VerdantWaterStyle");
        public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("Verdant/VerdantUGBackground");
		public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TearRain");

		public override string BestiaryIcon => base.BestiaryIcon;
		public override string BackgroundPath => MapBackground;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override string MapBackground => "Verdant/Backgrounds/VerdantMap";

		public override bool IsBiomeActive(Player player)
		{
			bool underground = player.position.Y > Main.worldSurface * 16;
			return VerdantSystem.InVerdant && underground;
		}

        public override void OnInBiome(Player player)
        {
            if (!ModContent.GetInstance<VerdantSystem>().apotheosisIntro)
            {
                ScreenTextManager.CurrentText = ApotheosisDialogueCache.IntroDialogue();
                ModContent.GetInstance<VerdantSystem>().apotheosisIntro = true;
            }
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (!Filters.Scene[EffectIDs.BiomeSteam].Active && ModContent.GetInstance<VerdantClientConfig>().EnableSteam)
            {
                if (isActive)
                {
                    _steamOpacity = 0.98f;
                    _steamProgress = 0.2f;

                    Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImage(Mod.Assets.Request<Texture2D>("Effects/Screen/Steam").Value, 0);
                    Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImage(Mod.Assets.Request<Texture2D>("Effects/Screen/Steam").Value, 1);
                    Filters.Scene.Activate(EffectIDs.BiomeSteam); //idk why I need to use UseImage twice but it works so I aint gonna complain
                }
            }
            else
            {
                bool validArea = isActive && ModContent.GetInstance<VerdantClientConfig>().EnableSteam;
                float opacity = validArea ? 0.94f : 1f;

                _steamOpacity = MathHelper.Lerp(_steamOpacity, opacity, 0.02f);
                UpdateShader(player);

                if (!validArea && _steamOpacity > 0.99f)
                {
                    Filters.Scene[EffectIDs.BiomeSteam].Deactivate();
                    _steamProgress = 0;
                }
            }
        }

        private void UpdateShader(Player player)
        {
            Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseTargetPosition(Main.screenPosition + (Vector2.UnitY * player.gfxOffY));
            Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseIntensity(_steamOpacity);
            Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseProgress(_steamProgress += 0.004f);
            Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImageScale(new Vector2(Main.screenWidth, Main.screenHeight), 0);
            Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImageScale(new Vector2(512, 512), 1);
            Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseOpacity(1f);
        }

        public override void OnEnter(Player player) => player.GetModPlayer<VerdantPlayer>().ZoneVerdant = true;
		public override void OnLeave(Player player) => player.GetModPlayer<VerdantPlayer>().ZoneVerdant = false;
	}
}
