using System;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    [field: SerializeField] private Animator Animator { get; set; }
    [field: SerializeField] private SpriteRenderer SpriteRenderer { get; set; }
    private BaseCharacter Character { get; set; }
    private Action AnimationCallback { get; set; }
    private AttackMove AttackMove { get; set; }
    public Vector2 Direction => new Vector2(Animator.GetFloat(lastMoveX), Animator.GetFloat(lastMoveY));
    private float _speed = 1;
    public float Speed
    {
        set
        {
            _speed = value;
            Animator.speed = IsPaused ? 0 : _speed;
        }
    }
    private bool IsPaused { get; set; }

    const string lastMoveX = "X";
    const string lastMoveY = "Y";
    const string speed = "Speed";

    public void Init(BaseCharacter character, Vector2 direction)
    {
        Character = character;
        SetDirection(direction);
    }

    private void LateUpdate()
    {
        PerformedSettedAttackMove();
    }

    private void PerformedSettedAttackMove()
    {
        if (AttackMove)
        {
            Character.DoMeleeDamage(AttackMove);
            AttackMove = null;
        }
    }

    public void SetDirection(Vector2 direction)
    {
        if (direction == Vector2.left)
            SpriteRenderer.flipX = true;
        else
            SpriteRenderer.flipX = false;

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            direction.x = 0;
        else
            direction.y = 0;

        Animator.SetFloat(lastMoveX, direction.x);
        Animator.SetFloat(lastMoveY, direction.y);
    }

    public void SetSpeed(Vector2 velocity)
    {
        if (velocity.x != 0 || velocity.y != 0)
            Animator.SetInteger(speed, 1);
        else
            Animator.SetInteger(speed, 0);
    }

    public void SetTrigger(string triggerName, Action callback = null)
    {
        AnimationCallback = callback;
        Animator.SetTrigger(triggerName);
    }

    public void SetAttackTrigger(string triggerName, Action callback = null)
    {
        AnimationCallback = callback;
        Animator.SetTrigger(triggerName);
    }

    public void Pause()
    {
        IsPaused = true;
        Animator.speed = 0;
    }

    public void DisPause()
    {
        IsPaused = false;
        Speed = _speed;
    }

    public void AttackMovePerformed(AttackMove move)
    {
        AttackMove = move;
    }

    public void CallAnimationCallback()
    {
        AnimationCallback?.Invoke();
        AnimationCallback = null;
    }

    public void SetBool(string name, bool value) => Animator.SetBool(name, value);
}