using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuComponentAnimator : MonoBehaviour
{
    [field: SerializeField] private bool AnimateScale { get; set; }
    [field: SerializeField] private Vector2 MovementAnimation { get; set; }
    [field: SerializeField] private bool AnimateFade;
    [field: SerializeField] private float EndDelay { get; set; } = 0;

    private CanvasGroup canvasGroup;
    private Vector3 openScale;
    private Vector3 localPosition;
    private RectTransform rect;
    private float alpha;
    private Tween tween;
    public void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        openScale = rect.localScale;
        localPosition = rect.localPosition;
        alpha = canvasGroup.alpha;

        if (AnimateFade)
            canvasGroup.alpha = 0;

        if (AnimateScale)
            rect.localScale = Vector3.zero;

        Vector2 initPos = rect.position;

        var canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        if (MovementAnimation == Vector2.left)
            initPos = new Vector2(canvas.offsetMin.x - canvas.offsetMax.x, initPos.y);
        else if (MovementAnimation == Vector2.right)
            initPos = new Vector2(canvas.offsetMax.x + rect.offsetMax.x, initPos.y);
        else if (MovementAnimation == Vector2.down)
            initPos = new Vector2(initPos.x, canvas.offsetMin.y - canvas.offsetMax.y);
        else if (MovementAnimation == Vector2.up)
            initPos = new Vector2(initPos.x, canvas.offsetMax.y + rect.offsetMax.y);

        rect.position = initPos;
    }

    public void Close(float time, Action callback = null)
    {
        Sequence seq = DOTween.Sequence();

        if (AnimateFade)
            seq.Insert(0, canvasGroup.DOFade(0, time));

        if (AnimateScale)
            seq.Insert(0, rect.DOScale(0, time));

        var canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        if (MovementAnimation == Vector2.left)
            seq.Insert(0, rect.DOLocalMoveX(canvas.offsetMin.x - canvas.offsetMax.x, time));
        else if (MovementAnimation == Vector2.right)
            seq.Insert(0, rect.DOLocalMoveX(canvas.offsetMax.x + rect.offsetMax.x, time));
        else if (MovementAnimation == Vector2.down)
            seq.Insert(0, rect.DOLocalMoveY(canvas.offsetMin.y - canvas.offsetMax.y, time));
        else if (MovementAnimation == Vector2.up)
            seq.Insert(0, rect.DOLocalMoveY(canvas.offsetMax.y + rect.offsetMax.y, time));

        seq.AppendInterval(EndDelay);

        PlaySequence(callback, seq);
    }

    private void KillTween()
    {
        if (tween != null)
            tween.Kill();
    }

    public void Open(float time, Action callback = null)
    {
        Sequence seq = DOTween.Sequence();

        if (AnimateFade)
            seq.Insert(0, canvasGroup.DOFade(alpha, time));

        if (AnimateScale)
            seq.Insert(0, rect.DOScale(openScale, time));

        seq.AppendInterval(EndDelay);

        Canvas canvas = GetComponentInParent<Canvas>();
        seq.Insert(0, rect.DOLocalMove(localPosition, time));
        PlaySequence(callback, seq);
    }

    private void PlaySequence(Action callback, Sequence seq)
    {
        KillTween();
        tween = seq;

        tween.Play().OnComplete(() =>
        {
            if (callback != null)
                callback.Invoke();
        });
    }

    public IEnumerator OpenCoroutine(float time)
    {
        yield return AnimCoroutine(time);
    }

    public IEnumerator CloseCoroutine(float time)
    {
        yield return AnimCoroutine(time, false);
    }

    private IEnumerator AnimCoroutine(float time, bool open = true)
    {
        bool canContinue = false;

        if (open)
            Open(time, () => canContinue = true);
        else
            Close(time, () => canContinue = true);

        while (!canContinue)
        {
            yield return null;
        }
    }

    private void OnDestroy()
    {
        KillTween();
    }
}
