using UnityEngine;

public interface IDamageble
{
    void TakeDamage(int power, Vector2 impactPoint, float knockbackPower);
}