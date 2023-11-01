using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.World;
using Verdant.Systems.RealtimeGeneration;
using System;
using Terraria.DataStructures;
using Verdant.Systems.Syncing;

namespace Verdant.Items.Verdant.Misc;

[Sacrifice(1)]
class PlainMicrocosm : ModItem
{
    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.buyPrice(platinum: 3);
        Item.consumable = true;
        Item.width = 28;
        Item.height = 32;
        Item.useAnimation = Item.useTime = 20;
        Item.useStyle = ItemUseStyleID.HoldUp;
    }

    public override bool? UseItem(Player player)
    {
        var pos = Main.MouseWorld.ToTileCoordinates16();

        if (Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer == player.whoAmI)
            new StartMicrocosmModule(pos, (short)Main.myPlayer, true).Send();
        else if (Main.netMode == NetmodeID.SinglePlayer)
            Microcosm.SpawnMicrocosm(pos, true);
        return true;
    }
}