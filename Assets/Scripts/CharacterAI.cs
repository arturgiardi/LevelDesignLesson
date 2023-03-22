using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[System.Serializable]
public class CharacterAI : MonoBehaviour
{
    [field: SerializeField] private Seeker Seeker { get; set; }
    [field: SerializeField] private PatrolData PatrolData { get; set; }

    private BaseCharacter Character { get; set; }
    public IList<Transform> PatrolPoints => PatrolData.Points;
    public bool RandomPatrol => PatrolData.RandomPatrol;
    private List<Vector3> pathPoints { get; set; } = new List<Vector3>();
    public bool SawPlayer => Character.SawPlayer();
    internal void LookTo(Vector3 position) => Character.LookTo(position);


    private int pathIndex;
    private const float minDistanceToChangePoint = 0.5f;

    public void Init(BaseCharacter character)
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

    public bool ReachedDestination(Vector3 destination) =>
        Vector2.Distance(transform.position, destination) <= minDistanceToChangePoint;

    public void StopMovement() => Character.Move(Character.Direction, 0);
}

