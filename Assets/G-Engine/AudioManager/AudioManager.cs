using System;
using GEngine.Controller;
using UnityEngine;

namespace GEngine.Manager
{
    public class AudioManager : MonoBehaviour
    {
        [field: SerializeField] private Transform SfxParent { get; set; }
        [SerializeField] private AudioControllerData[] _audioControllers;
        public float MaxSfxVolume { get; set; } = 1;

        #region SFX
        public void PlaySFX(AudioClip clip, float volume = 1, float pitch = 1, bool unique = true)
        {
            if (!clip)
                return;

            string goName = $"{clip.name}{Time.time}";

            if (unique && CheckIfSFXExists(goName))
                return;

            GameObject goTemp = new GameObject();
            goTemp.transform.SetParent(SfxParent);
            goTemp.name = goName;
            AudioSource audioTemp = goTemp.AddComponent<AudioSource>();

            audioTemp.pitch = pitch;
            audioTemp.volume = MaxSfxVolume * volume;
            audioTemp.clip = clip;
            audioTemp.Play();
            Destroy(goTemp, clip.length + 0.1f);
        }

        private bool CheckIfSFXExists(string name)
        {
            for (int i = 0; i < SfxParent.childCount; i++)
            {
                if (SfxParent.GetChild(i).gameObject.name == name)
                    return true;
            }
            return false;
        }
        #endregion

        #region AudioSourceController

        private AudioSourceController GetAudioSource(GameAudioType type)
        {
            if (type == GameAudioType.Unknown)
                throw new InvalidOperationException($"Tipo inválido {type}");
            foreach (var item in _audioControllers)
            {
                if (item.Type == type)
                    return item.Controller;
            }
            throw new InvalidOperationException($"Tipo inválido {type}");
        }

        public void PlayAudio(GameAudioType type, AudioClip audioClip, float volume = 1,
            float pitch = 1, bool loop = true)
        {
            GetAudioSource(type).PlayAudio(audioClip, volume, pitch, loop);
        }

        public void ChangeVolume(GameAudioType type, float volume, float time, Action onComplete = null)
        {
            GetAudioSource(type).ChangeVolume(volume, time, onComplete);
        }

        public void FadeAndPlayNewAudio(GameAudioType type, AudioClip audioClip, float timeToFade,
            Action onComplete = null, float volume = 1, float pitch = 1, bool loop = true)
        {
            GetAudioSource(type).FadeAndPlayNewAudio(audioClip, timeToFade, onComplete, volume, pitch, loop);
        }

        public void FadeAndStop(GameAudioType type, float timeToFade, Action onComplete = null)
        {
            GetAudioSource(type).FadeAndStop(timeToFade, onComplete);
        }
        #endregion

        [System.Serializable]
        private class AudioControllerData
        {
            [field: SerializeField] public GameAudioType Type { get; set; }
            [field: SerializeField] public AudioSourceController Controller { get; set; }
        }
    }
    public enum GameAudioType { Unknown, Bgm, Bgs }
}


