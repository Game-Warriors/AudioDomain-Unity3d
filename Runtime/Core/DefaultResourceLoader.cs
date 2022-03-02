using GameWarriors.AudioDomain.Abstraction;
using GameWarriors.AudioDomain.Data;
using System;
using UnityEngine;

namespace GameWarriors.AudioDomain.Core
{
    public class DefaultResourceLoader : IAudioResourceLoader
    {
        public void LoadResourceAsync(string assetName, Action<AudioConfigData> onLoadDone)
        {
            var operation = Resources.LoadAsync<AudioConfigData>(assetName);
            operation.completed += (asyncOperation) => onLoadDone((asyncOperation as ResourceRequest)?.asset as AudioConfigData);
        }
    }
}