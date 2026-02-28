using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Verdant;

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

    [DefaultValue(false)]
    public bool JungleSpawn;

    [DefaultValue(false)]
    public bool CenterSpawn;

    //[DefaultValue(true)]
    //[Label("$Mods.Verdant.Configs.FancyBookUILabel")]
    //[Tooltip("$Mods.Verdant.Configs.FancyBookUITooltip")]
    //public bool FancyBookUI;

    [DefaultValue(1)]
    [Range(0.5f, 4f)]
    [Increment(0.1f)]
    public float DialogueSpeed;
}
