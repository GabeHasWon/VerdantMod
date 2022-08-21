using Terraria;
using Terraria.ModLoader;

namespace Verdant.Backgrounds
{
    public class VerdantUGBackground : ModUndergroundBackgroundStyle
    {
        public override void FillTextureArray(int[] textureSlots)
        {
            for (int i = 0; i < 6; ++i)
                textureSlots[i] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Backgrounds/VerdantUG" + i);
        }
    }
}