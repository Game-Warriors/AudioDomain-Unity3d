using UnityEngine;
using UnityEngine.Audio;

namespace GameWarriors.AudioDomain.Data
{
    public class AudioConfigData : ScriptableObject
    {
        public const string ASSET_PATH = "Assets/AssetData/Resources/AudioConfig.asset";
        public const string RESOURCE_PATH = "AudioConfig";
        [SerializeField] private float _baseSfxVolume;
        [SerializeField] private float _baseLoopVolume;
        [SerializeField] private float _disableVolume;
        [SerializeField] private float _fadeSpeed;
        [SerializeField] private AudioClip[] _clipItems;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _loopSourceSample;
        [SerializeField] private int _sfxSourceCount;
        [SerializeField] private AudioSource _sfxSourceSample;

        public int SfxSourceCount => _sfxSourceCount;
        public AudioSource LoopSource => _loopSourceSample;
        public AudioSource SfxSource => _sfxSourceSample;
        public AudioMixer BaseAudioMixer => _audioMixer;
        public float BaseSfxVolume => _baseSfxVolume;
        public float BaseLoopVolume => _baseLoopVolume;
        public float DisableVolume => _disableVolume;
        public float FadeSpeed => _fadeSpeed;
        public AudioClip[] AudioClips => _clipItems;

        public void SetClips(AudioClip[] clipItems)
        {
            _clipItems = clipItems;
        }

        public void SetVolumeAndMixer(float baseLoopVolume, float baseSfxVolume, float disableVolume, AudioMixer audioMixer, float fadeSpeed)
        {
            _baseLoopVolume = baseLoopVolume;
            _baseSfxVolume = baseSfxVolume;
            _disableVolume = disableVolume;
            _audioMixer = audioMixer;
            _fadeSpeed = fadeSpeed;
        }

        public void SetAudioSource(AudioSource loopSourceSample, AudioSource sfxSourceSample, int sfxSourceCount)
        {
            _loopSourceSample = loopSourceSample;
            _sfxSourceSample = sfxSourceSample;
            _sfxSourceCount = sfxSourceCount;
        }
    }
}
