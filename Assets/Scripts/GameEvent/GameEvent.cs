using System.Collections;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    [field: SerializeField] public bool StopGameplay { get; set; } = true;
    public abstract IEnumerator Execute();
}
