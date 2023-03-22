using UnityEngine;

[System.Serializable]
public class DamageAreaController
{
    [field: SerializeField] private LayerMask LayerMask { get; set; }
    public void DoDamage(Vector3 attackerPosition, Vector3 point, int power, float area, float knockbackPower)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(point, area, LayerMask);

        foreach (var item in hits)
        {
            var hitObj = item.GetComponent<IDamageble>();
            if (hitObj != null)
            {
                hitObj.TakeDamage(power, attackerPosition, knockbackPower);
            }
        }
    }
}