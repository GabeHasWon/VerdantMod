using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Verdant.Systems.ScreenText
{
    internal class ScreenTextSystem : ModSystem
    {
        public override void OnWorldUnload() => ScreenTextManager.CurrentText = null;

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer is not null && layer.Name.Equals("Vanilla: Inventory"));
            LegacyGameInterfaceLayer uiScreenText = new LegacyGameInterfaceLayer("Verdant: Screen Text", () =>
                {
                    ScreenTextManager.Render();
                    return true;
                },
                InterfaceScaleType.UI);
            layers.Insert(index, uiScreenText);
        }
    }
}
