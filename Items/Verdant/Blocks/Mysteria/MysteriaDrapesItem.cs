using Microsoft.Xna.Framework;
using NetEasy;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Systems.Foreground.Tiled;
using Verdant.Systems.Syncing.Foreground;

namespace Verdant.Items.Verdant.Blocks.Mysteria;

public class MysteriaDrapesItem : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 16;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.maxStack = 999;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 6;
        Item.value = Item.buyPrice(0, 0, 0, 50);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.consumable = true;
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI != Main.myPlayer)
            return false;

        var pos = Main.MouseWorld.ToTileCoordinates();

        if (WorldGen.SolidOrSlopedTile(pos.X, pos.Y))
        {
            bool exists = ForegroundManager.Items.Any(x => x is MysteriaDrapes drape && drape.position.ToTileCoordinates() == pos);

            if (!exists)
            {
                ForegroundManager.AddItem(new MysteriaDrapes(pos), true);

                if (Main.netMode != NetmodeID.SinglePlayer)
                    new DrapesModule((byte)Main.myPlayer, pos.X, pos.Y, false).Send();
            }
            else
            {
                var drape = ForegroundManager.Items.First(x => x is MysteriaDrapes drape && drape.position.ToTileCoordinates() == pos) as MysteriaDrapes;
                drape.Grow();

                if (Main.netMode != NetmodeID.SinglePlayer)
                    new DrapesModule((byte)Main.myPlayer, ForegroundManager.Items.IndexOf(drape), 0, true).Send();
            }
            return true;
        }
        return false;
    }
}