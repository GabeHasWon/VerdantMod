using Terraria;
using Terraria.ModLoader;
using Verdant.World;

namespace Verdant.Scenes
{
    internal class ApotheosisSceneEffect : ModSceneEffect
    {
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ApotheosisLullaby");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
        public override bool IsSceneEffectActive(Player player) => VerdantWorld.ApotheosisTiles > 2;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (isActive)
                player.GetModPlayer<VerdantPlayer>().ZoneApotheosis = true;
        }
    }
}
