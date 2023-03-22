using System;
using UnityEngine;

public class PlayerController : BaseCharacterController
{
    [field: SerializeField] public PlayerCharacter PlayerCharacter { get; set; }

    private InputManager InputManager { get; set; }
    private PlayerStateMachineManager StateMachineManager { get; set; }

    public void Init(InputManager inputManager, Vector3 spawnPosition, Vector2 spawnDirection, PlayerData playerData)
    {
        PlayerCharacter.Init(this, spawnDirection, playerData);
        AI.Init(PlayerCharacter);
        InputManager = inputManager;
        ConfigStateMachine();
        EnableInputs();
        transform.position = spawnPosition;
    }

    private void ConfigStateMachine()
    {
        StateMachineManager = new PlayerStateMachineManager();
        StateMachineManager.PushState(new PlayerInitState(PlayerCharacter, StateMachineManager));
    }

    public void EnableControl() 
    {
        if(StateMachineManager.Contains<PlayerIdleState>())
            StateMachineManager.PopTo<PlayerIdleState>();
        else
            StateMachineManager.SwapState(new PlayerIdleState(PlayerCharacter, StateMachineManager));
    }

    public void StartPush(PushableObject pushableObject)
    {
        StateMachineManager.PushState(new PlayerPushState(PlayerCharacter, StateMachineManager, pushableObject));
    }

    public override void Pause()
    {
        StateMachineManager?.PushState(new PlayerPausedState(PlayerCharacter, StateMachineManager));
    }

    public override void DisPause()
    {
        StateMachineManager?.DisPause();
    }

    private void EnableInputs()
    {
        InputManager.RegisterMoveAction(Move);
        InputManager.RegisterAttackAction(Attack);
        InputManager.RegisterDodgeAction(Dodge);
        InputManager.RegisterInteractionStart(StartInteraction);
        InputManager.RegisterInteractionEnd(EndInteraction);
    }

    private void EndInteraction() => StateMachineManager?.EndInteraction();
    private void StartInteraction() => StateMachineManager?.StartInteraction(this);
    private void Move(Vector2 direction) => StateMachineManager?.Move(direction);
    private void Attack() => StateMachineManager?.Attack();
    private void Dodge(Vector2 direction) => StateMachineManager?.Dodge(direction);
    private void Update() => StateMachineManager?.Update();
    public override void TakeDamage(int power, Vector2 impactPoint, float knockbackPower) 
        => StateMachineManager?.OnDamageTaken(power, impactPoint, knockbackPower);


    private void OnDestroy()
    {
        DisableInputs();
    }

    private void DisableInputs()
    {
        InputManager?.UnregisterMoveAction(Move);
        InputManager?.UnregisterAttackAction(Attack);
        InputManager?.UnregisterDodgeAction(Dodge);
        InputManager?.UnregisterInteractionStart(StartInteraction);
        InputManager?.UnregisterInteractionEnd(EndInteraction);
    }

    public void DisableGameplay()
    {
        StateMachineManager?.SwapState(new PlayerStandByState(PlayerCharacter, StateMachineManager));
    }
}

