using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.World;

namespace Verdant.Systems.PestControl;

internal class PestPlayer : ModPlayer
{
    public bool inPestControl = false;

    public bool InPestControl
    {
        get
        {
            if (!inPestControl)
                return false;

            if (ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation is null)
                return false;

            var apoth = ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation.Value;
            var loc = new Vector2(apoth.X, apoth.Y).ToWorldCoordinates();

            return ModContent.GetInstance<PestSystem>().pestControlActive && Player.DistanceSQ(loc) / (16 * 16) < 120 * 120;
        }
    }

    public override void ResetEffects()
    {
        inPestControl = false;

        if (ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation is not null)
        {
            var apoth = ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation.Value;
            var loc = new Vector2(apoth.X, apoth.Y).ToWorldCoordinates();

            if (ModContent.GetInstance<PestSystem>().pestControlActive && Player.DistanceSQ(loc) / (16 * 16) < 120 * 120)
            {
                Player.noBuilding = true;
                Player.AddBuff(BuffID.NoBuilding, 2);
                inPestControl = true;
            }
        }
    }
}