using System;
using UnityEngine;

public class EnemyController : BaseCharacterController, IDamageble
{
    [field: SerializeField] private EnemyCharacter Character { get; set; }
    [field: SerializeField] private Animator StateMachine { get; set; }
    public Transform Player { get; private set; }
    private Action<EnemyController> OnDestroyCallback;

    public void Init(GameObject player, Action<EnemyController> onDestroyCallback)
    {
        Player = player.transform;
        OnDestroyCallback = onDestroyCallback;
        Character.Init(this, player);
        AI.Init(Character);
    }

    public override void TakeDamage(int power, Vector2 impactPoint, float knockbackPower) => Character.TakeDamage(power, impactPoint, knockbackPower, null);
    public void EnableBehaviour() => StateMachine?.SetTrigger("EnableBehaviour");
    public void DisableBehaviour() => StateMachine?.SetTrigger("DisableBehaviour");
    public override void Pause() => Character.Pause();
    public override void DisPause() => Character.DisPause();

    private void OnDestroy()
    {
        OnDestroyCallback.Invoke(this);
    }
}

