using System.Collections;
using UnityEngine;

[System.Serializable]
public abstract class CutsceneAction
{
    [field: SerializeField] public string nodeGUID { get; private set; }
    [field: SerializeField] public Vector2 position { get; set; }

    public CutsceneAction(string GUID, Vector2 pos)
    {
        nodeGUID = GUID;
        position = pos;
    }

    protected abstract IEnumerator EventAction();
    public IEnumerator Execute(CutsceneGraph actionList)
    {
        yield return EventAction();
        yield return ExecuteNext(actionList);
    }
    protected virtual IEnumerator ExecuteNext(CutsceneGraph actionList)
    {
        var nodeLink = actionList.GetNodeLink(nodeGUID);
        
        if(nodeLink == null)
            yield break;

        var nextEvent = actionList.GetEventByGUID(nodeLink.targetNodeGUID);
        if (nextEvent != null)
            yield return nextEvent.Execute(actionList);
    }
}

public enum CutsceneActionType
{
    MoveCharacter
}
