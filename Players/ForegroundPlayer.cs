using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.Syncing.Foreground;

namespace Verdant.Players;

internal class ForegroundPlayer : ModPlayer
{
    public override void OnEnterWorld()
    {
        if (Main.netMode != NetmodeID.SinglePlayer)
            new SyncForegroundModule((byte)Main.myPlayer).Send();
    }
}
