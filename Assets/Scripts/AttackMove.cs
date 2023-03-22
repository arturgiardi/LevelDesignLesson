using UnityEngine;

public abstract class AttackMove : ScriptableObject
{
    [field: SerializeField] public int Power { get; set; } = 1;
    [field: SerializeField] public int Area { get; set; } = 1;
    [field: SerializeField] public float Distance { get; set; } = 0.5f;
    [field: SerializeField] public float KnockbackPower { get; set; } = 0f;
}