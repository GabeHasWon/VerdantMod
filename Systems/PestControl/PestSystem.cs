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

    public List<int> trackedEnemies = new List<int>();
    public float lastSpawnProgress = 0;

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

    public override void LoadWorldData(TagCompound tag) => pestControlActive = tag.ContainsKey(nameof(pestControlActive));

    public override void OnWorldUnload()
    {
        pestControlActive = false;
        pestControlProgress = 0;
    }

    public override void PostUpdateWorld()
    {
        if (pestControlActive)
        {
            if (pestControlProgress > 1)
            {
                pestControlActive = false;
                pestControlProgress = 0;

                foreach (int item in trackedEnemies)
                    Main.npc[item].active = false;

                trackedEnemies.Clear();
            }

            List<int> types = new()
            {
                ModContent.NPCType<ThornBeholder>(),
                ModContent.NPCType<DimCore>()
            };

            trackedEnemies.RemoveAll(x => !Main.npc[x].active || !types.Contains(Main.npc[x].type));

            if (trackedEnemies.Count < 9 || lastSpawnProgress > pestControlProgress - 0.005f)
                pestControlProgress += 0.00001f;

            if ((int)(pestControlProgress * 10000) % 100 == 0 && (int)(pestControlProgress * 10000) != (int)(lastSpawnProgress * 10000))
            {
                var loc = ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation.Value.ToWorldCoordinates();
                var pos = loc + new Vector2(0, -1000).RotatedByRandom(MathHelper.PiOver2);

                trackedEnemies.Add(NPC.NewNPC(Entity.GetSource_NaturalSpawn(), (int)pos.X, (int)pos.Y, Main.rand.Next(types)));
                lastSpawnProgress = pestControlProgress;
            }
        }
    }
}
