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
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Verdant.Effects;
using MonoMod.Cil;
using Mono.Cecil.Cil;

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
            //ILHooks();
        }

        public override void Unload()
        {
            ForegroundManager.Unload();
            UnhookOn();
            //UnhookIL();

            Instance = null;
        }

        private void ILHooks()
        {
            IL.Terraria.Main.DoDraw += Main_DoDraw;
        }

        private void UnhookIL()
        {
            IL.Terraria.Main.DoDraw -= Main_DoDraw;
        }

        private void OnHooks()
        {
            On.Terraria.Main.DrawBackgroundBlackFill += Main_DrawBackgroundBlackFill; //BackgroundItemManager Draw hook
            On.Terraria.WorldGen.GrowTree += WorldGen_GrowTree; //So that GrowTree works along with other mods
            On.Terraria.Main.DrawWater += Main_DrawWater;
            On.Terraria.Main.Update += Main_Update; //Used for BackgroundItemManager Update
            On.Terraria.Main.DrawGore += Main_DrawGore; //ForegroundItem hook
            On.Terraria.Player.QuickMount += VinePulleyPlayer.Player_QuickMount;
        }

        private void UnhookOn()
        {
            On.Terraria.Main.DrawBackgroundBlackFill -= Main_DrawBackgroundBlackFill; //do I have to unhook this? maybe. do I do it anyway? yes
            On.Terraria.WorldGen.GrowTree -= WorldGen_GrowTree;
            On.Terraria.Main.DrawWater -= Main_DrawWater;
            On.Terraria.Main.Update -= Main_Update;
            On.Terraria.Main.DrawGore -= Main_DrawGore;
        }

        private void Main_DoDraw(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdsfld(typeof(Main), "snowMoon")))
                return; //Try and get to the ldsfld opcode of snowMoon

            if (!c.TryGotoNext(i => i.MatchLdsfld(typeof(Main), "spriteBatch")))
                return; //Try and get to the ldsfld opcode of spriteBatch, the one that draws frost moon

            if (!c.TryGotoNext(i => i.MatchLdsfld(typeof(Main), "spriteBatch")))
                return; //Try and get to the ldsfld opcode of spriteBatch, the one that draws generic moon

            c.Index++; //Move in front of the spriteBatch ldsfld
            ILLabel label = il.DefineLabel(); //Define return label

            c.EmitDelegate<OverrideVanillaMoonDelegate>(OverrideVanillaMoon); //Check if we want to override the vanilla moon
            c.Emit(OpCodes.Brfalse_S, label); //If we don't, skip to after the if

            c.Emit(OpCodes.Ldloc, 8); //num23 (x)
            c.Emit(OpCodes.Conv_R4); //Convert to float32

            c.Emit(OpCodes.Ldloc, 9); //num24 (y)
            c.Emit(OpCodes.Ldsfld, typeof(Main).GetField(nameof(Main.moonModY))); //Moon y offset
            c.Emit(OpCodes.Add); //Combine num24 and offset
            c.Emit(OpCodes.Conv_R4); //Convert to float32

            c.Emit(OpCodes.Ldloc, 10); //white2 (col)
            c.Emit(OpCodes.Ldloc, 12); //rotation2 (rot)
            c.Emit(OpCodes.Ldloc, 11); //num25 (scale)
            c.EmitDelegate<DrawMoonDelegate>(DrawMoon); //Now we have a DrawMoon hook!

            c.MarkLabel(label);
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
                Alpha *= 1.1f; //Cute little effect to make the water seem less clean

            orig(self, bg, Style, Alpha);
        }

        private bool WorldGen_GrowTree(On.Terraria.WorldGen.orig_GrowTree orig, int i, int y)
        {
            if (Framing.GetTileSafely(i, y).TileType == (ushort)ModContent.TileType<LushSapling>())
            {
                bool leaves = !WorldGen.gen && WorldGen.PlayerLOS(i, y);
                if (Framing.GetTileSafely(i, y).TileFrameY == 0 && Framing.GetTileSafely(i, y + 1).TileType == (ushort)ModContent.TileType<LushSapling>())
                    y++;
                return VerdantTree.Spawn(i, y, -1, WorldGen.gen ? WorldGen.genRand : Main.rand, 8, 34, leaves, -1, true);
            }
            return orig(i, y);
        }

        public override void AddRecipeGroups()/* tModPorter Note: Removed. Use ModSystem.AddRecipeGroups */
        {
            RecipeGroup woodGrp = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            woodGrp.ValidItems.Add(ModContent.ItemType<VerdantWoodBlock>());
        }
    }
}