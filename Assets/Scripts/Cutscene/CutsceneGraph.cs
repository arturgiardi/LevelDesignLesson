using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GameEventGraph", fileName = "GameEventGraph")]
public class CutsceneGraph : ScriptableObject
{
    [field: SerializeField] public string StartNodeGUID { get; set; }
    public List<NodeLinkData> nodeLinks = new List<NodeLinkData>();
    public NodeLinkData GetNodeLink(string nodeGUID)
    {
        foreach (var item in nodeLinks)
        {
            if (item.baseNodeGUID == nodeGUID)
                return item;
        }
        return null;
    }

    public void ClearData()
    {
        throw new NotImplementedException();
    }

    public CutsceneAction GetEventByGUID(string targetNodeGUID)
    {
        throw new NotImplementedException();
    }
}

[System.Serializable]
public class NodeLinkData
{
    public string baseNodeGUID;
    public string basePortName;
    public int basePortIndex;
    public string targetNodeGUID;
    public int targetPortIndex;
}
