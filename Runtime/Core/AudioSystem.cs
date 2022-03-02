using System;
using GameWarriors.AudioDomain.Abstraction;
using GameWarriors.AudioDomain.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace GameWarriors.AudioDomain.Core
{
    public class AudioSystem : IAudioEffect, IAudioLoop
    {
        private const string SFX_VOLUME_NAME = "SFXVolume";
        private const string LOOP_VOLUME_NAME = "LoopVolume";
        private const int LOOP_GROUP_COUNT = 3;
        [SerializeField] private AudioSource[] _onShotSource;
        private int _onShotCounter;
        private LoopAudioGroup[] _loopSource;

        private float _baseSfxVolume;
        private float _baseLoopVolume;
        private float _disableVolume;
        private float _fadeSpeed;
        private Dictionary<string, AudioClip> _clipTable;
        private AudioMixer _audioMixer;

        private bool _isSfxInit;

        public float SfxVolume
        {
            set
            {
                PlayerPrefs.SetFloat(SFX_VOLUME_NAME, value);
                _audioMixer.SetFloat(SFX_VOLUME_NAME, value);
            }
        }

        public float MusicVolume
        {
            set
            {
                PlayerPrefs.SetFloat(LOOP_VOLUME_NAME, value);
                _audioMixer.SetFloat(LOOP_VOLUME_NAME, value);
            }
        }


        public bool SfxEnableState
        {
            get
            {
                _audioMixer.GetFloat(SFX_VOLUME_NAME, out var volume);
                return volume == _baseSfxVolume;
            }
            set
            {
                var volume = value ? _baseSfxVolume : _disableVolume;
                SfxVolume = volume;
            }
        }

        public bool MusicEnableState
        {
            get
            {
                _audioMixer.GetFloat(LOOP_VOLUME_NAME, out var volume);
                return volume == _baseLoopVolume;
            }
            set
            {
                var volume = value ? _baseLoopVolume : _disableVolume;
                MusicVolume = volume;
            }
        }

        [UnityEngine.Scripting.Preserve]
        public AudioSystem()
        {
            IAudioResourceLoader audioResourceLoader = new DefaultResourceLoader();
            audioResourceLoader.LoadResourceAsync(AudioConfigData.RESOURCE_PATH, LoadComplete);
        }


        [UnityEngine.Scripting.Preserve]
        public async Task WaitForLoading()
        {
            while (_clipTable == null)
            {
                await Task.Delay(100);
            }
        }

        public void AddAudioClip(params AudioClip[] audioList)
        {
            int length = audioList.Length;
            for (int i = 0; i < length; ++i)
            {
                _clipTable.Add(audioList[i].name, audioList[i]);
            }
        }

        public void PlayEffect(string effectName, float volume = 1, float delay = 0)
        {
            if (_clipTable.TryGetValue(effectName, out var clip))
            {
                PlayEffect(clip, volume, delay);
            }
        }

        public void PlayEffect(AudioClip audioClip, float volume = 1, float delay = 0)
        {
            if (audioClip == null)
                return;
            if (!_isSfxInit)
            {
                _audioMixer.SetFloat(SFX_VOLUME_NAME, PlayerPrefs.GetFloat(SFX_VOLUME_NAME, _baseSfxVolume));
                _isSfxInit = true;
            }

            AudioSource source = GetSfxSource();
            source.clip = audioClip;
            source.volume = volume;
            if (delay <= 0)
                source.Play();
            else
                source.PlayDelayed(delay);
        }

        public void PlayLoop(string audioName, float volume = 1, EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1)
        {
            if (_clipTable.TryGetValue(audioName, out var clip))
            {
                PlayLoop(clip, volume);
            }
        }

        public void PlayLoop(AudioClip audioLoop, float volume = 1, EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1)
        {
            if (audioLoop == null)
                return;

            _audioMixer.SetFloat(LOOP_VOLUME_NAME, PlayerPrefs.GetFloat(LOOP_VOLUME_NAME, _baseLoopVolume));
            _loopSource[(int) loopGroup].Set(audioLoop, volume);
        }

        public void StopGroupLoopSource(EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1)
        {
            _loopSource[(int) loopGroup].Stop();
        }

        public void MuteGroupLoopSource(EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1)
        {
            _loopSource[(int) loopGroup].MuteFade();
        }

        public void UnmuteGroupLoopSource(EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1)
        {
            _loopSource[(int) loopGroup].UnmuteFade();
        }

        public void ChangeGroupLoopSourceVolume(EAudioLoopLayer loopGroup = EAudioLoopLayer.LoopGroup1, float targetVolume = 1)
        {
            _loopSource[(int) loopGroup].ChangeVolumeFade(targetVolume);
        }

        public float GetClipLength(string audioName)
        {
            if (_clipTable.TryGetValue(audioName, out var clip))
            {
                return clip.length;
            }

            return -1;
        }

        public void AudioUpdate()
        {
            for (int i = 0; i < LOOP_GROUP_COUNT; ++i)
            {
                if (_loopSource[i].IsUpdate)
                    _loopSource[i].FadeUpdate(_fadeSpeed);
            }
        }

        private void LoadComplete(AudioConfigData configData)
        {
            _baseSfxVolume = configData.BaseSfxVolume;
            _baseLoopVolume = configData.BaseLoopVolume;
            _disableVolume = configData.DisableVolume;
            _fadeSpeed = configData.FadeSpeed;
            int clipCount = configData.AudioClips?.Length ?? 0;
            _clipTable = new Dictionary<string, AudioClip>(clipCount + 10);
            for (int i = 0; i < clipCount; ++i)
            {
                if (configData.AudioClips == null) continue;
                var clip = configData.AudioClips[i];
                _clipTable.Add(clip.name, clip);
            }

            _audioMixer = configData.BaseAudioMixer;
            _audioMixer.SetFloat(LOOP_VOLUME_NAME, PlayerPrefs.GetFloat(LOOP_VOLUME_NAME, _baseLoopVolume));
            _audioMixer.SetFloat(SFX_VOLUME_NAME, PlayerPrefs.GetFloat(SFX_VOLUME_NAME, _baseSfxVolume));


            GameObject pillar = new GameObject("OnShotPillar");
            int length = configData.SfxSourceCount;
            length = Mathf.Max(1, length);
            _onShotSource = new AudioSource[length];
            for (int i = 0; i < length; ++i)
            {
                _onShotSource[i] = pillar.AddComponent<AudioSource>();
                _onShotSource[i].outputAudioMixerGroup = configData.SfxSource.outputAudioMixerGroup;
            }

            pillar = new GameObject("LoopPillar");
            _loopSource = new LoopAudioGroup[length];
            for (int i = 0; i < LOOP_GROUP_COUNT; ++i)
            {
                AudioSource source1 = pillar.AddComponent<AudioSource>();
                source1.outputAudioMixerGroup = configData.LoopSource.outputAudioMixerGroup;
                AudioSource source2 = pillar.AddComponent<AudioSource>();
                source2.outputAudioMixerGroup = configData.LoopSource.outputAudioMixerGroup;
                _loopSource[i] = new LoopAudioGroup(source1, source2);
            }
        }

        private AudioSource GetSfxSource()
        {
            AudioSource source = _onShotSource[_onShotCounter];
            ++_onShotCounter;
            _onShotCounter %= _onShotSource.Length;
            return source;
        }
    }
}