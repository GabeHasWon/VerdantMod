using FullSerializer;
using Microsoft.Xna.Framework.Graphics;
using NetEasy;
using NetSerializer;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Verdant.Effects;
using Verdant.Systems.Foreground;
using Verdant.Systems.ModCompat;
using Verdant.Systems.TearRain;
using Verdant.Tiles;
using Verdant.Tiles.Verdant;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant;

public partial class VerdantMod : Mod
{
    public static VerdantMod Instance => ModContent.GetInstance<VerdantMod>();

    public static ModKeybind SquidHotkey;

    public static bool DebugModActive => ModLoader.HasMod("CheatSheet") || ModLoader.HasMod("HEROsMod") || ModLoader.HasMod("DragonLens");

    public static void DebugLogMessage(Type type)
    {
#if DEBUG
        Instance.Logger.Debug("Got packet: " + type.Name);
#endif
    }

    public override void Load()
    {
        SquidHotkey = KeybindLoader.RegisterKeybind(this, "Verdant:SquidForm", Microsoft.Xna.Framework.Input.Keys.LeftShift);

        if (!Main.dedServ)
        {
            Asset<Effect> filterRef = Assets.Request<Effect>("Effects/Screen/SteamEffect", AssetRequestMode.ImmediateLoad);
            Filters.Scene[EffectIDs.BiomeSteam] = new Filter(new ScreenShaderData(filterRef, "Steam"), EffectPriority.VeryHigh);
            Filters.Scene[EffectIDs.BiomeSteam].Load();

            Asset<Effect> filterShader = Assets.Request<Effect>("Effects/Screen/RainSteam");
            Filters.Scene[EffectIDs.RainSteam] = new Filter(new ScreenShaderData(filterShader, "Rain"), EffectPriority.VeryHigh);
        }

        LoadVerdantGrasses();
        MonoModChanges();
        NewBeginningsCompatibility.AddOrigin();

        NPCUtils.NPCUtils.AutoloadModBannersAndCritters(this);
        NPCUtils.NPCUtils.TryLoadBestiaryHelper(this);
    }

    private void LoadVerdantGrasses()
    {
        var types = AssemblyManager.GetLoadableTypes(Code).Where(x => typeof(IVerdantGrassTile).IsAssignableFrom(x) && !x.IsAbstract);

        foreach (var item in types)
        {
            string name = item.Assembly.GetName().Name;
            string type = item.Name;
            VerdantGrassLeaves.AddGrass(name + "." + type);
        }

        VerdantGrassLeaves.FinalizeGrass();
    }

    public override void PostSetupContent()
    {
        NetEasy.NetEasy.Register(this);
        Flowers.Load(this);
    }

    public override void Unload() => ForegroundManager.Unload();

    public override void HandlePacket(BinaryReader reader, int whoAmI) => NetEasy.NetEasy.HandleModule(reader, whoAmI);

    private void MonoModChanges()
    {
        ForegroundManager.Hooks();
        On_Main.DrawBackgroundBlackFill += Main_DrawBackgroundBlackFill; //BackgroundItemManager Draw hook
        On_WorldGen.GrowTree += WorldGen_GrowTree; //So that GrowTree works along with other mods
        On_Main.DoUpdateInWorld += On_Main_DoUpdateInWorld; //Used for BackgroundItemManager Update
        On_Main.oldDrawWater += On_Main_oldDrawWater;

        if (ModContent.GetInstance<VerdantClientConfig>().Waterfalls)
            IL_WaterfallManager.FindWaterfalls += WaterfallManager_FindWaterfalls;
    }

    public override object Call(params object[] args)
    {
        if (args[0] is not string message)
            throw new ArgumentException("[Verdant] First argument of Call must be a string! Check the GitHub for more info.");

        message = message.ToLower();

        if (message == "inverdant")
            return CallMethods.InVerdant(args);
        else if (message == "nearapotheosis")
            return CallMethods.NearApotheosis(args);
        else if (message == "setverdantarea")
        {
            CallMethods.SetVerdantArea(args);
            return null;
        }
        else if (message == "anyrainat")
            return CallMethods.AnyRainAt(args[1..]);
        else if (message == "tearrain")
            return TearRainSystem.Raining;

        throw new ArgumentException("[Verdant] Call didn't recieve a valid message! Valid messages are:\nInVerdant NearApotheosis SetVerdantArea AnyRainAt");
    }
}