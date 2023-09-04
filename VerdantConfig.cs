using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Verdant.Items.Verdant.Blocks.Unobtainable;

namespace Verdant
{
    //[Label("Currently unused.")]
    //public class VerdantServerConfig : ModConfig
    //{
    //    public override ConfigScope Mode => ConfigScope.ServerSide;
    //}

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
