using System;
using UnityEngine;

public abstract class BaseCharacterState : StateMachineBehaviour
{
    protected CharacterAI ai;
    private float elapsedTime;
    private const float timeToRecalculatePath = .5f;
    protected Transform player;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.speed > 0)
            StateUpdate(animator, stateInfo, layerIndex);
    }

    protected virtual void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {}

    protected void SetAi(Animator animator)
    {
        if (!ai)
            ai = animator.GetComponent<CharacterAI>();
    }

    protected void SetPlayer(Animator animator)
    {
        if (!player)
            player = animator.GetComponent<EnemyController>().Player;
    }

    protected void CalculatePath(Vector3 position)
    {
        ai.CalculatePath(position);
        elapsedTime = 0;
    }

    protected void Move(float moveSpeed, Vector3 position)
    {
        ai.Move(moveSpeed);
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeToRecalculatePath)
            CalculatePath(position);
    }
    protected void FindPlayer(Action callback)
    {
        if (ai.SawPlayer)
            callback.Invoke();
    }

    protected void CheckIfCharacterReachedDestination(Vector3 destination, Action callback)
    {
        if (ai.ReachedDestination(destination))
            callback.Invoke();
    }
}