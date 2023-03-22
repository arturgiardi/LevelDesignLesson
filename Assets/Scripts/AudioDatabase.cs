using System;
using UnityEngine;

namespace GEngine.Database
{
    [CreateAssetMenu(menuName = "ScriptableObject/AudioDatabase", fileName = "AudioDatabase")]

    public class AudioDatabase : ScriptableObject
    {
        [field: SerializeField] private AudioData[] Audios { get; set; }

        public AudioClip GetAudio(AudioId id)
        {
            if(id == AudioId.None)
                return null;

            foreach (var item in Audios)
            {
                if(item.Id == id)
                    return item.AudioClip;
            }
            throw new InvalidOperationException($"Id inválido {id}");
        }
    }

    [System.Serializable]
    public class AudioData
    {
        [field: SerializeField] public AudioId Id { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
    }

    public enum AudioId
    {
        None = 0,
    }
}
