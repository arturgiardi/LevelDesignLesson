using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class CharacterAI2 : MonoBehaviour
{
    [field: SerializeField] private Seeker Seeker { get; set; }
    [field: SerializeField] private WaitData WaitData { get; set; }
    [field: SerializeField] private PatrolData PatrolData { get; set; }
    [field: SerializeField] private ChaseData ChaseData { get; set; }
    [field: SerializeField] public EnemyBehaviour Behaviour { get; private set; }

    private EnemyCharacter2 Character { get; set; }
    public float WaitTime => WaitData.WaitTime;
    public Vector3 PatrolDestination => PatrolData.GetPatrolDestination();
    private List<Vector3> pathPoints { get; set; } = new List<Vector3>();
    public bool HasPath => pathIndex < pathPoints.Count;
    public float PatrolSpeed => PatrolData.Speed;
    public float ChaseSpeed => ChaseData.Speed;
    public float ChanceDistanceToReach => ChaseData.DistanceToReach;
    public float ChanceDistanceToLose => ChaseData.DistanceToLose;

    private int pathIndex;
    private const float minDistanceToChangePoint = 0.5f;

    public void Init(EnemyCharacter2 character)
    {
        Character = character;
        pathPoints.Clear();
        pathIndex = 0;
    }

    public void CalculatePath(Vector3 position)
    {
        if (!Seeker.IsDone())
            Seeker.CancelCurrentPathRequest();

        Seeker.StartPath(transform.position, position, path =>
        {
            if (path.error)
                throw new InvalidOperationException($"Erro ao calcular o path {path.errorLog}");
            pathPoints = path.vectorPath;
            pathIndex = 0;
        });
    }

    internal (bool isFar, bool isClose) CheckTargetDistance(float distanceToReachTarget, float distanceToLoseTarget, Vector3 target)
    {
        var isFar = false;
        var isClose = false;
        var distance = Vector3.Distance(transform.position, target);
        if (distance >= distanceToLoseTarget)
            isFar = true;
        if (distance <= distanceToReachTarget)
            isClose = true;
        return (isFar, isClose);
    }

    internal Vector2 GetMoveDirection()
    {
        return (Vector2)(pathPoints[pathIndex] - transform.position).normalized;
    }

    public void Move(float moveSpeed)
    {
        if (pathIndex >= pathPoints.Count)
            return;

        Vector2 direction = (Vector2)(pathPoints[pathIndex] - transform.position).normalized;
        Character.SetDirection(direction);
        Character.Move(direction, moveSpeed);

        if (ReachedDestination(pathPoints[pathIndex]))
            pathIndex++;
    }

    public void UpdateCurrentPathPoint()
    {
        if (ReachedDestination(pathPoints[pathIndex]))
            pathIndex++;
    }

    public bool ReachedDestination(Vector3 destination) =>
        Vector2.Distance(transform.position, destination) <= minDistanceToChangePoint;

    public void StopMovement() => Character.Move(Character.Direction, 0);
}

[System.Serializable]
public class WaitData
{
    [field: SerializeField] private float MinWaitTime { get; set; }
    [field: SerializeField] private float MaxWaitTime { get; set; }

    public float WaitTime => Random.Range(MinWaitTime, MaxWaitTime);
}

[System.Serializable]
public class PatrolData
{
    [field: SerializeField] public bool RandomPatrol { get; private set; }
    [SerializeField] private List<Transform> _points;
    public IList<Transform> Points => _points.AsReadOnly();
    private int Index { get; set; } = -1;
    public float Speed { get; private set; } = 5;

    internal Vector3 GetPatrolDestination()
    {
        if (RandomPatrol)
        {
            return _points[Random.Range(0, _points.Count)].position;
        }
        else
        {
            Index++;
            if (Index >= _points.Count)
                Index = 0;
            return _points[Index].position;
        }
    }
}

[System.Serializable]
public class ChaseData
{
    public float Speed { get; private set; } = 5;
    public float DistanceToReach { get; internal set; }
    public float DistanceToLose { get; internal set; }
}