using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Backgrounds.BGItem;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant
{
    public partial class VerdantMod : Mod
    {
        private void Main_DrawBackgroundBlackFill(On.Terraria.Main.orig_DrawBackgroundBlackFill orig, Main self)
        {
            orig(self);

            if (Main.PlayerLoaded && BackgroundItemManager.Loaded && !Main.gameMenu)
                BackgroundItemManager.Draw();
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

        private void WaterfallManager_FindWaterfalls(ILContext il)
        {
            ILCursor c = new(il);

            if (!c.TryGotoNext(MoveType.After, x => x.MatchCall<Tile>("active")))
                return;

            c.Emit(OpCodes.Ldloc_S, (byte)4); //i
            c.Emit(OpCodes.Ldloc_S, (byte)5); //j
            c.Emit(OpCodes.Ldarg_0);
            c.Emit<WaterfallManager>(OpCodes.Ldfld, "waterfalls"); //WaterfallManager.waterfalls
            c.Emit(OpCodes.Ldarg_0);
            c.Emit<WaterfallManager>(OpCodes.Ldflda, "currentMax"); //WaterfallManager.currentMax
            c.Emit(OpCodes.Ldarg_0);
            c.Emit<WaterfallManager>(OpCodes.Ldfld, "qualityMax"); //WaterfallManager.qualityMax

            c.EmitDelegate(AddWaterFlowersFalls); //Call the adjustment method (which also sets currentMax)
        }

        private static void AddWaterFlowersFalls(int i, int j, WaterfallManager.WaterfallData[] data, ref int currentMax, int qualityMax)
        {
            if (currentMax >= qualityMax || data is null)
                return;

            Tile currentTile = Main.tile[i, j];
            if (currentTile.HasTile && currentTile.TileType == ModContent.TileType<WaterPlant>() && currentTile.TileFrameX == 18 && currentTile.TileFrameY == 18)
            {
                data[currentMax].x = i;
                data[currentMax].y = j;
                data[currentMax].type = 6;
                currentMax++;

                data[currentMax].x = i + 1;
                data[currentMax].y = j;
                data[currentMax].type = 6;
                currentMax++;
            }
        }
    }
}