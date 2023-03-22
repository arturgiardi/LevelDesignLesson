using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    [field: SerializeField] SpriteRenderer SpriteRenderer { get; set; }
    [SerializeField] private Sprite[] _sprites;
    [field: SerializeField] private float Cooldown { get; set; }

    int index = 0;
    float time;


    private void Start()
    {
        index = 0;
        time = Cooldown;
    }

    void Update()
    {
        Animate();
    }

    private void Animate()
    {
        time += Time.deltaTime;
        if (time >= Cooldown)
        {
            time = 0;

            if (index >= _sprites.Length)
                index = 0;

            SpriteRenderer.sprite = _sprites[index];
            index++;
        }
    }
}