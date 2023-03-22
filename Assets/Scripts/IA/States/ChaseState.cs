using UnityEngine;

public class ChaseState : BaseCharacterState
{

    [Header("Params")]
    [SerializeField] private float moveSpeed = 9;
    [SerializeField] private float distanceToReachTarget = 1;
    [SerializeField] private float distanceToLoseTarget = 10;

    [Header("Triggers")]
    [SerializeField] private string playerReached;
    [SerializeField] private string lostPlayer;

    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetAi(animator);
        SetPlayer(animator);

        if(!player)
            animator.SetTrigger(lostPlayer);
        
        CalculatePath(player.position);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ChecKTargetDistance(animator);
        Move(moveSpeed, player.position);
    }

    private void ChecKTargetDistance(Animator animator)
    {
        if(!player)
            animator.SetTrigger(lostPlayer);

        (bool isFar, bool isClose) = ai.CheckTargetDistance(distanceToReachTarget,
                    distanceToLoseTarget, player.position);

        if (isClose)
            animator.SetTrigger(playerReached);
        else if(isFar)
            animator.SetTrigger(lostPlayer);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai.StopMovement();
    }
}
