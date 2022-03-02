using GameWarriors.AudioDomain.Data;
using System;

namespace GameWarriors.AudioDomain.Abstraction
{
    public interface IAudioResourceLoader
    {
        void LoadResourceAsync(string assetName, Action<AudioConfigData> onLoadDone);
    }
}