using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Verdant.NPCs
{
    class VerdantNPCWorld : ModSystem
    {
        public bool yellowPetalDialogue = false;

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("petal", yellowPetalDialogue);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            yellowPetalDialogue = tag.GetBool("petal");
        }
    }
}
