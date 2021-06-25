using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using Verdant.Backgrounds.BGItem;
using Verdant.Noise;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Decor;

namespace Verdant.World
{
    ///General use content for the mod.
    public partial class VerdantWorld : ModWorld
    {
        public static float WorldSize { get => Main.maxTilesX / 4200f; }

        public static int VerdantTiles;
        public static int ApotheosisTiles;
        public static FastNoise genNoise;

        public static int apotheosisDialogueIndex = 0;
        public static bool apotheosisEvilDown = false;
        public static bool apotheosisSkelDown = false;

        public override TagCompound Save()
        {
            var apotheosisStats = new List<string>();
            if (apotheosisDialogueIndex >= 3)
                apotheosisStats.Add("indexFin");
            if (apotheosisEvilDown)
                apotheosisStats.Add("evilDown");
            if (apotheosisSkelDown)
                apotheosisStats.Add("skelDown");

            List<TagCompound> backgroundItems = BackgroundItemManager.Save();

            return new TagCompound
            {
                ["apotheosisStats"] = apotheosisStats,
                ["backgroundItems"] = backgroundItems
            };
        }

        public override void Load(TagCompound tag)
        {
            var stats = tag.GetList<string>("apotheosisStats");
            if (stats.Contains("indexFin")) apotheosisDialogueIndex = 3;
            apotheosisEvilDown = stats.Contains("evilDown");
            apotheosisSkelDown = stats.Contains("skelDown");

            BackgroundItemManager.Load(tag.GetList<TagCompound>("backgroundItems"));
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = apotheosisDialogueIndex >= 3;
            flags[1] = apotheosisEvilDown;
            flags[2] = apotheosisSkelDown;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();

            if (flags[0]) apotheosisDialogueIndex = 3;
            apotheosisEvilDown = flags[1];
            apotheosisSkelDown = flags[1];
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int VerdantIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

            if (tasks.Count > 0)
                tasks.Insert(1, new PassLegacy("Noise Seed", (GenerationProgress p) => { genNoise = new FastNoise(WorldGen._genRandSeed); }));

            if (VerdantIndex != -1)
                tasks.Insert(VerdantIndex + 1, new PassLegacy("Verdant Biome", VerdantGeneration)); //Verdant biome gen

            tasks.Add(new PassLegacy("Verdant Cleanup", VerdantCleanup));

            apotheosisDialogueIndex = 0;
            apotheosisEvilDown = false;
            apotheosisSkelDown = false;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            VerdantTiles = tileCounts[ModContent.TileType<VerdantSoilGrass>()] + tileCounts[ModContent.TileType<VerdantLeaves>()];
            ApotheosisTiles = tileCounts[ModContent.TileType<Apotheosis>()];
        }

        public override void ResetNearbyTileEffects()
        {
            VerdantTiles = 0;
            ApotheosisTiles = 0;
        }
    }
}