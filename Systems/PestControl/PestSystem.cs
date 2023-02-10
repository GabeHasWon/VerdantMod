using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace Verdant.Systems.PestControl;

internal class PestSystem : ModSystem
{
    public bool pestControlActive = false;
    public Point16 pestControlCenter = new(0, 0);

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (pestControlActive)
        {
            int index = layers.FindIndex(layer => layer is not null && layer.Name.Equals("Vanilla: Inventory"));
            LegacyGameInterfaceLayer uiInvasionProgress = new LegacyGameInterfaceLayer("Verdant: Pest Control UI",
                delegate
                {
                    PestInvasionUI.DrawEventUI(Main.spriteBatch);
                    return true;
                },
                InterfaceScaleType.UI);
            layers.Insert(index, uiInvasionProgress);
        }
    }

    public override void SaveWorldData(TagCompound tag)
    {
        if (pestControlActive)
        {
            tag.Add(nameof(pestControlActive), true);
            tag.Add(nameof(pestControlCenter), pestControlCenter);
        }
    }

    public override void LoadWorldData(TagCompound tag)
    {
        pestControlActive = tag.ContainsKey(nameof(pestControlActive));

        if (pestControlActive)
            pestControlCenter = tag.Get<Point16>(nameof(pestControlCenter));
    }
}
