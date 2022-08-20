using System;

namespace GameWarriors.AudioDomain.Abstraction
{
    public interface IAudioEventHandler
    {
        void RegisterUpdate(Action audioUpdate);
    }
}