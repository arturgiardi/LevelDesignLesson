using System;
using UnityEngine;

public class EnemyController2 : BaseCharacterController, IDamageble
{
    [field: SerializeField] private EnemyCharacter2 Character { get; set; }
    [field: SerializeField] private CharacterAI2 Ai { get; set; }


    private EnemyStateMachineManager StateMachineManager { get; set; }

    public void Init(GameObject player)
    {
        Character.Init(this, player);
        Ai.Init(Character);
        ConfigStateMachine();
    }

    private void ConfigStateMachine()
    {
        StateMachineManager = new EnemyStateMachineManager();
        StateMachineManager.PushState(new EnemyStartState(Character, StateMachineManager, Ai));
    }

    private void Update() => StateMachineManager?.Update();

    public override void TakeDamage(int power, Vector2 impactPoint, float knockbackPower)
    {
        throw new NotImplementedException();
    }

    public override void Pause()
    {
        throw new NotImplementedException();
    }

    public override void DisPause()
    {
        throw new NotImplementedException();
    }
}

public class EnemyStateMachineManager : BaseStateMachineManager<EnemyBaseState>
{
    public void Update() => CurrentState?.Update();
}

public abstract class EnemyBaseState : BaseState
{
    protected EnemyCharacter2 Character { get; set; }
    protected EnemyStateMachineManager Manager { get; set; }
    protected CharacterAI2 Ai { get; set; }

    protected float elapsedTimeToRecalculatePath;
    private const float timeToRecalculatePath = .5f;
    public EnemyBaseState(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai)
    {
        Character = character;
        Manager = manager;
        Ai = ai;
    }

    public virtual void Update() { }

    protected void SearchPlayer()
    {
        if (Character.SawPlayer())
            Manager.SwapState(Ai.Behaviour.SawPlayer(Character, Manager, Ai));
    }

    protected void CalculatePath(Vector3 position)
    {
        Ai.CalculatePath(position);
    }

    protected void Move(float moveSpeed, Vector3 position)
    {
        Ai.Move(moveSpeed);
        elapsedTimeToRecalculatePath += Time.deltaTime;

        if (elapsedTimeToRecalculatePath >= 0.5f)
        {
            CalculatePath(position);
            elapsedTimeToRecalculatePath = 0;
        }
    }
}

public class EnemyStartState : EnemyBaseState
{
    public EnemyStartState(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai) : base(character, manager, ai)
    {
    }

    public override void Init()
    {
        Manager.SwapState(Ai.Behaviour.GetStartState(Character, Manager, Ai));
    }
}
public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai) : base(character, manager, ai)
    {
    }

    public override void Init()
    {
        Manager.SwapState(new EnemyPatrolWaitState(Character, Manager, Ai));
    }
}

public class EnemyPatrolWaitState : EnemyBaseState
{
    private float waitTime;
    public EnemyPatrolWaitState(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai) : base(character, manager, ai)
    {
    }

    public override void Init()
    {
        waitTime = Ai.WaitTime;
    }
    public override void Update()
    {
        SearchPlayer();
        Wait();
    }

    private void Wait()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
            Manager.SwapState(new EnemyPatrolWalkState(Character, Manager, Ai));
    }
}

public class EnemyPatrolWalkState : EnemyBaseState
{
    private Vector3 destination;

    public EnemyPatrolWalkState(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai) : base(character, manager, ai)
    {
    }

    public override void Init()
    {
        destination = Ai.PatrolDestination;
        CalculatePath(destination);
        elapsedTimeToRecalculatePath = 0;
    }

    public override void Update()
    {
        Move(Ai.PatrolSpeed, destination);
        SearchPlayer();
        CheckIfCharacterReachedDestination();
    }

    private void CheckIfCharacterReachedDestination()
    {
        if (Ai.ReachedDestination(destination))
            Manager.SwapState(new EnemyPatrolWaitState(Character, Manager, Ai));
    }

    public override void Exit()
    {
        Character.StopMovement();
    }
}

public class EnemyChaseState : EnemyBaseState
{
    private Transform player;

    public EnemyChaseState(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai) : base(character, manager, ai)
    {
    }

    public override void Init()
    {
        player = Character.Player.transform;
        CalculatePath(player.position);
        elapsedTimeToRecalculatePath = 0;
    }

    public override void Update()
    {
        Move(Ai.ChaseSpeed, player.position);
        ChecKTargetDistance();
    }

    private void ChecKTargetDistance()
    {
        var distanceToReachTarget = Ai.ChanceDistanceToReach;
        var distanceToLoseTarget = Ai.ChanceDistanceToLose;

        (bool isFar, bool isClose) = Ai.CheckTargetDistance(distanceToReachTarget,
                    distanceToLoseTarget, player.position);

        if (isClose)
            Manager.SwapState(Ai.Behaviour.ChasePlayerSuccessful(Character, Manager, Ai));
        else if (isFar)
            Manager.SwapState(Ai.Behaviour.StopChasePlayer(Character, Manager, Ai));
    }

    public override void Exit()
    {
        Character.StopMovement();
    }
}

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai) : base(character, manager, ai)
    {
    }

    public override void Init()
    {
        Character.LookTo(Character.Player.transform.position);
        Character.Attack(() => Manager.SwapState(new EnemyCooldownState(Character, Manager, Ai, .5f)));
    }
}

public class EnemyCooldownState : EnemyBaseState
{
    private float waitTime;
    public EnemyCooldownState(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai, float cooldownTime) : base(character, manager, ai)
    {
    }

    public override void Init()
    {
        waitTime = Ai.WaitTime;
    }
    public override void Update()
    {
        Wait();
    }

    private void Wait()
    {
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
            Manager.SwapState(new EnemyPatrolWalkState(Character, Manager, Ai));
    }
}

