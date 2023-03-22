using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolState : BaseCharacterState
{

    [Header("Params")]
    [SerializeField] private float moveSpeed = 7;

    [Header("Triggers")]
    [SerializeField] private string destinationReached;
    [SerializeField] private string sawPlayer;
    
    private int moveIndex = 0;
    private bool randomPatrol;
    
    private IList<Transform> patrolPoints;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetAi(animator);
        Setup();
        CalculatePath(patrolPoints[moveIndex].position);
    }

    private void Setup()
    {
        patrolPoints = ai.PatrolPoints;
        randomPatrol = ai.RandomPatrol;

        if (moveIndex >= patrolPoints.Count)
            moveIndex = 0;
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move(moveSpeed, patrolPoints[moveIndex].position);
        FindPlayer(() => animator.SetTrigger(sawPlayer));
        CheckIfCharacterReachedDestination(patrolPoints[moveIndex].position, 
            ()=> animator.SetTrigger(destinationReached));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IncrementMoveIndex();
        ai.StopMovement();
    }

    private void IncrementMoveIndex()
    {
        if (randomPatrol)
            moveIndex = Random.Range(0, patrolPoints.Count);
        else
            moveIndex = moveIndex < patrolPoints.Count - 1 ? moveIndex + 1 : 0;
    }
}
