using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Systems.PestControl;

internal class PestPlayer : ModPlayer
{
    public bool inPestControl = false;

    public override void ResetEffects()
    {
        inPestControl = false;

        if (ModContent.GetInstance<PestSystem>().pestControlActive)
        {
            Player.noBuilding = true;
            Player.AddBuff(BuffID.NoBuilding, 2);
            inPestControl = true;
        }
    }
}
