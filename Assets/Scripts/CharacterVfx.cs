using System;
using System.Collections;
using UnityEngine;

public class CharacterVfx : MonoBehaviour
{
    [field: SerializeField] private SpriteRenderer Renderer { get; set; }
    [field: SerializeField][field: ColorUsage(true, true)] private Color DamageFlashColor { get; set; }
    [field: Header ("Damage Flash")]
    [field: SerializeField] private float DamageFlashDuration { get; set; } = .1f; 
    private Action FlashCallback { get; set; }
    private Coroutine FlashCoroutine { get; set; }
    private Coroutine FlashLoopCoroutine { get; set; }

    private const string lerpProp = "_OverrideColorLerp";

    public void DamageFlash(Action callback)
    {
        Flash(DamageFlashColor, DamageFlashDuration, callback);
    }

    internal void Flash(Color color, float duration, Action callback)
    {
        StopFlashCoroutine();
        FlashCallback = callback;
        FlashCoroutine = StartCoroutine(Flash(color, duration));
    }

    private void StopFlashCoroutine()
    {
        if (FlashCoroutine != null)
        {
            StopCoroutine(FlashCoroutine);
            InvokeFlashCallback();
        }
    }

    private IEnumerator Flash(Color flashColor, float duration, float flashSpeed = 7.5f)
    {
        if (FlashLoopCoroutine != null)
            StopCoroutine(FlashLoopCoroutine);

        FlashLoopCoroutine = StartCoroutine(FlashLoop(flashColor, flashSpeed));
        yield return new WaitForSeconds(duration);
        Renderer.material.SetFloat(lerpProp, 0);
        StopCoroutine(FlashLoopCoroutine);

        InvokeFlashCallback();
    }

    private IEnumerator FlashLoop(Color flashColor, float flashSpeed)
    {
        var mat = Renderer.material;
        mat.SetColor("_OverrideColor", flashColor);

        mat.SetFloat(lerpProp, 0);

        while (true)
        {
            while (mat.GetFloat(lerpProp) < .65f)
            {
                var flashIntensity = mat.GetFloat(lerpProp);
                mat.SetFloat(lerpProp, flashIntensity += flashSpeed * Time.deltaTime);
                yield return null;
            }
            while (mat.GetFloat(lerpProp) > 0f)
            {
                var flashIntensity = mat.GetFloat(lerpProp);
                mat.SetFloat(lerpProp, flashIntensity -= flashSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    

    private void InvokeFlashCallback()
    {
        FlashCallback?.Invoke();
        FlashCallback = null;
    }
}
