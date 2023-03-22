using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GEngine.Manager
{
    public class ScreenFlash : MonoBehaviour, IManager
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] Image image;

        public void Flash(Color color, float duration, System.Action callback = null)
        {
            image.color = color;

            canvasGroup.DOComplete();
            canvasGroup.DOKill();
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, duration).OnComplete(() => callback?.Invoke());
        }
    }
}
