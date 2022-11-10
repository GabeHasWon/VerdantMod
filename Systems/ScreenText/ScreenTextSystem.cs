using Terraria.ModLoader;

namespace Verdant.Systems.ScreenText
{
    internal class ScreenTextSystem : ModSystem
    {
        public override void OnWorldUnload()
        {
            ScreenTextManager.CurrentText = null;
        }
    }
}
