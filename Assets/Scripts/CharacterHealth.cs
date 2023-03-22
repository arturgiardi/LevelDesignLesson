using System;
using UnityEngine;

[System.Serializable]
public class CharacterHealth
{
    [field: SerializeField] public int MaxHP { get; set; }
    [field: SerializeField] private HpCanvas HpCanvas { get; set; }

    public int CurrentHp { get; private set; }
    public bool IsDeath => CurrentHp == 0;
    private Action OnDeathCallback { get; set; }
    private Action OnTakeDamageCallback { get; set; }
    private Action OnHealCallback { get; set; }
    public float HpPercentage => (float)CurrentHp / (float)MaxHP;

    public void Init(Action onTakeDamageCallback, Action onHealCallback, Action onDeathCallback)
    {
        CurrentHp = MaxHP;
        OnDeathCallback += onDeathCallback;
        OnTakeDamageCallback += onTakeDamageCallback;
        OnHealCallback += onHealCallback;
        HpCanvas?.SetFill(HpPercentage);
    }
    public void SetHP(int maxHp, int currentHP)
    {
        MaxHP = maxHp;
        CurrentHp = currentHP;
        HpCanvas?.SetFill(HpPercentage);
    }

    public void AddHP(int amount)
    {
        if (IsDeath)
            return;

        if (amount > 0)
            Heal(amount);
        else if (amount < 0)
            TakeDamage(-amount);
        
        HpCanvas?.SetFill(HpPercentage);
    }

    private void TakeDamage(int amount)
    {
        CurrentHp = Mathf.Clamp(CurrentHp - amount, 0, MaxHP);
        OnTakeDamageCallback?.Invoke();
        if (IsDeath)
            Death();
    }

    private void Heal(int amount)
    {
        CurrentHp = Mathf.Clamp(CurrentHp + amount, 0, MaxHP);
        OnHealCallback?.Invoke();
    }

    public void Death()
    {
        OnDeathCallback?.Invoke();
    }
}