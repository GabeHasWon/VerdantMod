using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Equipables;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Materials;

public class ApotheoticSoul : ApotheoticItem
{
    public override void SetStaticDefaults()
    {
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 6));
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        ItemID.Sets.ItemIconPulse[Item.type] = true;
        ItemID.Sets.ItemNoGravity[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 18;
        Item.maxStack = Item.CommonMaxStack;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.rare = ItemRarityID.Purple;
        Item.color = RollColor();
    }

    private static Color RollColor()
    {
        Color col = (Main.GameUpdateCount % 3) switch
        {
            0 => Color.Pink,
            1 => Color.LightGreen,
            _ => Color.Red,
        };

        return new Color(Math.Clamp(col.R + Main.rand.Next(-50, 50), 0, 255),
            Math.Clamp(col.G + Main.rand.Next(-50, 50), 0, 255),
            Math.Clamp(col.B + Main.rand.Next(-50, 50), 0, 255));
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(ApotheoticSoul))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.", 9, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.0").
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.1")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.2")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.3")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.4")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.5")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.6")).
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.7")).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ApotheoticSoul.8"));
    }
}