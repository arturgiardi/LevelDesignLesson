using System.Collections;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    public abstract IEnumerator Execute();
}
