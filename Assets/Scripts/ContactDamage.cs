using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [field: SerializeField] private int DamageValue { get; set; } = 1;
    [field: SerializeField] private int KnockbackPower { get; set; } = 10;
    private GameObject Player { get; set; }

    public void Init(GameObject player)
    {
        Player = player;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == Player)
        {
            Player.GetComponent<IDamageble>().TakeDamage(DamageValue, transform.position, KnockbackPower);
        }
    }
}
