using UnityEngine;

public class Bullet : MonoBehaviour
{
    [field: SerializeField] private Rigidbody2D Rigidbody { get; set; }
    [field: SerializeField] private float Speed { get; set; }
    [field: SerializeField] private int Damage { get; set; }
    [field: SerializeField] private float KnockbackPower { get; set; }

    public void Shot(Vector2 direction)
    {
        Rigidbody.velocity = direction * Speed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var damageble = other.GetComponent<IDamageble>();
        if(damageble != null)
            damageble.TakeDamage(Damage, transform.position, KnockbackPower);

        Destroy(gameObject);
    }
}
