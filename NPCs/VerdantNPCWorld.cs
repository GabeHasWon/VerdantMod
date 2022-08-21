using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Verdant.NPCs
{
    class VerdantNPCWorld : ModSystem
    {
        public bool yellowPetalDialogue = false;

        public override void SaveWorldData(TagCompound tag)/* tModPorter Suggestion: Edit tag parameter instead of returning new TagCompound */
        {
            return new TagCompound()
            {
                { "petal", yellowPetalDialogue }
            };
        }

        public override void LoadWorldData(TagCompound tag)
        {
            yellowPetalDialogue = tag.GetBool("petal");
        }
    }
}
