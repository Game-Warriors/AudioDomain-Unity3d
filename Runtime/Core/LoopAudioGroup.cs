using UnityEngine;

namespace GameWarriors.AudioDomain.Core
{
    public enum EFadeState
    {
        None,
        Default,
        Mute,
        Unmute
    }

    public class LoopAudioGroup
    {
        private AudioSource _audioSource1;
        private AudioSource _audioSource2;
        private float _targetVolume;
        private EFadeState _fadeState;

        public bool IsUpdate => _fadeState != EFadeState.None;

        public LoopAudioGroup(AudioSource source1, AudioSource source2)
        {
            source1.loop = true;
            source2.loop = true;
            _audioSource1 = source1;
            _audioSource2 = source2;
            _fadeState = EFadeState.None;
            _targetVolume = 1;
        }

        public void Set(AudioClip clip, float volume = 1)
        {
            _audioSource2.volume = 1 - _audioSource1.volume;
            _audioSource2.clip = clip;
            _targetVolume = volume;
            var tmp = _audioSource2;
            _audioSource2 = _audioSource1;
            _audioSource1 = tmp;
            _audioSource1.Play();
            _fadeState = EFadeState.Default;
        }

        public void FadeUpdate(float fadeSpeed)
        {
            switch (_fadeState)
            {
                case EFadeState.Default:
                    {
                        float startVolume = _audioSource1.volume;
                        float volume = Mathf.MoveTowards(startVolume, _targetVolume, fadeSpeed * Time.deltaTime);
                        _audioSource1.volume = Mathf.Abs(_audioSource1.volume - _targetVolume) > 0.05f
                            ? volume
                            : _targetVolume;

                        if (_audioSource2.volume > 0.05f)
                            _audioSource2.volume = 1 - volume;
                        else
                            _audioSource2.volume = 0;

                        if (_audioSource1.volume == _targetVolume && _audioSource2.volume == 0)
                            _fadeState = EFadeState.None;
                        break;
                    }
                case EFadeState.Mute:
                    {
                        _audioSource1.volume = Mathf.MoveTowards(_audioSource1.volume, 0, fadeSpeed * Time.deltaTime);
                        _audioSource2.volume = Mathf.MoveTowards(_audioSource2.volume, 0, fadeSpeed * Time.deltaTime);
                        if (_audioSource1.volume < 0.05f)
                        {
                            _fadeState = EFadeState.None;
                            _audioSource1.volume = 0;
                            _audioSource2.volume = 0;
                        }

                        break;
                    }
                case EFadeState.Unmute when Mathf.Abs(_audioSource1.volume - _targetVolume) > 0.05f:
                    _audioSource1.volume =
                        Mathf.MoveTowards(_audioSource1.volume, _targetVolume, fadeSpeed * Time.deltaTime);
                    break;
                case EFadeState.Unmute:
                    _audioSource1.volume = _targetVolume;
                    _fadeState = EFadeState.None;
                    break;
                case EFadeState.None:
                    break;
            }
        }

        public void Stop()
        {
            _audioSource1.Stop();
            _audioSource2.Stop();
        }

        public void MuteFade()
        {
            _fadeState = EFadeState.Mute;
        }

        public void UnmuteFade()
        {
            _fadeState = EFadeState.Unmute;
        }

        public void ChangeVolumeFade(float targetVolume)
        {
            _fadeState = EFadeState.Unmute;
            _targetVolume = targetVolume;
        }
    }
}