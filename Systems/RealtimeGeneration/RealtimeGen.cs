using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Verdant.Systems.RealtimeGeneration.CaptureRendering;

namespace Verdant.Systems.RealtimeGeneration;

internal class RealtimeGen : ModSystem
{
    internal Dictionary<string, (string, Point16)> CapturedStructures = new();

    public RealtimeAction CurrentAction;

    private int _structureID = 0;

    public static int StructureID 
    { 
        get
        {
            int val = ModContent.GetInstance<RealtimeGen>()._structureID;
            ModContent.GetInstance<RealtimeGen>()._structureID++;
            return val;
        }
    }

    public override void PreUpdateEntities()
    {
        CurrentAction?.Play();

        if (CurrentAction is not null && CurrentAction.finished)
            CurrentAction = null;
    }

    public static bool ReplaceStructure(string name)
    {
        var captures = ModContent.GetInstance<RealtimeGen>().CapturedStructures;

        if (!captures.ContainsKey(name))
            return false;

        var structure = captures[name];
        StructureHelper.Generator.GenerateStructure(structure.Item1, structure.Item2, VerdantMod.Instance, true, true);

        captures.Remove(name);
        File.Delete(structure.Item1);
        return true;
    }

    public static bool HasStructure(string name) => ModContent.GetInstance<RealtimeGen>().CapturedStructures.ContainsKey(name);

    public override void PostDrawTiles()
    {
        var renderer = ModContent.GetInstance<OverlayRenderer>();

        renderer.Render();
    }
}
