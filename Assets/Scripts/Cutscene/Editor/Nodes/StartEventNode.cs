using UnityEngine;

public class StartEventNode : CutsceneActionNode
{
    public StartEventNode(Vector2 position, string guid = "") : base(position, guid)
    {
        title = "Start";
        entryPoint = true;

        GenerateOutputPort(string.Empty);

        RefreshExpandedState();
        RefreshPorts();
    }
}