﻿using System;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Walls;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Materials;

class PinkPetal : ApotheoticItem
{
    int updateCounter = 0;

    public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 24, ItemRarityID.White, 999, false);

    public override void AddRecipes()
    {
        QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<VerdantPinkPetalWallItem>(), 4));

        QuickItem.AddRecipe(ItemID.BrightPinkDye, TileID.DyeVat, 1, (ModContent.ItemType<PinkPetal>(), 8), (ItemID.SilverDye, 1));
        QuickItem.AddRecipe(ItemID.PinkandBlackDye, TileID.DyeVat, 1, (ModContent.ItemType<PinkPetal>(), 8), (ItemID.BlackDye, 1));
    }

    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        if (Item.velocity.Y > 0.10f)
            Item.velocity.X = (float)-Math.Sin(updateCounter++ * 0.03f) * 1.15f * Item.velocity.Y * (1 - (Item.stack / 999f));

        gravity = 0.09f;
        maxFallSpeed = 0.8f;
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(PinkPetal))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.PinkPetal", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.PinkPetal", true);
    }
}