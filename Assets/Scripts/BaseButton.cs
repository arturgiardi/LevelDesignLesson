using UnityEngine;
using UnityEngine.Events;

public abstract class BaseButton : MonoBehaviour
{
    [field: SerializeField] protected SpriteRenderer SpriteRenderer { get; set; }
    [field: SerializeField] protected Sprite PressedSprite { get; set; }
    [field: SerializeField] protected UnityEvent OnClick { get; set; }
    public bool IsPressed => SpriteRenderer.sprite == PressedSprite;

}
