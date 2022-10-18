using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Verdant.Backgrounds.BGItem;
using Verdant.Foreground;
using Verdant.Foreground.Parallax;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Items.Verdant.Tools;
using Verdant.Noise;
using Verdant.Projectiles.Misc;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Decor;
using Verdant.World;

namespace Verdant
{
    public class VerdantSystem : ModSystem
    {
        private int VerdantTiles;
        private int ApotheosisTiles;

        public List<Projectile> Vines = new List<Projectile>();

        public static bool InVerdant => ModContent.GetInstance<VerdantSystem>().VerdantTiles > 40;
        public static bool NearApotheosis => ModContent.GetInstance<VerdantSystem>().ApotheosisTiles > 2;

        public static FastNoise genNoise;

        public int apotheosisDialogueIndex = 0;
        public bool apotheosisEyeDown = false;
        public bool apotheosisEvilDown = false;
        public bool apotheosisSkelDown = false;
        public bool apotheosisWallDown = false;

        public override void SaveWorldData(TagCompound tag)
        {
            var apotheosisStats = new List<string>();
            if (apotheosisDialogueIndex >= 3)
                apotheosisStats.Add("indexFin");
            if (apotheosisEvilDown)
                apotheosisStats.Add("eocDown");
            if (apotheosisEvilDown)
                apotheosisStats.Add("evilDown");
            if (apotheosisSkelDown)
                apotheosisStats.Add("skelDown");
            if (apotheosisWallDown)
                apotheosisStats.Add("wallDown");

            List<TagCompound> backgroundItems = BackgroundItemManager.Save();

            genNoise = null; //Unload this so it's not taking up space

            tag.Add("apotheosisStats", apotheosisStats);
            tag.Add("backgroundItems", backgroundItems);

            SaveVines(tag);

            var clouds = ForegroundManager.PlayerLayerItems.Where(x => x is CloudbloomEntity);
            var positions = new List<Vector2>();
            foreach (var item in clouds)
                positions.Add(item.Center);
            tag.Add("cloudPositions", positions);
        }

        public override void OnWorldUnload()
        {
            ForegroundManager.Unload();
            BackgroundItemManager.Unload();
        }

        private static void SaveVines(TagCompound tag)
        {
            var vines = ForegroundManager.Items.Where(x => !x.killMe && x is EnchantedVine vine && vine.perm);
            var positions = new List<Vector2>();
            var continueSet = new List<bool>();

            for (int i = 0; i < vines.Count(); ++i)
            {
                var item = vines.ElementAt(i) as EnchantedVine;
                positions.Add(item.Center);

                if (i > 0 && i < vines.Count() - 2 && item != (vines.ElementAt(i + 1) as EnchantedVine).PriorVine)
                    positions.Add(Vector2.Zero);
            }

            tag.Add("permVinePositions", positions);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var stats = tag.GetList<string>("apotheosisStats");
            if (stats.Contains("indexFin")) apotheosisDialogueIndex = 3;
            apotheosisEyeDown = stats.Contains("eocDown");
            apotheosisEvilDown = stats.Contains("evilDown");
            apotheosisSkelDown = stats.Contains("skelDown");
            apotheosisWallDown = stats.Contains("wallDown");

            if (Main.netMode != NetmodeID.Server)
            {
                var bgItems = tag.GetList<TagCompound>("backgroundItems");
                if (bgItems != null)
                    BackgroundItemManager.Load(bgItems);

                SpawnPermVines(tag.GetList<Vector2>("permVinePositions"));

                var clouds = tag.GetList<Vector2>("cloudPositions");
                foreach (var item in clouds)
                    ForegroundManager.AddItem(new CloudbloomEntity(item), true, true);
            }
        }

        private static void SpawnPermVines(IList<Vector2> positions)
        {
            List<List<Vector2>> Vines = new()
            {
                new List<Vector2>()
            };

            int currentSet = 0;

            for (int i = 0; i < positions.Count; ++i)
            {
                if (positions[i] == Vector2.Zero /*i > 0 && !continuity[i - 1]*/)
                {
                    currentSet++;
                    Vines.Add(new List<Vector2>());
                    continue;
                }

                Vines[currentSet].Add(positions[i]);
            }

            foreach (var item in Vines)
                BuildVine(item);
        }

        private static void BuildVine(List<Vector2> item)
        {
            EnchantedVine lastVine = null;
            for (int i = 0; i < item.Count; i++)
                lastVine = VineWandCommon.BuildVine(Main.myPlayer, lastVine, true, item[i]);
        }

        public override void PostAddRecipes() => SacrificeAutoloader.Load(Mod);

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = apotheosisDialogueIndex >= 3;
            flags[1] = apotheosisEvilDown;
            flags[2] = apotheosisSkelDown;
            flags[3] = apotheosisWallDown;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();

            if (flags[0]) apotheosisDialogueIndex = 3;
            apotheosisEvilDown = flags[1];
            apotheosisSkelDown = flags[2];
            apotheosisWallDown = flags[3];
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int VerdantIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Jungle Temple"));
            VerdantGenSystem genSystem = ModContent.GetInstance<VerdantGenSystem>();

            if (tasks.Count > 0)
                tasks.Insert(1, new PassLegacy("Noise Seed", (GenerationProgress p, GameConfiguration config) => { genNoise = new FastNoise(WorldGen._genRandSeed); }));

            if (VerdantIndex != -1)
                tasks.Insert(VerdantIndex + 1, new PassLegacy("Verdant Biome", genSystem.VerdantGeneration)); //Verdant biome gen

            tasks.Add(new PassLegacy("Verdant Cleanup", genSystem.VerdantCleanup)); //And final cleanup

            apotheosisDialogueIndex = 0;
            apotheosisEvilDown = false;
            apotheosisSkelDown = false;
        }

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            VerdantTiles = tileCounts[ModContent.TileType<VerdantGrassLeaves>()] + tileCounts[ModContent.TileType<VerdantLeaves>()];
            ApotheosisTiles = tileCounts[ModContent.TileType<Apotheosis>()];
        }

        public override void ResetNearbyTileEffects()
        {
            VerdantTiles = 0;
            ApotheosisTiles = 0;
        }

        public override void Unload() => BackgroundItemManager.Unload();

        public override void AddRecipeGroups()
        {
            RecipeGroup woodGrp = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            woodGrp.ValidItems.Add(ModContent.ItemType<VerdantWoodBlock>());
        }
    }
}