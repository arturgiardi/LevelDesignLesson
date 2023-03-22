using System;
using DG.Tweening;
using UnityEngine;

namespace GEngine.Controller
{
    public class AudioSourceController : MonoBehaviour
    {
        [field: SerializeField] private AudioSource AudioSource {get; set;}
        float _maxVolume = 1;
        public AudioClip AudioClip => AudioSource.clip;

        public float Volume
        {
            get => AudioSource.volume / MaxVolume;
            set => AudioSource.volume = value * MaxVolume;
        }

        public float MaxVolume
        {
            get => _maxVolume;
            set
            {
                _maxVolume = value;
                AudioSource.volume = AudioSource.volume * MaxVolume;
            }
        }

        public void PlayAudio(AudioClip audioClip, float volume, float pitch, bool loop)
        {
            if (audioClip)
            {
                AudioSource.volume = MaxVolume * volume;
                AudioSource.pitch = pitch;
                AudioSource.loop = loop;

                if (audioClip != AudioSource.clip || !AudioSource.isPlaying)
                {
                    AudioSource.clip = audioClip;
                    AudioSource.Play();
                }
            }
            else
            {
                AudioSource.Stop();
                AudioSource.clip = null;
            }
        }
        public void ChangeVolume(float volume, float time, Action onComplete = null)
        {
            AudioSource.DOFade(volume * MaxVolume, time).OnComplete(() => onComplete?.Invoke());
        }

        public void FadeAndPlayNewAudio(AudioClip audioClip, float timeToFade, Action onComplete, float volume, float pitch, bool loop)
        {
            if (audioClip == AudioSource.clip)
            {
                PlayAudio(audioClip, volume, pitch, loop);
                onComplete?.Invoke();
                return;
            }
            ChangeVolume(0, timeToFade, () =>
            {
                PlayAudio(audioClip, volume, pitch, loop);
                onComplete?.Invoke();
            });
        }

        public void FadeAndStop(float timeToFade, Action onComplete)
        {
            var volume = AudioSource.volume;
            ChangeVolume(0, timeToFade, () =>
            {
                AudioSource.Stop();
                AudioSource.volume = volume;
                onComplete?.Invoke();
            });
        }
    }
}
