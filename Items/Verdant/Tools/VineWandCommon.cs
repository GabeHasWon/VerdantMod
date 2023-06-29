using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Systems.Syncing;

namespace Verdant.Items.Verdant.Tools
{
    internal class VineWandCommon
    {
        public static EnchantedVine BuildVine(int owner, EnchantedVine LastVine, Vector2? overridePosition = null, bool noSync = false)
        {
            var oldVine = LastVine;

            if (LastVine is null)
                LastVine = ForegroundManager.AddItemDirect(new EnchantedVine(overridePosition ?? Main.MouseWorld, owner), true, true) as EnchantedVine;
            else
            {
                Vector2 pos = LastVine.Center + (LastVine.DirectionTo(Main.MouseWorld) * 14);
                LastVine = ForegroundManager.AddItemDirect(new EnchantedVine(overridePosition ?? pos, owner), true, true) as EnchantedVine;
            }

            if (Main.netMode != NetmodeID.SinglePlayer && !noSync)
                new EnchantedVineModule(LastVine.position.X, LastVine.position.Y, oldVine is null ? null : (short)oldVine.WhoAmI, (short)Main.myPlayer).Send();

            if (oldVine != null)
            {
                oldVine.NextVine = LastVine;
                LastVine.PriorVine = oldVine;
            }
            LastVine.permanent = true;
            return LastVine;
        }
    }
}
