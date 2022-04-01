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

        [DefaultValue(0)]
        [Range(0, 2)]
        [Label("Enable Background Objects")]
        [Tooltip("Enables objects appearing in the surface background.")]
        public bool BackgroundObjects;
    }

    public class VerdantClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(true)]
        [Label("Enable Steam Effect")]
        [Tooltip("Enables steam overlay when inside of the underground Verdant.")]
        public bool EnableSteam;
    }
}
