using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Verdant.Backgrounds.BGItem;
using Verdant.Foreground;
using Verdant.Foreground.Parallax;
using Verdant.Items.Verdant.Blocks.LushWood;
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

            var clouds = ForegroundManager.Items.Where(x => x is CloudbloomEntity);
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
            var vines = Main.projectile.Take(Main.maxProjectiles).Where(x => x.active && x.ModProjectile is VineWandVine vine && (vine.nextVine > -1 || vine.priorVine > -1));
            var positions = new List<Vector2>();
            var continueSet = new List<bool>();

            for (int i = 0; i < vines.Count(); ++i)
            {
                Projectile item = vines.ElementAt(i);
                positions.Add(item.Center);

                if (i > 0 && i < vines.Count() - 2)
                {
                    var vine = item.ModProjectile as VineWandVine;
                    continueSet.Add(item.whoAmI == (vines.ElementAt(i - 1).ModProjectile as VineWandVine).nextVine && item.whoAmI == (vines.ElementAt(i + 1).ModProjectile as VineWandVine).priorVine);
                }
                else
                    continueSet.Add(false);
            }

            tag.Add("permVinePositions", positions);
            tag.Add("permVineContinuity", continueSet);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var stats = tag.GetList<string>("apotheosisStats");
            if (stats.Contains("indexFin")) apotheosisDialogueIndex = 3;
            apotheosisEyeDown = stats.Contains("eocDown");
            apotheosisEvilDown = stats.Contains("evilDown");
            apotheosisSkelDown = stats.Contains("skelDown");
            apotheosisWallDown = stats.Contains("wallDown");

            var bgItems = tag.GetList<TagCompound>("backgroundItems");
            if (bgItems != null)
                BackgroundItemManager.Load(bgItems);

            SpawnPermVines(tag.GetList<Vector2>("permVinePositions"), tag.GetList<bool>("permVineContinuity"));

            var clouds = tag.GetList<Vector2>("cloudPositions");
            foreach (var item in clouds)
                ForegroundManager.AddItem(new CloudbloomEntity(item));
        }

        private static void SpawnPermVines(IList<Vector2> positions, IList<bool> continuity)
        {
            int lastVine = 0;

            for (int i = positions.Count - 1; i >= 0; i--)
            {
                int proj = Projectile.NewProjectile(Entity.GetSource_NaturalSpawn(), positions[i], Vector2.Zero, ModContent.ProjectileType<VineWandVine>(), 0, 0f, Main.LocalPlayer.whoAmI);
                Projectile projectile = Main.projectile[proj];
                VineWandVine vine = projectile.ModProjectile as VineWandVine;
                vine.perm = true;

                if (continuity[i])
                {
                    vine.priorVine = lastVine;
                    (Main.projectile[lastVine].ModProjectile as VineWandVine).nextVine = proj;
                }

                //if (i == 0)
                //    vine.priorVine = lastVine;

                lastVine = proj;
            }
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

        public override void Unload() => BackgroundItemManager.Unload();

        public override void AddRecipeGroups()
        {
            RecipeGroup woodGrp = RecipeGroup.recipeGroups[RecipeGroup.recipeGroupIDs["Wood"]];
            woodGrp.ValidItems.Add(ModContent.ItemType<VerdantWoodBlock>());
        }
    }
}