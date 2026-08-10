using Microsoft.Xna.Framework.Graphics;
using SpiritReforged.Common.WorldGeneration.GenConfiguration;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace Verdant.World;

#nullable enable

// These classes are decorated to not be seen by the compiler unless Reforged is enabled, which stops errors from occurring if Reforged is disabled.
// This contains all direct references to Reforged code as well, and should be the only places that includes it (unless more pages are needed, of course).

[ExtendsFromMod("SpiritReforged")]
[JITWhenModsEnabled("SpiritReforged")]
public class VerdantGenerationPage : IGenerationPage
{
    // Page name, page back (the main element for the UI - doesn't need the outline) and page button (for use in the next/back buttons in the UI, also doesn't need outline)
    PageInfo IGenerationPage.Info => new("Verdant", ModContent.Request<Texture2D>("Verdant/World/ConfigPageBack"), ModContent.Request<Texture2D>("Verdant/World/ConfigPageButton"));

    // This should just point to your own mod.
    Mod IGenerationPage.Mod => VerdantMod.Instance;
}

[ExtendsFromMod("SpiritReforged")]
[JITWhenModsEnabled("SpiritReforged")]
public class BackslatePage : IGenerationPage
{
    PageInfo IGenerationPage.Info => new("Backslate", ModContent.Request<Texture2D>("Verdant/World/BackslatePageBack"), ModContent.Request<Texture2D>("Verdant/World/BackslatePageButton"));
    Mod IGenerationPage.Mod => VerdantMod.Instance;
}

public class VerdantGenerationLoading
{
    internal static void LoadBindings()
    {
        GenConfigLoader.LoadingMods.Add(VerdantMod.Instance);

        GenConfigLoader.CrossmodConfigurables.Add("Verdant/Verdant", LoadConfigsFrom<VerdantGenConfiguration>);
        GenConfigLoader.CrossmodConfigurables.Add("Verdant/Backslate", LoadConfigsFrom<BackslateConfiguration>);
    }

    private static List<(MemberInfo, ConfigInfo)> LoadConfigsFrom<T>()
    {
        Type type = typeof(T);
        List<(MemberInfo, ConfigInfo)> configs = [];

        foreach (FieldInfo field in type.GetFields())
        {
            if (field.GetCustomAttribute<BindToGenConfigAttribute>() is BindToGenConfigAttribute gen)
            {
                GenConfigParameters parameters = new(gen.Minimum, gen.Maximum, gen.Step);
                configs.Add((field, new ConfigInfo(parameters, gen.ReverseMinMax, gen.Slider, gen.Denominator, gen.PriorityConfig)));
            }
        }

        foreach (PropertyInfo field in type.GetProperties())
        {
            if (field.GetCustomAttribute<BindToGenConfigAttribute>() is BindToGenConfigAttribute gen)
            {
                GenConfigParameters parameters = new(gen.Minimum, gen.Maximum, gen.Step);
                configs.Add((field, new ConfigInfo(parameters, gen.ReverseMinMax, gen.Slider, gen.Denominator, gen.PriorityConfig)));
            }
        }

        return configs;
    }
}

// Classes used to hold the variables for gen configuration (per page), alongside a class safely loading it.
// LoadBindings is necessary to skip JIT on, otherwise VerdantGenerationPage is discovered by the runtime (and throws).

public class VerdantGenConfigLoader : ILoadable
{
    void ILoadable.Load(Mod mod)
    {
        if (!ModLoader.HasMod("SpiritReforged"))
            return;

        LoadBindings();
    }

    [JITWhenModsEnabled("SpiritReforged")]
    private static void LoadBindings() => VerdantGenerationLoading.LoadBindings();

    void ILoadable.Unload()
    {
    }
}

public class BackslateConfiguration
{
    [BindToGenConfig(0.2f, 3, 1, true)]
    public static float BackslateMicrobiomeModifier { get; set; } = 1;

    [BindToGenConfig(1f, 2.5f, 1f, true)]
    public static float BackslateSizeModifier { get; set; } = 1f;

    [BindToGenConfig(1, 6, 1, true)]
    public static float BackslateSpam { get; set; } = 1;

    [BindToGenConfig(5, 100, 1, true)]
    public static int BackslateWidth { get; set; } = 16;

    [BindToGenConfig(3, 160, 1, true, true, true)]
    public static int AquamarineChance { get; set; } = 22;

    [BindToGenConfig(1, 40, 1, true, true, true)]
    public static int WallChance { get; set; } = 8;

    [BindToGenConfig(1, 80, 1, true, true, true)]
    public static int BubblingWallChance { get; set; } = 12;

    [BindToGenConfig(1, 50, 1, true, true, true)]
    public static int BushChance { get; set; } = 2;

    [BindToGenConfig(5, 1000, 1, true, true, true)]
    public static int RandomAquamarine { get; set; } = 150;

    [BindToGenConfig(1, 10, 1, true)]
    public static float AquamarineChunkSize { get; set; } = 1;
}

public class VerdantGenConfiguration
{
    [BindToGenConfig(0.5f, 2, 0.05f, true)]
    public static float CircleModifier { get; set; } = 1f;

    [BindToGenConfig(0, 5, 0.05f, true)]
    public static float WaterMultiplier { get; set; } = 1f;

    [BindToGenConfig(0.5f, 5, 0.05f, true)]
    public static float StoneModifier { get; set; } = 1f;

    [BindToGenConfig(0.5f, 5, 0.05f, true)]
    public static float OreModifier { get; set; } = 1f;

    [BindToGenConfig(0.5f, 10, 0.05f, true)]
    public static float AquamarineModifier { get; set; } = 1f;

    [BindToGenConfig(0.5f, 5, 0.05f, true)]
    public static float WaterfallModifier { get; set; } = 1f;

    [BindToGenConfig(0.5f, 5, 0.05f, true)]
    public static float VinesModifier { get; set; } = 1f;

    [BindToGenConfig(0, 5, 1, true, true, true)]
    public static float VinesChance { get; set; } = 2;

    [BindToGenConfig(1, 50, 1, true, true, true)]
    public static int VineStrongChance { get; set; } = 10;

    [BindToGenConfig(1, 50, 1, true, true, true)]
    public static int LightbulbChance { get; set; } = 11;

    [BindToGenConfig(1, 150, 1, true, true, true)]
    public static int WaterfallFlowerChance { get; set; } = 32;

    [BindToGenConfig(1, 150, 1, true, true, true)]
    public static int BeehiveChance { get; set; } = 40;

    [BindToGenConfig(1, 150, 1, true, true, true)]
    public static int WallLightbulbChance { get; set; } = 42;

    [BindToGenConfig(1, 150, 1, true, true, true)]
    public static int WallLightbulbBigChance { get; set; } = 68;

    [BindToGenConfig(1, 150, 1, true, true, true)]
    public static int TreeChance { get; set; } = 16;

    [BindToGenConfig(1, 150, 1, true, true, true)]
    public static int PuffChance { get; set; } = 60;
}

// Wrapper attribute used to mimic the in-house attributes used by Reforged. 
// This can be copied at will for your own mod.

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class BindToGenConfigAttribute(object min, object max, object step, bool slider = false, bool reverseMinMax = false,
    bool denominator = false, string? priorityConfig = null) : Attribute
{
    public readonly object Minimum = min;
    public readonly object Maximum = max;
    public readonly object Step = step;
    public readonly bool Slider = slider;
    public readonly bool ReverseMinMax = reverseMinMax;
    public readonly bool Denominator = denominator;
    public readonly string? PriorityConfig = priorityConfig;
}
