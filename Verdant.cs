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
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Verdant.Tiles.Verdant;

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

            OnHooks();
        }

        public override void PostSetupContent()
        {
            Flowers.Load(this);
        }

        public override void Unload()
        {
            ForegroundManager.Unload();

            Instance = null;
        }

        private void OnHooks()
        {
            ForegroundManager.Hooks();
            On.Terraria.Main.DrawBackgroundBlackFill += Main_DrawBackgroundBlackFill; //BackgroundItemManager Draw hook
            On.Terraria.WorldGen.GrowTree += WorldGen_GrowTree; //So that GrowTree works along with other mods
            On.Terraria.Main.Update += Main_Update; //Used for BackgroundItemManager Update
            On.Terraria.Main.DrawWater += Main_DrawWater;
            On.Terraria.Player.QuickMount += VinePulleyPlayer.Player_QuickMount;
        }

        private delegate bool OverrideVanillaMoonDelegate();
        private delegate void DrawMoonDelegate(float x, float y, Color col, float rot, float scale);
        private void DrawMoon(float x, float y, Color col, float rot, float scale)
        {
            Texture2D tex = Assets.Request<Texture2D>("NPCs/Passive/Flotiny", ReLogic.Content.AssetRequestMode.AsyncLoad).Value;
            col.A = 255;
            Main.spriteBatch.Draw(tex, new Vector2(x, y + Main.moonModY), 
                new Rectangle(0, 0, tex.Width, tex.Width), 
                col, rot, new Vector2(tex.Width / 2, tex.Width / 2), scale * 3, SpriteEffects.None, 0f);
        }

        public bool OverrideVanillaMoon()
        {
            return false;
        }

        private void Main_Update(On.Terraria.Main.orig_Update orig, Main self, GameTime gameTime)
        {
            bool playerInv = Main.hasFocus && (!Main.autoPause || Main.netMode != NetmodeID.SinglePlayer || (Main.autoPause && !Main.playerInventory && Main.netMode == NetmodeID.SinglePlayer));
            if (Main.PlayerLoaded && BackgroundItemManager.Loaded && playerInv)
                BackgroundItemManager.Update();

            orig(self, gameTime);
        }

        private void Main_DrawWater(On.Terraria.Main.orig_DrawWater orig, Main self, bool bg, int Style, float Alpha)
        {
            if (Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                Alpha *= 0.9f;

            orig(self, bg, Style, Alpha);
        }

        private bool WorldGen_GrowTree(On.Terraria.WorldGen.orig_GrowTree orig, int i, int y)
        {
            if (Framing.GetTileSafely(i, y).TileType == (ushort)ModContent.TileType<LushSapling>())
            {
                bool leaves = !WorldGen.gen && WorldGen.PlayerLOS(i, y);
                if (Framing.GetTileSafely(i, y).TileFrameY == 0 && Framing.GetTileSafely(i, y + 1).TileType == (ushort)ModContent.TileType<LushSapling>())
                    y++;

                int maxSize = y > Main.worldSurface ? 20 : 42;
                return VerdantTree.Spawn(i, y, -1, WorldGen.gen ? WorldGen.genRand : Main.rand, 4, maxSize, leaves, -1, true);
            }
            return orig(i, y);
        }
    }
}