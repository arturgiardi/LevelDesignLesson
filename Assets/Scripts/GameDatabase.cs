using UnityEngine;

namespace GEngine.Database
{
    [System.Serializable]
    public class GameDatabase
    {
        [field: SerializeField] private AudioDatabase AudioDatabase {get; set;}

        public AudioClip GetAudio(AudioId id) => AudioDatabase.GetAudio(id);
    }
}
