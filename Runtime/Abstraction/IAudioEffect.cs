using UnityEngine;

namespace GameWarriors.AudioDomain.Abstraction
{
    public interface IAudioEffect
    {
        float SfxVolume { set; }
        bool SfxEnableState { get; set; }
        void PlayEffect(string effectName, float volume = 1, float delay = 0);
        void PlayEffect(AudioClip audioClip, float volume = 1, float delay = 0);
    }
}
