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
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Items.Verdant.Tools;
using Verdant.Noise;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Decor;
using Verdant.World;
using Terraria.DataStructures;

namespace Verdant;

public class VerdantSystem : ModSystem
{
    private int VerdantTiles;
    private int ApotheosisTiles;

    public List<Projectile> Vines = new List<Projectile>();

    public static bool InVerdant => ModContent.GetInstance<VerdantSystem>().VerdantTiles > 40;
    public static bool NearApotheosis => ModContent.GetInstance<VerdantSystem>().ApotheosisTiles > 2;

    public static FastNoise genNoise;

    public bool apotheosisIntro = false;
    public bool apotheosisGreeting = false;
    public bool apotheosisEyeDown = false;
    public bool apotheosisEvilDown = false;
    public bool apotheosisSkelDown = false;
    public bool apotheosisWallDown = false;
    public bool apotheosisPestControlNotif = false;

    public Dictionary<string, bool> apotheosisDowns = new() { { "anyMech", false }, { "plantera", false }, { "golem", false }, { "moonLord", false } };

    public bool microcosmUsed = false;

    public override void SaveWorldData(TagCompound tag)
    {
        var apotheosisStats = new List<string>();

        void AddIfTrue(bool condition, string name)
        {
            if (condition)
                apotheosisStats.Add(name);
        }

        AddIfTrue(apotheosisIntro, "intro");
        AddIfTrue(apotheosisGreeting, "indexFin");
        AddIfTrue(apotheosisEyeDown, "eocDown");
        AddIfTrue(apotheosisEvilDown, "evilDown");
        AddIfTrue(apotheosisSkelDown, "skelDown");
        AddIfTrue(apotheosisWallDown, "wallDown");
        AddIfTrue(microcosmUsed, "microcosm");
        AddIfTrue(apotheosisPestControlNotif, "pestControlNotif");

        foreach (var pair in apotheosisDowns)
            AddIfTrue(pair.Value, pair.Key);

        List<TagCompound> backgroundItems = BackgroundItemManager.Save();

        genNoise = null; //Unload this so it's not taking up space

        tag.Add("apotheosisStats", apotheosisStats);
        tag.Add("backgroundItems", backgroundItems);

        SaveVines(tag);
        SaveClouds(tag);

        if (ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation is not null)
            tag.Add("apotheosisLocation", ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation.Value);
    }

    private void SaveClouds(TagCompound tag)
    {
        var clouds = ForegroundManager.PlayerLayerItems.Where(x => x is CloudbloomEntity);
        var positions = new List<Vector2>();
        var puffPositions = new List<Vector2>();

        foreach (var item in clouds)
        {
            if ((item as CloudbloomEntity).puff)
                puffPositions.Add(item.Center);
            else
                positions.Add(item.Center);
        }

        tag.Add("cloudPositions", positions);
        tag.Add("puffPositions", puffPositions);
    }

    public override void OnWorldUnload()
    {
        ForegroundManager.Unload();
        BackgroundItemManager.Unload();
    }

    private static void SaveVines(TagCompound tag)
    {
        var vines = ForegroundManager.Items.Where(x => !x.killMe && x is EnchantedVine vine && vine.permanent);
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
        apotheosisIntro = stats.Contains("intro");
        apotheosisGreeting = stats.Contains("indexFin");
        apotheosisEyeDown = stats.Contains("eocDown");
        apotheosisEvilDown = stats.Contains("evilDown");
        apotheosisSkelDown = stats.Contains("skelDown");
        apotheosisWallDown = stats.Contains("wallDown");
        microcosmUsed = stats.Contains("microcosm");
        apotheosisPestControlNotif = stats.Contains("pestControlNotif");

        foreach (var pair in apotheosisDowns)
            apotheosisDowns[pair.Key] = stats.Contains(pair.Key);

        if (Main.netMode != NetmodeID.Server)
        {
            var bgItems = tag.GetList<TagCompound>("backgroundItems");
            if (bgItems != null)
                BackgroundItemManager.Load(bgItems);

            SpawnPermVines(tag.GetList<Vector2>("permVinePositions"));

            var clouds = tag.GetList<Vector2>("cloudPositions");
            foreach (var item in clouds)
                ForegroundManager.AddItem(new CloudbloomEntity(item), true, true);

            var puffs = tag.GetList<Vector2>("puffPositions");
            foreach (var item in puffs)
                ForegroundManager.AddItem(new CloudbloomEntity(item, true), true, true);
        }

        if (tag.ContainsKey("apotheosisLocation"))
            ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation = tag.Get<Point16>("apotheosisLocation");
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
            lastVine = VineWandCommon.BuildVine(Main.myPlayer, lastVine, item[i]);
    }

    public override void PostAddRecipes() => SacrificeAutoloader.Load(Mod);

    public override void NetSend(BinaryWriter writer)
    {
        var flags = new BitsByte();
        flags[0] = apotheosisGreeting;
        flags[1] = apotheosisEvilDown;
        flags[2] = apotheosisSkelDown;
        flags[3] = apotheosisWallDown;
        flags[4] = apotheosisIntro;
        writer.Write(flags);
    }

    public override void NetReceive(BinaryReader reader)
    {
        BitsByte flags = reader.ReadByte();

        apotheosisGreeting = flags[0];
        apotheosisEvilDown = flags[1];
        apotheosisSkelDown = flags[2];
        apotheosisWallDown = flags[3];
        apotheosisIntro = flags[4];
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
    {
        int VerdantIndex = tasks.FindIndex(genpass => genpass.Name.Equals(ModLoader.TryGetMod("Remnants", out Mod _) ? "Jungle Pyramid" : "Jungle Temple"));
        VerdantGenSystem genSystem = ModContent.GetInstance<VerdantGenSystem>();

        if (tasks.Count > 0)
            tasks.Insert(1, new PassLegacy("Noise Seed", (GenerationProgress p, GameConfiguration config) => { genNoise = new FastNoise(WorldGen._genRandSeed); }));

        if (VerdantIndex != -1)
        {
            tasks.Insert(VerdantIndex + 1, new PassLegacy("Verdant Biome", genSystem.VerdantGeneration)); //Verdant biome gen
            tasks.Add(new PassLegacy("Verdant Cleanup", genSystem.VerdantCleanup)); //And final cleanup
        }

        apotheosisIntro = false;
        apotheosisGreeting = false;
        apotheosisEvilDown = false;
        apotheosisSkelDown = false;
        apotheosisWallDown = false;
        apotheosisEyeDown = false;
        microcosmUsed = false;

        foreach (var pair in apotheosisDowns)
            apotheosisDowns[pair.Key] = false;
    }

    public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
    {
        VerdantTiles = tileCounts[ModContent.TileType<VerdantGrassLeaves>()] + tileCounts[ModContent.TileType<VerdantLeaves>()];
        ApotheosisTiles = tileCounts[ModContent.TileType<Apotheosis>()] + tileCounts[ModContent.TileType<HardmodeApotheosis>()];
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