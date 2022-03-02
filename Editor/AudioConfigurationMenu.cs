using GameWarriors.AudioDomain.Data;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace GameWarriors.AudioDomain.Editor
{

    public class AudioConfigurationMenu : ScriptableWizard
    {
        [SerializeField,Range(0,1)] private float _baseSfxVolume;
        [SerializeField, Range(0, 1)] private float _baseLoopVolume;
        [SerializeField] private float _disableVolume;
        [SerializeField] private float _fadeSpeed;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _loopSourceSample;
        [SerializeField] private int _sfxSourceCount;
        [SerializeField] private AudioSource _sfxSourceSample;
        [SerializeField] private AudioClip[] _clipItems;

        [MenuItem("Tools/Audio Configuration")]
        private static void OpenBuildConfigWindow()
        {
            if (!Directory.Exists("Assets/AssetData/Resources"))
                Directory.CreateDirectory("Assets/AssetData/Resources");

            AudioConfigurationMenu tmp = DisplayWizard<AudioConfigurationMenu>("Audio Configuration", "Save");
            tmp.Initialization();
        }

        private void Initialization()
        {
            AudioConfigData asset = AssetDatabase.LoadAssetAtPath<AudioConfigData>(AudioConfigData.ASSET_PATH);
            if (asset != null)
            {
                _baseSfxVolume = asset.BaseSfxVolume;
                _baseLoopVolume = asset.BaseLoopVolume;
                _disableVolume = asset.DisableVolume;
                _fadeSpeed = asset.FadeSpeed;
                _clipItems = asset.AudioClips;
                _audioMixer = asset.BaseAudioMixer;
                _loopSourceSample = asset.LoopSource;
                _sfxSourceSample = asset.SfxSource;
                _sfxSourceCount = asset.SfxSourceCount;
            }
        }

        private void OnWizardCreate()
        {
            AudioConfigData asset = AssetDatabase.LoadAssetAtPath<AudioConfigData>(AudioConfigData.ASSET_PATH);
            if (asset != null)
            {
                asset.SetClips(_clipItems);
                asset.SetVolumeAndMixer(_baseLoopVolume, _baseSfxVolume, _disableVolume, _audioMixer, _fadeSpeed);
                asset.SetAudioSource(_loopSourceSample, _sfxSourceSample, _sfxSourceCount);
                EditorUtility.SetDirty(asset);
            }
            else
            {
                asset = ScriptableObject.CreateInstance<AudioConfigData>();
                asset.SetClips(_clipItems);
                asset.SetVolumeAndMixer(_baseLoopVolume, _baseSfxVolume, _disableVolume, _audioMixer, _fadeSpeed);
                asset.SetAudioSource(_loopSourceSample, _sfxSourceSample, _sfxSourceCount);
                AssetDatabase.CreateAsset(asset, AudioConfigData.ASSET_PATH);
            }
            AssetDatabase.SaveAssets();
        }

    }
}

