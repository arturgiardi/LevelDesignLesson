using System;
using GEngine.Util;
using UnityEngine;

public abstract class BaseCharacter
{
    [field: SerializeField] protected CharacterHealth Health { get; set; }
    [field: SerializeField] protected CharacterAnimatorController AnimatorController { get; set; }
    [field: SerializeField] protected Rigidbody2D Rigidbody { get; set; }
    [field: SerializeField] protected Collider2D Collider { get; set; }
    [field: SerializeField] protected DamageAreaController MeleeAttack { get; set; }
    [field: SerializeField] protected CharacterVfx Vfx { get; set; }
    public bool IsInvencible { get; protected set; }
    protected BaseCharacterController Controller { get; set; }
    public Vector3 BodyCenter => Collider.bounds.center;
    public Bounds BodyBounds => Collider.bounds;
    public Vector2 Direction => AnimatorController.Direction;
    public abstract bool SawPlayer();

    public void Move(Vector2 moveDirection, float moveSpeed)
    {
        Rigidbody.velocity = moveDirection * moveSpeed;
        AnimatorController.SetSpeed(Rigidbody.velocity);
    }

    public void StopMovement()
    {
        Rigidbody.velocity = Vector2.zero;
        AnimatorController.SetSpeed(Rigidbody.velocity);
    }

    public void LookTo(Vector3 position) =>
        SetDirection(((Vector2)position - Rigidbody.position).normalized);

    public void SetDirection(Vector2 value)
    {
        if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
        {
            if (value.x > 0)
                AnimatorController.SetDirection(Vector2.right);
            else if (value.x < 0)
                AnimatorController.SetDirection(Vector2.left);
        }
        else
        {
            if (value.y > 0)
                AnimatorController.SetDirection(Vector2.up);
            else if (value.y < 0)
                AnimatorController.SetDirection(Vector2.down);
        }
    }

    public void Attack(Action callback)
    {
        AnimatorController.SetAttackTrigger("Attack", callback);
    }

    public void DoMeleeDamage(AttackMove move)
    {
        var point = (Vector2)BodyCenter + (Direction * move.Distance);
        MeleeAttack.DoDamage(BodyCenter, point, move.Power, move.Area, move.KnockbackPower);
    }
    public void TakeDamage(int power, Vector2 impactPoint, float knockbackPower, Action callback)
    {
        Vector2 direction = GCalc.CalculeDirectionAway(impactPoint, Rigidbody.position);
        AnimatorController.SetTrigger("Damage", callback);
        PushCharacter(direction, knockbackPower);
        Health.AddHP(-power);
    }

    private void PushCharacter(Vector2 direction, float power)
    {
        StopMovement();
        Rigidbody.AddForce(direction * power, ForceMode2D.Impulse);
    }
    protected abstract void OnDeath();
    protected abstract void OnDamageTaken();
    public void Pause()
    {
        StopMovement();
        AnimatorController.Pause();
    }
    public void DisPause() => AnimatorController.DisPause();
}