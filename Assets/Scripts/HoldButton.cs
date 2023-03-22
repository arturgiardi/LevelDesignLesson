using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoldButton : BaseButton
{
    [field: SerializeField] protected UnityEvent OnRelease { get; set; }
    [field: SerializeField] private Sprite ReleasedSprite { get; set; }

    private List<GameObject> collidingObjects = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!collidingObjects.Contains(other.gameObject))
            collidingObjects.Add(other.gameObject);

        if (SpriteRenderer.sprite == PressedSprite)
            return;
        SpriteRenderer.sprite = PressedSprite;
        OnClick.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(collidingObjects.Contains(other.gameObject))
            collidingObjects.Remove(other.gameObject);

        if(collidingObjects.Count > 0)
            return;
        
        SpriteRenderer.sprite = ReleasedSprite;
        OnRelease.Invoke();
    }

    public void SetPressed()
    {
        SpriteRenderer.sprite = PressedSprite;
    }
}
