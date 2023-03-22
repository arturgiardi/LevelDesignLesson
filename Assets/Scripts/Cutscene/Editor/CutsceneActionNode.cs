using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public abstract class CutsceneActionNode : Node
{
    protected readonly StyleLength minWidth = new StyleLength(250);
    readonly Vector2 defaultNodeSize = new Vector2(150, 200);
    protected CutsceneAction _nodeData;
    [field: SerializeField] public string GUID { get; set; }
    [field: SerializeField] public bool entryPoint { get; set; }

    public CutsceneActionNode(Vector2 position, string guid)
    {
        GUID = guid != string.Empty ? guid : Guid.NewGuid().ToString();
        SetPosition(new Rect(position, defaultNodeSize));
    }
    protected void Refresh()
    {
        RefreshExpandedState();
        RefreshPorts();
    }

    protected void AddPorts()
    {
        GenerateInputPort();
        GenerateOutputPort();
    }
    protected Port GenerateOutputPort(string portName = "", Port.Capacity capacity = Port.Capacity.Single)
    {
        Port port = GeneratePort(portName, Direction.Output, capacity);
        outputContainer.Add(port);
        return port;
    }

    protected void GenerateInputPort(string portName = "", Port.Capacity capacity = Port.Capacity.Multi)
    {
        Port port = GeneratePort(portName, Direction.Input, capacity);
        inputContainer.Add(port);
    }

    Port GeneratePort(string portName, Direction portDirection, Port.Capacity capacity)
    {
        Port port = InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(CutsceneActionNode)); //type: tipo de dados que ser�o transmitidos
        port.portName = portName;
        port.portColor = Color.green;
        return port;
    }

    protected void GenerateBoolPort(Direction direction, string portName = "")
    {
        Port port = InstantiatePort(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(bool));
        port.portName = portName;
        if (direction == Direction.Input)
            inputContainer.Add(port);
        if (direction == Direction.Output)
            outputContainer.Add(port);
    }

    protected void SetPosition()
    {
        _nodeData.position = GetPosition().position;
    }
}