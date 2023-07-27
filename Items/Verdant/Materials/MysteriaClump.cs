using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Materials;

class MysteriaClump : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 36, ItemRarityID.White);

    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        gravity = 0.2f;
        maxFallSpeed = 2f;

        var abs = Math.Abs(Item.velocity.ToRotation());
        if (abs > MathHelper.PiOver2 - MathHelper.PiOver4 && abs < MathHelper.PiOver2 + MathHelper.PiOver4)
            Item.velocity = Item.velocity.RotatedByRandom(0.08f);
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(MysteriaClump))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.MysteriaClump", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.MysteriaClump", 100, true);
    }
}
