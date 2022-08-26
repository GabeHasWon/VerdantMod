using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Verdant.Backgrounds.BGItem;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Noise;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Decor;
using Verdant.World;

namespace Verdant
{
    public class VerdantSystem : ModSystem
    {
        private int VerdantTiles;
        private int ApotheosisTiles;

        public static bool InVerdant => ModContent.GetInstance<VerdantSystem>().VerdantTiles > 40;
        public static bool NearApotheosis => ModContent.GetInstance<VerdantSystem>().ApotheosisTiles > 2;

        public static FastNoise genNoise;

        public int apotheosisDialogueIndex = 0;
        public bool apotheosisEvilDown = false;
        public bool apotheosisSkelDown = false;
        public bool apotheosisWallDown = false;

        public override void SaveWorldData(TagCompound tag)
        {
            var apotheosisStats = new List<string>();
            if (apotheosisDialogueIndex >= 3)
                apotheosisStats.Add("indexFin");
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
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var stats = tag.GetList<string>("apotheosisStats");
            if (stats.Contains("indexFin")) apotheosisDialogueIndex = 3;
            apotheosisEvilDown = stats.Contains("evilDown");
            apotheosisSkelDown = stats.Contains("skelDown");
            apotheosisWallDown = stats.Contains("wallDown");

            var bgItems = tag.GetList<TagCompound>("backgroundItems");
            if (bgItems != null)
                BackgroundItemManager.Load(bgItems);
        }

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

        public override void Unload()
        {
            BackgroundItemManager.Unload();
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup woodGrp = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            woodGrp.ValidItems.Add(ModContent.ItemType<VerdantWoodBlock>());
        }
    }
}