using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Verdant.Backgrounds.BGItem;
using Verdant.Systems.Foreground;
using Verdant.Items.Verdant.Blocks.LushWood;
using Verdant.Noise;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Decor;
using Verdant.World;
using Terraria.DataStructures;

namespace Verdant;

public class DownedID
{
    public const string AnyMech = "anyMech";
    public const string Plantera = "plantera";
    public const string Golem = "golem";
    public const string Cultist = "cultist";
    public const string MoonLord = "moonLord";
}

public class VerdantSystem : ModSystem
{
    private int VerdantTiles;
    private int ApotheosisTiles;

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

    public Dictionary<string, bool> apotheosisDowns = new() 
    { 
        { DownedID.AnyMech, false }, { DownedID.Plantera, false }, { DownedID.Golem, false }, { DownedID.Cultist, false }, { DownedID.MoonLord, false } 
    };

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
        AddIfTrue(apotheosisPestControlNotif, "pestControlNotif");

        foreach (var pair in apotheosisDowns)
            AddIfTrue(pair.Value, pair.Key);

        List<TagCompound> backgroundItems = BackgroundItemManager.Save();

        genNoise = null;

        tag.Add("apotheosisStats", apotheosisStats);
        tag.Add("backgroundItems", backgroundItems);

        if (ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation is not null)
            tag.Add("apotheosisLocation", ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation.Value);
    }

    public override void OnWorldUnload()
    {
        ForegroundManager.Unload();
        BackgroundItemManager.Unload();

        apotheosisIntro = false;
        apotheosisGreeting = false;
        apotheosisEyeDown = false;
        apotheosisEvilDown = false;
        apotheosisSkelDown = false;
        apotheosisWallDown = false;
        apotheosisPestControlNotif = false;
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
        apotheosisPestControlNotif = stats.Contains("pestControlNotif");

        foreach (var pair in apotheosisDowns)
            apotheosisDowns[pair.Key] = stats.Contains(pair.Key);

        if (Main.netMode != NetmodeID.Server)
        {
            var bgItems = tag.GetList<TagCompound>("backgroundItems");

            if (bgItems != null)
                BackgroundItemManager.Load(bgItems);
        }

        if (tag.ContainsKey("apotheosisLocation"))
            ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation = tag.Get<Point16>("apotheosisLocation");
    }

    public override void PostAddRecipes() => SacrificeAutoloader.Load(Mod);

    public override void NetSend(BinaryWriter writer)
    {
        var flags = new BitsByte();
        flags[0] = apotheosisGreeting;
        flags[1] = apotheosisEyeDown;
        flags[2] = apotheosisEvilDown;
        flags[3] = apotheosisSkelDown;
        flags[4] = apotheosisWallDown;
        flags[5] = apotheosisIntro;
        flags[6] = apotheosisDowns.TryGetValue(DownedID.AnyMech, out bool anyMech) && anyMech;
        flags[7] = apotheosisDowns.TryGetValue(DownedID.Plantera, out bool plantera) && plantera;
        writer.Write(flags);

        var moreHmFlags = new BitsByte();
        moreHmFlags[0] = apotheosisDowns.TryGetValue(DownedID.Golem, out bool golem) && golem;
        moreHmFlags[1] = apotheosisDowns.TryGetValue(DownedID.Cultist, out bool cultist) && cultist;
        moreHmFlags[2] = apotheosisDowns.TryGetValue(DownedID.MoonLord, out bool moonLord) && moonLord;
        writer.Write(moreHmFlags);
    }

    public override void NetReceive(BinaryReader reader)
    {
        BitsByte flags = reader.ReadByte();

        apotheosisGreeting = flags[0];
        apotheosisEyeDown = flags[1];
        apotheosisEvilDown = flags[2];
        apotheosisSkelDown = flags[3];
        apotheosisWallDown = flags[4];
        apotheosisIntro = flags[5];

        if (!apotheosisDowns.TryAdd(DownedID.AnyMech, flags[6]))
            apotheosisDowns[DownedID.AnyMech] = flags[6];

        if (!apotheosisDowns.TryAdd(DownedID.Plantera, flags[7]))
            apotheosisDowns[DownedID.Plantera] = flags[7];

        BitsByte moreHmFlags = reader.ReadByte();

        if (!apotheosisDowns.TryAdd(DownedID.Golem, moreHmFlags[0]))
            apotheosisDowns[DownedID.Golem] = moreHmFlags[0];

        if (!apotheosisDowns.TryAdd(DownedID.Cultist, moreHmFlags[1]))
            apotheosisDowns[DownedID.Cultist] = moreHmFlags[1];

        if (!apotheosisDowns.TryAdd(DownedID.MoonLord, moreHmFlags[2]))
            apotheosisDowns[DownedID.MoonLord] = moreHmFlags[2];
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        bool remnants = ModLoader.HasMod("Remnants");
        int VerdantIndex = tasks.FindIndex(genpass => genpass.Name.Equals(remnants ? "[R] Jungle Pyramid" : "Jungle Temple"));
        VerdantGenSystem genSystem = ModContent.GetInstance<VerdantGenSystem>();

        if (tasks.Count > 0)
            tasks.Insert(1, new PassLegacy("Noise Seed", (GenerationProgress p, GameConfiguration config) => { genNoise = new FastNoise(WorldGen._genRandSeed); }));

        if (VerdantIndex != -1)
        {
            tasks.Insert(VerdantIndex + 1, new PassLegacy("Verdant Biome", genSystem.VerdantGeneration, 600)); //Verdant biome gen
            tasks.Insert(tasks.Count - 2, new PassLegacy("Verdant Cleanup", genSystem.VerdantCleanup, 25)); //And final cleanup
            tasks.Insert(VerdantIndex - 1, new PassLegacy("Spam Aquamarine", AquamarineGen.SpamGems, 2f));
        }

        int settleWaterIndex = tasks.FindIndex(pass => pass.Name.Equals(remnants ? "Oasis" : "Settle Liquids Again"));

        if (settleWaterIndex != -1)
            tasks.Insert(settleWaterIndex + 1, new PassLegacy("Aquamarine Microbiome", AquamarineGen.Gen, 40f)); //Aquamarine microbiome

        apotheosisIntro = false;
        apotheosisGreeting = false;
        apotheosisEvilDown = false;
        apotheosisSkelDown = false;
        apotheosisWallDown = false;
        apotheosisEyeDown = false;

        foreach (var pair in apotheosisDowns)
            apotheosisDowns[pair.Key] = false;
    }

    public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
    {
        VerdantTiles = tileCounts[ModContent.TileType<VerdantGrassLeaves>()] + tileCounts[ModContent.TileType<VerdantLeaves>()] + tileCounts[ModContent.TileType<LushGrass>()]
            + tileCounts[ModContent.TileType<LightbulbLeaves>()] + tileCounts[ModContent.TileType<OvergrownBricks>()];
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