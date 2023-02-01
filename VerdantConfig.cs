using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Verdant
{
    //[Label("Currently unused.")]
    //public class VerdantServerConfig : ModConfig
    //{
    //    public override ConfigScope Mode => ConfigScope.ServerSide;
    //}

    [Label("$Mods.Verdant.Configs.HeadLabel")]
    public class VerdantClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [Label("$Mods.Verdant.Configs.EnableSteamEffectLabel")]
        [Tooltip("$Mods.Verdant.Configs.EnableSteamEffectTooltip")]
        public bool EnableSteam;

        [DefaultValue(true)]
        [Label("$Mods.Verdant.Configs.EnableForeOrBackgroundObjectsLabel")]
        [Tooltip("$Mods.Verdant.Configs.EnableForeOrBackgroundObjectsTooltip")]
        public bool BackgroundObjects;

        [DefaultValue(true)]
        [Label("$Mods.Verdant.Configs.EnableCustomWaterfallsLabel")]
        [Tooltip("$Mods.Verdant.Configs.EnableCustomWaterfallsTooltip")]
        [ReloadRequired]
        public bool Waterfalls;

        [DefaultValue(true)]
        [Label("$Mods.Verdant.Configs.CustomDialogueSystemLabel")]
        [Tooltip("$Mods.Verdant.Configs.CustomDialogueSystemTooltip")]
        public bool CustomDialogue;
    }
}
