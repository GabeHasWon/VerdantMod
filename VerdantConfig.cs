using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Verdant
{
    public class VerdantServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(0)]
        [Range(0, 2)]
        [Label("Talk Interaction")]
        [Tooltip("If 0, chat interaction will be done in-world only. If 1, it will be in chat only. If 2, it will be in both.")]
        public int ApothTextSetting;
    }

    public class VerdantClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [DefaultValue(0)]
        [Label("Show Catch Text")]
        [Tooltip("Displays text when hovering over an NPC that can be caught with a Bug Net.")]
        public bool ShowCatchText;
    }
}
