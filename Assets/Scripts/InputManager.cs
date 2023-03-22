using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Inputs Inputs { get; set; }
    private Action<Vector2> MoveAction { get; set; }
    private Action AttackAction { get; set; }
    private Action<Vector2> DodgeAction { get; set; }
    private Action InteractionStart { get; set; }
    private Action InteractionEnd { get; set; }
    private Action PauseAction { get; set; }

    private void Awake()
    {
        Inputs = new Inputs();
        Inputs.Player.Enable();
        Inputs.Player.Attack.performed += AttackPerformed;
        Inputs.Player.Dodge.performed += DodgePerformed;

        Inputs.Player.Interaction.performed += InteractionPerformed;
        Inputs.Player.Interaction.canceled += InteractionCanceled;
        Inputs.Player.Pause.performed += PausePerformed;
    }

    private void Update() 
    {
        MoveAction?.Invoke(Inputs.Player.Move.ReadValue<Vector2>());
    }

    private void InteractionCanceled(InputAction.CallbackContext context) => InteractionEnd?.Invoke();
    private void InteractionPerformed(InputAction.CallbackContext context) => InteractionStart?.Invoke();
    private void AttackPerformed(InputAction.CallbackContext context) => AttackAction?.Invoke();
    private void DodgePerformed(InputAction.CallbackContext context) => DodgeAction?.Invoke(Inputs.Player.Move.ReadValue<Vector2>());
    private void PausePerformed(InputAction.CallbackContext context) => PauseAction?.Invoke();

    public void RegisterMoveAction(Action<Vector2> action) => MoveAction += action;
    public void RegisterAttackAction(Action action) => AttackAction += action;
    public void RegisterDodgeAction(Action<Vector2> action) => DodgeAction += action;
    public void RegisterInteractionStart(Action action) => InteractionStart += action;
    public void RegisterInteractionEnd(Action action) => InteractionEnd += action;
    public void RegisterPauseAction(Action action) => PauseAction += action;

    public void UnregisterMoveAction(Action<Vector2> action) => MoveAction -= action;
    public void UnregisterAttackAction(Action action) => AttackAction -= action;
    public void UnregisterDodgeAction(Action<Vector2> action) => DodgeAction -= action;
    public void UnregisterInteractionStart(Action action) => InteractionStart -= action;
    public void UnregisterInteractionEnd(Action action) => InteractionEnd -= action;
    public void UnregisterPauseAction(Action action) => PauseAction -= action;

    private void OnDestroy()
    {
        Inputs?.Disable();
    }
}
