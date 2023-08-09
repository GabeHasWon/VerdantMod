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
class Microcosm : ModItem
{
    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.buyPrice(platinum: 3);
        Item.consumable = true;
        Item.width = 24;
        Item.height = 34;
        Item.useAnimation = Item.useTime = 20;
        Item.useStyle = ItemUseStyleID.HoldUp;
    }

    public override bool? UseItem(Player player)
    {
        var pos = Main.MouseWorld.ToTileCoordinates16();

        if (Main.myPlayer == player.whoAmI)
            SpawnMicrocosm(pos);

        if (Main.netMode != NetmodeID.SinglePlayer)
            new StartMicrocosmModule(pos, (short)Main.myPlayer).Send();
        return true;
    }

    internal static void SpawnMicrocosm(Point16 position)
    {
        var gen = ModContent.GetInstance<RealtimeGen>();
        gen.CurrentActions.Add(new(MicroVerdantGen.MicroVerdant(position), 15f));
    }
}