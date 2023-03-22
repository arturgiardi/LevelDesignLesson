using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField] private Collider2D Collider { get; set; }
    [field: SerializeField] public float AnimationTime { get; private set; }
    [SerializeField] private Sprite[] _closeAnimation;
    [SerializeField] private Sprite[] _openAnimation;

    public bool IsOpen { get; private set; }

    private Coroutine AnimationCoroutine { get; set; }

    public void InstantClose()
    {
        Collider.enabled = true;
        SpriteRenderer.sprite = _closeAnimation[_closeAnimation.Length - 1];
        IsOpen = false;
    }

    public void InstantOpen()
    {
        Collider.enabled = false;
        SpriteRenderer.sprite = _openAnimation[_openAnimation.Length - 1];
        IsOpen = true;
    }

    public void Open()
    {
        StopDoorAnimationCoroutine();
        IsOpen = true;
        AnimationCoroutine = StartCoroutine(
            AnimateDoor(AnimationTime, _openAnimation, () => Collider.enabled = false));
    }

    public void Close()
    {
        StopDoorAnimationCoroutine();
        IsOpen = false;
        Collider.enabled = true;
        AnimationCoroutine = StartCoroutine(AnimateDoor(AnimationTime, _openAnimation));
    }

    private IEnumerator AnimateDoor(float animationTime, Sprite[] animation, Action callback = null)
    {
        var timePerFrame = new WaitForSeconds(animationTime / animation.Length);
        int animationStep = GetAnimationStep(animation);
        for (int i = 0; i < animation.Length; i++)
        {
            if (animationStep > i)
                continue;

            SpriteRenderer.sprite = animation[i];
            yield return timePerFrame;
        }
        callback?.Invoke();
    }

    private int GetAnimationStep(Sprite[] animation)
    {
        for (int i = 0; i < animation.Length; i++)
        {
            if (animation[i] == SpriteRenderer.sprite)
                return i;
        }
        throw new InvalidOperationException($"Sprite não existe na animação {SpriteRenderer.sprite}");
    }

    private void StopDoorAnimationCoroutine()
    {
        if (AnimationCoroutine != null)
            StopCoroutine(AnimationCoroutine);
    }
}
