using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Verdant
{
    public class VerdantClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        public bool EnableSteam;

        [DefaultValue(true)]
        public bool BackgroundObjects;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool Waterfalls;

        [DefaultValue(true)]
        public bool CustomDialogue;

        //[DefaultValue(true)]
        //[Label("$Mods.Verdant.Configs.FancyBookUILabel")]
        //[Tooltip("$Mods.Verdant.Configs.FancyBookUITooltip")]
        //public bool FancyBookUI;
    }
}
