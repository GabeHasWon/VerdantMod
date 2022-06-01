using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Verdant.NPCs
{
    class VerdantNPCWorld : ModWorld
    {
        public bool yellowPetalDialogue = false;

        public override TagCompound Save()
        {
            return new TagCompound()
            {
                { "petal", yellowPetalDialogue }
            };
        }

        public override void Load(TagCompound tag)
        {
            yellowPetalDialogue = tag.GetBool("petal");
        }
    }
}
