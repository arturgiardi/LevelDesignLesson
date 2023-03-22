using System;
using UnityEngine;

public abstract class PlayerBaseState : BaseState
{
    protected PlayerCharacter Player { get; set; }
    protected PlayerStateMachineManager Manager { get; set; }
    public PlayerBaseState(PlayerCharacter player, PlayerStateMachineManager manager)
    {
        Player = player;
        Manager = manager;
    }

    public virtual void Move(Vector2 moveInput) { }
    public virtual void Attack() { }
    public virtual void Dodge(Vector2 direction) { }
    public virtual void Update() { }
    public virtual void StartInteraction(PlayerController playerController) { }
    public virtual void EndInteraction() { }
    public virtual void OnDamageTaken(int power, Vector3 impactPoint, float knockbackPower) { }
    public virtual void Dispause() {}
    public virtual void Heal(int healAmount) {}

    protected void TakeDamage(int power, Vector3 impactPoint, float knockbackPower)
    {
        if (!Player.IsInvencible)
            Manager.PushState(new PlayerDamageState(Player, Manager, power, impactPoint, knockbackPower));
    }

    
}

public class PlayerInitState : PlayerBaseState
{
    public PlayerInitState(PlayerCharacter player, PlayerStateMachineManager manager) : base(player, manager)
    {
    }
}

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerCharacter player, PlayerStateMachineManager manager) : base(player, manager)
    {
    }

    public override void Init()
    {
        Player.Move(Vector2.zero, 0);
    }

    public override void Update()
    {
        Player.Update();
    }

    public override void Move(Vector2 moveInput)
    {
        Player.SetDirection(moveInput);
        Player.Move(moveInput, Player.MoveSpeed);
    }

    public override void Attack()
    {
        Manager.PushState(new PlayerAttackState(Player, Manager));
    }

    public override void Dodge(Vector2 direction)
    {
        Manager.PushState(new PlayerDodgeState(Player, Manager, direction));
    }

    public override void StartInteraction(PlayerController playerController)
    {
        Player.StartInteraction(playerController);
    }

    public override void OnDamageTaken(int power, Vector3 impactPoint, float knockbackPower)
    {
        TakeDamage(power, impactPoint, knockbackPower);
    }

    public override void Heal(int healAmount) 
    {
        Player.Heal(healAmount);
    }
}

public class PlayerDamageState : PlayerBaseState
{
    private int Power { get; set; }
    private Vector3 ImpactPoint { get; set; }
    private float KnockbackPower { get; set; }

    public PlayerDamageState(PlayerCharacter player, PlayerStateMachineManager manager,
        int power, Vector3 impactPoint, float knockbackPower) : base(player, manager)
    {
        Player = player;
        Manager = manager;
        this.Power = power;
        this.ImpactPoint = impactPoint;
        this.KnockbackPower = knockbackPower;
    }

    public override void Init()
    {
        Player.TakeDamage(Power, ImpactPoint, KnockbackPower, () => Manager.PopTo<PlayerIdleState>());
    }
}

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerCharacter player, PlayerStateMachineManager manager) : base(player, manager)
    {
    }

    public override void Init()
    {
        Player.Move(Vector2.zero, 0);
        PerformAttack();
    }

    private void PerformAttack()
    {
        Player.Attack(() => Manager.PopState());
    }

    public override void OnDamageTaken(int power, Vector3 impactPoint, float knockbackPower)
    {
        TakeDamage(power, impactPoint, knockbackPower);
    }
}

public class PlayerDodgeState : PlayerBaseState
{
    private Vector2 MoveDirection { get; set; }
    public PlayerDodgeState(PlayerCharacter player, PlayerStateMachineManager manager, Vector2 moveDirection) : base(player, manager)
    {
        MoveDirection = moveDirection;
    }

    public override void Init()
    {
        Player.Move(Vector2.zero, 0);
        if (MoveDirection == Vector2.zero)
            MoveDirection = Player.Direction;
        PerformDodge();
    }

    private void PerformDodge()
    {
        Player.SetInvencible(.15f);
        Player.Dodge(MoveDirection, () => Manager.PopState());
    }

    public override void OnDamageTaken(int power, Vector3 impactPoint, float knockbackPower)
    {
        TakeDamage(power, impactPoint, knockbackPower);
    }
}

public class PlayerPushState : PlayerBaseState
{
    private enum MoveDirection { X, Y }
    private MoveDirection Direction { get; set; }
    private PushableObject PushableObject { get; set; }
    public PlayerPushState(PlayerCharacter player, PlayerStateMachineManager manager, PushableObject pushableObject) : base(player, manager)
    {
        PushableObject = pushableObject;
    }

    public override void Init()
    {
        SetMoveDirection();
        Player.StartPush();
    }

    private void SetMoveDirection()
    {
        if (Player.Direction == Vector2.right || Player.Direction == Vector2.left)
            Direction = MoveDirection.X;
        else if (Player.Direction == Vector2.up || Player.Direction == Vector2.down)
            Direction = MoveDirection.Y;
        else
            throw new InvalidOperationException("Direção inválida");
    }

    public override void Move(Vector2 moveDirection)
    {
        moveDirection = ConvertToDirection(moveDirection);
        Player.PushMove(moveDirection, PushableObject);
    }

    public override void EndInteraction()
    {
        PushableObject.Move(Vector2.zero, 0);
        Manager.PopState();
        Player.EndPush();
    }

    private Vector2 ConvertToDirection(Vector2 moveInput)
    {
        var x = moveInput.x;
        var y = moveInput.y;

        if (Direction == MoveDirection.X)
            y = 0;
        else if (Direction == MoveDirection.Y)
            x = 0;
        else
            throw new InvalidOperationException("Direção inválida");

        return new Vector2(x, y);
    }

    public override void OnDamageTaken(int power, Vector3 impactPoint, float knockbackPower)
    {
        EndInteraction();
        TakeDamage(power, impactPoint, knockbackPower);
    }
}

public class PlayerStandByState : PlayerBaseState
{
    public PlayerStandByState(PlayerCharacter player, PlayerStateMachineManager manager) : base(player, manager)
    {
    }

    public override void Init()
    {
        Player.StopMovement();
    }
}

public class PlayerPausedState : PlayerBaseState
{
    public PlayerPausedState(PlayerCharacter player, PlayerStateMachineManager manager) : base(player, manager)
    {
    }

    public override void Init()
    {
        Player.Pause();
    }

    public override void Dispause()
    {
        Player.DisPause();
        Manager.PopState();
    }
}
