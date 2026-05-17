using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;
using Verdant.Effects;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Verdant.Systems.TearRain;
using Verdant.Tiles.Verdant.Basic.Plants;
using Terraria.ID;

namespace Verdant.Scenes;

internal class VerdantUndergroundBiome : ModBiome
{
    private float _steamOpacity = 0f;
    private float _steamProgress = 0f;

    private float _rainSteamOpacity = 0f;

    public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("Verdant/VerdantWaterStyle");
    public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("Verdant/VerdantUGBackground");
    public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;
    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeMedium;

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

    public override void SpecialVisuals(Player player, bool isActive)
    {
        if (!ModContent.GetInstance<VerdantClientConfig>().EnableSteam)
        {
            if (Filters.Scene[EffectIDs.BiomeSteam].Active)
            {
                Filters.Scene[EffectIDs.BiomeSteam].Deactivate();
                _steamProgress = 0;
                _steamOpacity = 1;
            }

            if (Filters.Scene[EffectIDs.RainSteam].Active)
            {
                Filters.Scene[EffectIDs.RainSteam].Deactivate();
                _rainSteamOpacity = 0;
            }

            return;
        }

        bool canShowNormalSteam = (isActive && !TearRainSystem.Raining) || SmokeBulb.SmokeBulbFlag.NearSmokeBulb;

        if (!Filters.Scene[EffectIDs.BiomeSteam].Active && canShowNormalSteam)
        {
            _steamOpacity = 0.994f;
            _steamProgress = 0.2f;

            SetShader(EffectIDs.BiomeSteam);
        }
        else if (Filters.Scene[EffectIDs.BiomeSteam].Active)
        {
            float opacity = canShowNormalSteam ? 0.94f : 1f;

            _steamOpacity = MathHelper.Lerp(_steamOpacity, opacity, 0.02f);
            UpdateSteamEffect(player);

            if (!canShowNormalSteam && _steamOpacity > 0.995f)
            {
                Filters.Scene[EffectIDs.BiomeSteam].Deactivate();
                _steamProgress = 0;
                _steamOpacity = 1;
            }
        }

        bool canShowTearSteam = (isActive && TearRainSystem.Raining) || TearBulb.TearBulbFlag.NearTearBulb;
        float rainStrength = TearRainSystem.RainStrength;

        if (TearBulb.TearBulbFlag.NearTearBulb)
            rainStrength = 1;

        if (!Filters.Scene[EffectIDs.RainSteam].Active && canShowTearSteam)
        {
            Filters.Scene[EffectIDs.RainSteam].GetShader().UseImage(Mod.Assets.Request<Texture2D>("Effects/Screen/Steam", AssetRequestMode.ImmediateLoad));
            SetShader(EffectIDs.RainSteam);
        }
        else if (Filters.Scene[EffectIDs.RainSteam].Active)
        {
            float opacity = canShowTearSteam ? 0.1f + rainStrength * 0.25f : 0;

            _rainSteamOpacity = MathHelper.Lerp(_rainSteamOpacity, opacity, 0.02f);

            Filters.Scene[EffectIDs.RainSteam].GetShader().UseProgress(Main.GameUpdateCount * 0.001f + rainStrength * 0.001f);
            Vector2 direction = Main.screenPosition / Main.ScreenSize.ToVector2();
            direction.X %= 1;
            direction.Y %= 1;
            Filters.Scene[EffectIDs.RainSteam].GetShader().UseDirection(direction);
            Filters.Scene[EffectIDs.RainSteam].GetShader().UseIntensity(_rainSteamOpacity);

            if (!canShowTearSteam && _rainSteamOpacity <= 0.01f)
            {
                Filters.Scene[EffectIDs.RainSteam].Deactivate();
                _rainSteamOpacity = 0;
            }
        }
    }

    private void SetShader(string effect)
    {
        Filters.Scene[effect].GetShader().UseImage(Mod.Assets.Request<Texture2D>("Effects/Screen/Steam", AssetRequestMode.ImmediateLoad).Value, 0);
        Filters.Scene[effect].GetShader().UseImage(Mod.Assets.Request<Texture2D>("Effects/Screen/Steam", AssetRequestMode.ImmediateLoad).Value, 1);
        Filters.Scene.Activate(effect); //idk why I need to use UseImage twice but it works so I aint gonna complain
    }

    private void UpdateSteamEffect(Player player)
    {
        Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseTargetPosition(Main.screenPosition - (Vector2.UnitY * player.gfxOffY));
        Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseIntensity(_steamOpacity);
        Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseProgress(_steamProgress += 0.004f);
        Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImageScale(new Vector2(Main.screenWidth, Main.screenHeight), 0);
        Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImageScale(new Vector2(512, 512), 1);
        Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseOpacity(1f);
    }
}