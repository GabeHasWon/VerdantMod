using Ionic.Zip;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Players;

/// <summary>
/// Simply a bool for if the player is currently on a Propellerpad.
/// </summary>
internal class PropellerpadPlayer : ModPlayer
{
    public bool onPropellerpad = false;
    public bool lastOnPropellerpad = false;

    public override void ResetEffects()
    {
        lastOnPropellerpad = onPropellerpad;
        onPropellerpad = false;
    }
}
