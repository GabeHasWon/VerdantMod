using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Foreground;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Verdant.Backgrounds.BGItem;
using Verdant.Tiles.Verdant.Trees;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.World.Biome;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Verdant.Effects;
using Verdant.Tiles.Verdant;
using System;

namespace Verdant
{
    public partial class VerdantMod : Mod
	{
        public static VerdantMod Instance;

        public VerdantMod() 
        {
            Instance = this;
        }

        public override void Load()
        {
            SkyManager.Instance["Verdant:Verdant"] = new VerdantSky();

            if (!Main.dedServ)
            {
                Ref<Effect> filterRef = new Ref<Effect>(Assets.Request<Effect>("Effects/Screen/SteamEffect", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
                Filters.Scene[EffectIDs.BiomeSteam] = new Filter(new ScreenShaderData(filterRef, "Steam"), EffectPriority.VeryHigh);
                Filters.Scene[EffectIDs.BiomeSteam].Load();
            }

            MonoModChanges();
        }

        public override void PostSetupContent()
        {
            Flowers.Load(this);
        }

        public override void Unload()
        {
            ForegroundManager.Unload();
            UnMonoModChanges();

            Instance = null;
        }

        private void UnMonoModChanges()
        {
            //if (ModContent.GetInstance<VerdantClientConfig>().Waterfalls)
            //    IL.Terraria.WaterfallManager.FindWaterfalls -= WaterfallManager_FindWaterfalls;
        }

        private void MonoModChanges()
        {
            ForegroundManager.Hooks();
            On.Terraria.Main.DrawBackgroundBlackFill += Main_DrawBackgroundBlackFill; //BackgroundItemManager Draw hook
            On.Terraria.WorldGen.GrowTree += WorldGen_GrowTree; //So that GrowTree works along with other mods
            On.Terraria.Main.Update += Main_Update; //Used for BackgroundItemManager Update
            On.Terraria.Main.DrawWater += Main_DrawWater;

            On.Terraria.Player.QuickMount += VinePulleyPlayer.Player_QuickMount;
            On.Terraria.Player.Teleport += VinePulleyPlayer.Player_Teleport;

            if (ModContent.GetInstance<VerdantClientConfig>().Waterfalls)
                IL.Terraria.WaterfallManager.FindWaterfalls += WaterfallManager_FindWaterfalls;
        }

        public override object Call(params object[] args)
        {
            if (args[0] is not string message)
                throw new ArgumentException("[Verdant] First argument of Call must be a string! Check the GitHub for more info.");

            message = message.ToLower();

            if (message == "inverdant")
            {
                if (args.Length == 1)
                    return VerdantSystem.InVerdant;
                else
                {
                    if (args[1] is Player player)
                        return player.GetModPlayer<VerdantPlayer>().ZoneVerdant;

                    throw new ArgumentException("[Verdant] Second argument of InVerdant must be a Player!");
                }
            }
            else if (message == "nearapotheosis")
            {
                if (args.Length == 1)
                    return VerdantSystem.NearApotheosis;
                else
                {
                    if (args[1] is Player player)
                        return player.GetModPlayer<VerdantPlayer>().ZoneApotheosis;

                    throw new ArgumentException("[Verdant] Second argument of ZoneApotheosis must be a Player!");
                }
            }

            throw new ArgumentException("[Verdant] Call didn't recieve a valid message! Valid messages are:\nInVerdant NearApotheosis");
        }
    }
}