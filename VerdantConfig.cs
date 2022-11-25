using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Verdant
{
    //[Label("Currently unused.")]
    //public class VerdantServerConfig : ModConfig
    //{
    //    public override ConfigScope Mode => ConfigScope.ServerSide;
    //}

    [Label("Verdant Client Configuration")]
    public class VerdantClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [Label("Enable Steam Effect")]
        [Tooltip("Enables steam overlay when inside of the underground Verdant.")]
        public bool EnableSteam;

        [DefaultValue(true)]
        [Label("Enable Foreground/Background Objects")]
        [Tooltip("Enables decoration objects appearing in the surface background or foreground everywhere.")]
        public bool BackgroundObjects;

        [DefaultValue(true)]
        [Label("Enable Custom Waterfalls")]
        [Tooltip("Enables waterfalls appearing from tiles, such as the Weeping Bud.")]
        [ReloadRequired]
        public bool Waterfalls;

        [DefaultValue(true)]
        [Label("Custom Dialogue System")]
        [Tooltip("Allows the usage of a custom dialogue system for the Apotheosis. If turned off, will just use the chat.")]
        public bool CustomDialogue;
    }
}
