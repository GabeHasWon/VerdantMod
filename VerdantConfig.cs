using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Verdant
{
    public class VerdantServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public enum ApotheosisInteraction
        {
            World = 0, 
            Chat,
            Both
        }

        [DefaultValue(ApotheosisInteraction.World)]
        [Label("Talk Interaction")]
        [Tooltip("Controls whether certain things talk using in-world text, the chatbox, or both.")]
        public ApotheosisInteraction ApothTextSetting;
    }

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
    }
}
