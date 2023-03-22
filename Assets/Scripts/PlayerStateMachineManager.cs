using System;
using UnityEngine;

public class PlayerStateMachineManager : BaseStateMachineManager<PlayerBaseState>
{
    public void Move(Vector2 moveInput) => CurrentState?.Move(moveInput);
    public void Attack() => CurrentState?.Attack();
    public void Dodge(Vector2 direction) => CurrentState?.Dodge(direction);
    public void Update()
    {
        CurrentState?.Update();
    }
    public void StartInteraction(PlayerController playerController) => CurrentState?.StartInteraction(playerController);
    public void EndInteraction() => CurrentState?.EndInteraction();
    public void OnDamageTaken(int power, Vector3 impactPoint, float knockbackPower) 
        => CurrentState?.OnDamageTaken(power, impactPoint, knockbackPower);

    public void DisPause() => CurrentState?.Dispause();
    public void Heal(int healAmount) => CurrentState?.Heal(healAmount);
}
