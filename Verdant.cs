using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Foreground;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Verdant.Backgrounds.BGItem;
using Verdant.Tiles.Verdant.Trees;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.World;
using Verdant.World.Biome;

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
            //Filters.Scene["Verdant:Verdant"] = new Filter(new VerdantScreenShaderData("FilterMiniTower").UseColor(0.0f, 1f, 0.0f).UseOpacity(0.01f), EffectPriority.VeryHigh); //Verdant Green shader
            SkyManager.Instance["Verdant:Verdant"] = new VerdantSky();

            OnHooks();
        }

        private void OnHooks()
        {
            On.Terraria.Main.DrawBackgroundBlackFill += Main_DrawBackgroundBlackFill; //BackgroundItemManager Draw hook
            On.Terraria.WorldGen.GrowTree += WorldGen_GrowTree; //So that GrowTree works along with other mods
            On.Terraria.Main.DrawWater += Main_DrawWater;
            //On.Terraria.Main.DrawPlayer += Main_DrawPlayer; //ForegroundItem hook
            On.Terraria.Main.Update += Main_Update; //Used for BackgroundItemManager Update
            On.Terraria.Main.DrawGore += Main_DrawGore;
        }

        private void Main_Update(On.Terraria.Main.orig_Update orig, Main self, GameTime gameTime)
        {
            bool playerInv = Main.hasFocus && (!Main.autoPause || Main.netMode != NetmodeID.SinglePlayer || (Main.autoPause && !Main.playerInventory && Main.netMode == NetmodeID.SinglePlayer));
            if (Main.playerLoaded && BackgroundItemManager.Loaded && playerInv)
                BackgroundItemManager.Update();

            orig(self, gameTime);
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
            VerdantPlayer.Unload();
            VerdantWorld.Unload();
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
            //On.Terraria.Main.DrawPlayer -= Main_DrawPlayer;
            On.Terraria.Main.Update -= Main_Update;
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
            if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneApotheosis) //apotheosis melody
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/ApotheosisLullaby");
                priority = MusicPriority.BiomeHigh;
            }
        }
    }
}