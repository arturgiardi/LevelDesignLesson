using System;
using UnityEngine;

public class MeleeAttackState : BaseCharacterState
{
    [Header("Params")]
    [SerializeField] private int power = 1;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetAi(animator);
        SetPlayer(animator);
        ai.LookTo(player.position);
    }
}
