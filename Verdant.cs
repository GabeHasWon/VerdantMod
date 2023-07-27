using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Verdant.Effects;
using Verdant.Tiles.Verdant;
using System;
using System.IO;
using Verdant.Systems.ModCompat;

namespace Verdant;

public partial class VerdantMod : Mod
{
    public static ModKeybind SquidHotkey;
    public static VerdantMod Instance;

    public static bool DebugModActive => ModLoader.TryGetMod("CheatSheet", out Mod _) || ModLoader.TryGetMod("HEROsMod", out Mod _) || ModLoader.TryGetMod("DragonLens", out Mod _);

    public VerdantMod() 
    {
        Instance = this;
    }

    public override void Load()
    {
        SquidHotkey = KeybindLoader.RegisterKeybind(this, "Verdant:SquidForm", Microsoft.Xna.Framework.Input.Keys.LeftShift);

        if (!Main.dedServ)
        {
            Ref<Effect> filterRef = new Ref<Effect>(Assets.Request<Effect>("Effects/Screen/SteamEffect", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene[EffectIDs.BiomeSteam] = new Filter(new ScreenShaderData(filterRef, "Steam"), EffectPriority.VeryHigh);
            Filters.Scene[EffectIDs.BiomeSteam].Load();
        }

        MonoModChanges();
        NewBeginningsCompatibility.AddOrigin();
    }

    public override void PostSetupContent()
    {
        NetEasy.NetEasy.Register(this);

        Flowers.Load(this);
    }

    public override void Unload()
    {
        ForegroundManager.Unload();

        Instance = null;
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        NetEasy.NetEasy.HandleModule(reader, whoAmI);
    }

    private void MonoModChanges()
    {
        ForegroundManager.Hooks();
        On_Main.DrawBackgroundBlackFill += Main_DrawBackgroundBlackFill; //BackgroundItemManager Draw hook
        On_WorldGen.GrowTree += WorldGen_GrowTree; //So that GrowTree works along with other mods
        On_Main.Update += Main_Update; //Used for BackgroundItemManager Update
        On_Main.oldDrawWater += On_Main_oldDrawWater;

        On_Player.QuickMount += VinePulleyPlayer.Player_QuickMount;
        On_Player.Teleport += VinePulleyPlayer.Player_Teleport;

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

        throw new ArgumentException("[Verdant] Call didn't recieve a valid message! Valid messages are:\nInVerdant NearApotheosis SetVerdantArea");
    }
}