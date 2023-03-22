using UnityEngine;

public class ClickButton : BaseButton
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (SpriteRenderer.sprite == PressedSprite)
            return;
        SpriteRenderer.sprite = PressedSprite;
        OnClick.Invoke();
    }

    public void SetPressed()
    {
        SpriteRenderer.sprite = PressedSprite;
    }
}