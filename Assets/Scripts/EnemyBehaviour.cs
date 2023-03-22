using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Behaviour/000 - Default", fileName = "NewEnemyBehaviour")]
public class EnemyBehaviour : ScriptableObject
{
    public virtual EnemyBaseState GetStartState(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai)
    {
        return new EnemyPatrolState(character, manager, ai);
    }

    public virtual EnemyBaseState SawPlayer(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai)
    {
        return new EnemyChaseState(character, manager, ai);
    }

    internal EnemyBaseState ChasePlayerSuccessful(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai)
    {
        return new EnemyAttackState(character, manager, ai);
    }

    internal EnemyBaseState StopChasePlayer(EnemyCharacter2 character, EnemyStateMachineManager manager, CharacterAI2 ai)
    {
        return new EnemyPatrolState(character, manager, ai);
    }
}
