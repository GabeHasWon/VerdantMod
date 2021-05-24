using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace Verdant.Sounds
{
    public class Arpiano : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            soundInstance = sound.CreateInstance();
            soundInstance.Volume = volume * .5f;
            soundInstance.Pan = pan;
            return soundInstance;
        }
    }

    public class SoftMelodyPiano : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            soundInstance = sound.CreateInstance();
            soundInstance.Volume = volume * .5f;
            soundInstance.Pan = pan;
            return soundInstance;
        }
    }
}