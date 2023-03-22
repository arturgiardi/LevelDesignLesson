using UnityEngine;
using Random = UnityEngine.Random;

public class WaitState : BaseCharacterState
{
    [Header("Params")]
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    
    [Header("Triggers")]
    [SerializeField] private string waitTimeOver;
    [SerializeField] private string sawPlayer;

    private float waitTime = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        SetAi(animator);
    }

    protected override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FindPlayer(() => animator.SetTrigger(sawPlayer));
        Wait(animator);
    }

    private void Wait(Animator animator)
    {
        waitTime -= Time.deltaTime;
        if(waitTime <= 0)
            animator.SetTrigger(waitTimeOver);
    }
}
