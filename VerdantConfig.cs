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
        [Tooltip("")]
        public bool BackgroundObjects;
    }

    public class VerdantClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(false)]
        [Label("Show Catch Text")]
        [Tooltip("Displays text when hovering over an NPC that can be caught with a Bug Net.")]
        public bool ShowCatchText;
    }
}
