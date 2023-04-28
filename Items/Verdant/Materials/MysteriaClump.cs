using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Materials;

class MysteriaClump : ModItem
{
    public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 36, ItemRarityID.White);
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Mysteria Flower Clump", "'Wonderfully soft'");

    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        gravity = 0.2f;
        maxFallSpeed = 2f;

        var abs = Math.Abs(Item.velocity.ToRotation());
        if (abs > MathHelper.PiOver2 - MathHelper.PiOver4 && abs < MathHelper.PiOver2 + MathHelper.PiOver4)
            Item.velocity = Item.velocity.RotatedByRandom(0.08f);
    }
}
