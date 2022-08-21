using Terraria;
using Terraria.ModLoader;

namespace Verdant.Backgrounds
{
    public class VerdantSurfaceBgStyle : ModSurfaceBackgroundStyle
    {
        public override int ChooseFarTexture() => BackgroundTextureLoader.GetBackgroundSlot(Mod, "Backgrounds/VerdantSurfaceFar");

        public override int ChooseMiddleTexture() => -1;// mod.GetBackgroundSlot("Backgrounds/VerdantSurfaceMid");

        public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
        {
            //scale *= .76f;
            b -= 250;
            return BackgroundTextureLoader.GetBackgroundSlot(Mod, "Backgrounds/VerdantSurfaceClose");
        }

        // Use this to keep far Backgrounds like the mountains.
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {
            for (int i = 0; i < fades.Length; i++)
            {
                if (i == Slot)
                {
                    fades[i] += transitionSpeed;
                    if (fades[i] > 1f)
                    {
                        fades[i] = 1f;
                    }
                }
                else
                {
                    fades[i] -= transitionSpeed;
                    if (fades[i] < 0f)
                    {
                        fades[i] = 0f;
                    }
                }
            }
        }
    }
}