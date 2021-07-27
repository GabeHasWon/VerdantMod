using Terraria;
using Terraria.ModLoader;

namespace Verdant.Backgrounds
{
    public class VerdantUGBackground : ModUgBgStyle
    {
        public override bool ChooseBgStyle()
        {
            return Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant;
        }

        public override void FillTextureArray(int[] textureSlots)
        {
            for (int i = 0; i < 6; ++i)
                textureSlots[i] = mod.GetBackgroundSlot("Backgrounds/VerdantUG" + i);
        }
    }
}