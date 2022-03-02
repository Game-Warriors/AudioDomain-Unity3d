using UnityEngine;

namespace GameWarriors.AudioDomain.Abstraction
{
    public enum EAudioLoopLayer { LoopGroup1, LoopGroup2, LoopGroup3 }

    public interface IAudioLoop
    {
        float MusicVolume { set; }
        bool MusicEnableState { get; set; }
        void PlayLoop(string audioName, float volume = 1, EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1);
        void PlayLoop(AudioClip audioLoop, float volume = 1, EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1);
        void StopGroupLoopSource(EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1);
        void MuteGroupLoopSource(EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1);
        void UnmuteGroupLoopSource(EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1);
        float GetClipLength(string name);
    }
}
