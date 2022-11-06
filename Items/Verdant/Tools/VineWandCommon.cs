using Microsoft.Xna.Framework;
using Terraria;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;

namespace Verdant.Items.Verdant.Tools
{
    internal class VineWandCommon
    {
        public static EnchantedVine BuildVine(int owner, EnchantedVine LastVine, bool perm = true, Vector2? overridePosition = null)
        {
            var oldVine = LastVine;

            if (LastVine is null)
                LastVine = ForegroundManager.AddItemDirect(new EnchantedVine(overridePosition ?? Main.MouseWorld, owner), true, true) as EnchantedVine;
            else
            {
                Vector2 pos = LastVine.Center + (LastVine.DirectionTo(Main.MouseWorld) * 14);
                LastVine = ForegroundManager.AddItemDirect(new EnchantedVine(overridePosition ?? pos, owner), true, true) as EnchantedVine;
            }

            if (oldVine != null)
            {
                oldVine.NextVine = LastVine;
                LastVine.PriorVine = oldVine;
            }
            LastVine.perm = perm;
            return LastVine;
        }
    }
}
