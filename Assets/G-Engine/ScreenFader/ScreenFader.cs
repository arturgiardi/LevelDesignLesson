using DG.Tweening;
using System;
using UnityEngine;

namespace GEngine.Manager
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] CanvasGroup fadeCanvasGroup;

        public void FadeIn(float time, Action callback = null) => Fade(time, 0, callback);
        public void FadeOut(float time, Action callback = null) => Fade(time, 1, callback);

        private void Fade(float time, float value, Action callback)
        {
            fadeCanvasGroup.DOComplete();
            fadeCanvasGroup.DOKill();
            fadeCanvasGroup.DOFade(value, time).OnComplete(() => callback?.Invoke());
        }

        void OnDestroy()
        {
            fadeCanvasGroup.DOKill();
        }
    }
}

