using UnityEngine;
using UnityEngine.Events;

public class TriggerEventZone : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnter;
    private string playerTag = "Player";

    GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
            onEnter?.Invoke();
    }
}
