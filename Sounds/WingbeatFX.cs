using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace Verdant.Sounds
{
    public class WingbeatFX : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            soundInstance = sound.CreateInstance();
            type = SoundType.Custom;
            return soundInstance;
        }
    }
}