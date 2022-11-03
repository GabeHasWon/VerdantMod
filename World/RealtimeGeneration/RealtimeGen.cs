using Terraria.ModLoader;

namespace Verdant.World.RealtimeGeneration
{
    internal class RealtimeGen : ModSystem
    {
        public RealtimeAction CurrentAction;

        public override void PreUpdateEntities() => CurrentAction?.Play();
    }
}
