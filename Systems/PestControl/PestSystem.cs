using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Verdant.NPCs.Enemy.PestControl;
using Verdant.World;

namespace Verdant.Systems.PestControl;

internal class PestSystem : ModSystem
{
    public bool pestControlActive = false;
    public float pestControlProgress = 0;

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
            tag.Add(nameof(pestControlActive), true);
    }

    public override void LoadWorldData(TagCompound tag)
    {
        pestControlActive = tag.ContainsKey(nameof(pestControlActive));
    }

    public override void PostUpdateWorld()
    {
        if (pestControlActive)
        {
            List<int> types = new()
            {
                ModContent.NPCType<ThornBeholder>(),
                ModContent.NPCType<DimCore>()
            };

            pestControlProgress += 0.0001f;

            if ((int)(pestControlProgress * 10000) % 1200 == 0)
            {
                var loc = ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation.Value.ToWorldCoordinates();
                var pos = loc + new Vector2(0, -1000).RotatedByRandom(MathHelper.PiOver2);
                NPC.NewNPC(Entity.GetSource_NaturalSpawn(), (int)pos.X, (int)pos.Y, Main.rand.Next(types));
            }
        }
    }
}
