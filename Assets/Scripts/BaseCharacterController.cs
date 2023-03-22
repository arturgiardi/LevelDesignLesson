using UnityEngine;

public abstract class BaseCharacterController : MonoBehaviour, IDamageble
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public CharacterAI AI { get; set; }
    public abstract void TakeDamage(int power, Vector2 impactPoint, float knockbackPower);
    public abstract void Pause();
    public abstract void DisPause();

    internal void Destroy()
    {
        Destroy(gameObject);
    }
}

