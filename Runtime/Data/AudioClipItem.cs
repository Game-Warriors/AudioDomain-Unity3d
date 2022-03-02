using UnityEngine;

namespace GameWarriors.AudioDomain.Data
{
    [System.Serializable]
    public struct AudioClipItem 
    {
        [SerializeField] private string _name;
        [SerializeField] private AudioClip _clip;
    }
}
