using UnityEngine;
using UnityEngine.Events;

public class EventArea : MonoBehaviour
{
    [field: SerializeField] private UnityEvent OnPlayerTouch { get; set; }
    private GameObject Player { get; set; }
    private GameplayScene _scene;
    public void Init(GameObject player)
    {
        Player = player;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Player)
        {
            OnPlayerTouch.Invoke();
        }
    }
}
