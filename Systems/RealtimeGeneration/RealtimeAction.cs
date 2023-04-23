using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Verdant.Systems.RealtimeGeneration.CaptureRendering;

namespace Verdant.Systems.RealtimeGeneration;

public class RealtimeAction
{
    private readonly float TickRate = 0;
    private readonly Queue<RealtimeStep> TileActions = new();
    private readonly bool Undoable = false;
    private readonly string Name = "";

    internal bool finished = false;

    private float _timer = 0;
    private float _surpassedValues = 0;
    private Point _topLeft = new(short.MaxValue, short.MaxValue);
    private Point _bottomRight = new();
    private CaptureData _captureData = new DefaultCapture("");

    public RealtimeAction(Queue<RealtimeStep> tileActions, float tickRate, bool undoable = false, string name = null, CaptureData captureData = null)
    {
        TileActions = tileActions;
        TickRate = tickRate;
        Undoable = undoable;
        Name = name;

        if (captureData is not null)
        {
            _captureData = captureData;
            _captureData.Action = this;
        }

            var actions = tileActions.ToList();

        if (Undoable)
            foreach (var item in actions)
                AdjustRectangle(item);

        OnStart();
    }

    public void Play()
    {
        _surpassedValues = (float)Math.Floor(_timer);
        _timer += TickRate;

        if (TileActions.Count <= 0)
        {
            finished = true;
            return;
        }

        int repeats = (int)(Math.Floor(_timer) - _surpassedValues);
        for (int i = 0; i < repeats; ++i)
        {
            if (TileActions.Count <= 0)
                return;

            var step = TileActions.Dequeue();
            bool success = false;
            step.Invoke(step.Position.X, step.Position.Y, ref success);
            
            if (!success)
                i--;

            _timer = 0;
        }
    }

    private void OnStart()
    {
        if (Undoable)
        {
            string path = Path.Combine(ModLoader.ModPath.Replace("Mods", "SavedStructures"), "Structure_Verdant_" + Name + RealtimeGen.StructureID);

            var rect = new Rectangle(_topLeft.X, _topLeft.Y, _bottomRight.X - _topLeft.X, _bottomRight.Y - _topLeft.Y);
            StructureHelper.Saver.SaveToFile(rect, path);
            ModContent.GetInstance<RealtimeGen>().CapturedStructures.Add(Name, (path, new Point16(_topLeft.X, _topLeft.Y)));

            if (_captureData is not null)
            {
                _captureData.Area = new Rectangle(rect.X * 16, rect.Y * 16, rect.Width * 16, rect.Height * 16);
                OverlayRenderer.Capture(true, _captureData);
            }
        }
    }

    private void AdjustRectangle(RealtimeStep step)
    {
        if (step.Position.X < _topLeft.X)
            _topLeft.X = step.Position.X;

        if (step.Position.X > _bottomRight.X)
            _bottomRight.X = step.Position.X;

        if (step.Position.Y < _topLeft.Y)
            _topLeft.Y = step.Position.Y;

        if (step.Position.Y > _bottomRight.Y)
            _bottomRight.Y = step.Position.Y;
    }
}
