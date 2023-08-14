using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Materials;

public class ApotheoticSoul : ModItem
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
}