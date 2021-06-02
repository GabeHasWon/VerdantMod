using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Backgrounds.BGItem;
using Verdant.Foreground;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Trees;
using Verdant.World.Biome.Verdant;

namespace Verdant
{
    public partial class VerdantMod : Mod
	{
        public static VerdantMod Instance;

        public VerdantMod() 
        {
            Instance = this;

            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
                AutoloadBackgrounds = true,
            };
        }

        public override void Load()
        {
            Filters.Scene["Verdant:Verdant"] = new Filter(new VerdantScreenShaderData("FilterMiniTower").UseColor(0.0f, 1f, 0.0f).UseOpacity(0.06f), EffectPriority.VeryHigh); //Verdant Green shader
            SkyManager.Instance["Verdant:Verdant"] = new VerdantSky();

            BackgroundItemManager.Load(); //Main BGItem draw

            OnHooks();
        }

        private void OnHooks()
        {
            On.Terraria.Main.DrawBackgroundBlackFill += Main_DrawBackgroundBlackFill; //BGItem hook
            On.Terraria.WorldGen.GrowTree += WorldGen_GrowTree; //So that GrowTree works along with other mods
            On.Terraria.Main.DrawWater += Main_DrawWater;
            On.Terraria.Main.DrawPlayer += Main_DrawPlayer; //ForegroundItem hook
        }

        private void Main_DrawWater(On.Terraria.Main.orig_DrawWater orig, Main self, bool bg, int Style, float Alpha)
        {
            if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                Alpha *= 1.4f; //Cute little effect to make the water seem less clean

            orig(self, bg, Style, Alpha);
        }

        private bool WorldGen_GrowTree(On.Terraria.WorldGen.orig_GrowTree orig, int i, int y)
        {
            if (Framing.GetTileSafely(i, y).type == (ushort)ModContent.TileType<LushSapling>())
            {
                bool leaves = !WorldGen.gen && WorldGen.PlayerLOS(i, y);
                if (Framing.GetTileSafely(i, y).frameY == 0 && Framing.GetTileSafely(i, y + 1).type == (ushort)ModContent.TileType<LushSapling>())
                    y++;
                return VerdantTree.Spawn(i, y, -1, WorldGen.gen ? WorldGen.genRand : Main.rand, 8, 34, leaves, -1, true);
            }
            return orig(i, y);
        }

        public override void Unload()
        {
            ForegroundManager.Unload();
            BackgroundItemManager.Unload();
            VerdantPlayer.Unload();
            UnHookOn();

            Instance = null;
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup woodGrp = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            woodGrp.ValidItems.Add(ModContent.ItemType<VerdantWoodBlock>());
        }

        private void UnHookOn()
        {
            On.Terraria.Main.DrawBackgroundBlackFill -= Main_DrawBackgroundBlackFill; //do I have to unhook this? maybe. do I do it anyway? yes
            On.Terraria.WorldGen.GrowTree -= WorldGen_GrowTree;
            On.Terraria.Main.DrawWater -= Main_DrawWater;
            On.Terraria.Main.DrawPlayer -= Main_DrawPlayer;
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return;

            if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
            {
                if (Main.raining && Main.LocalPlayer.position.Y / 16 < Main.worldSurface) //raining music
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/PetalsFall");
                    priority = MusicPriority.BiomeLow;
                }
            }
        }
    }
}