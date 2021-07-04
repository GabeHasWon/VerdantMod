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
            textureSlots[0] = mod.GetBackgroundSlot("Backgrounds/VerdantUGB0");
            textureSlots[1] = mod.GetBackgroundSlot("Backgrounds/VerdantUGB1");
            //textureSlots[2] = mod.GetBackgroundSlot("Backgrounds/ExampleBiomeUG2");
            //textureSlots[3] = mod.GetBackgroundSlot("Backgrounds/ExampleBiomeUG3");
        }
    }
}