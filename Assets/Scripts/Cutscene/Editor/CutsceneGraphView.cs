using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CutsceneGraphView : GraphView
{
    protected CutsceneGraphSearchWindow _searchWindow;

    public CutsceneGraphView(CutsceneGraphEditorWindow editorWindow)
    {
        Create();
        AddSearchWindow(editorWindow);
    }

    internal void CreateNode(CutsceneActionType type, Vector2 position)
    {
        // switch (type)
        //     {
        //         case GameEventType.Dialogue:
        //             AddElement(new DialogueEventNode(position));
        //             break;
        throw new NotImplementedException();
    }

    protected void CreateSearchWindow()
    {
        _searchWindow = ScriptableObject.CreateInstance<CutsceneGraphSearchWindow>();
    }

    protected void Create()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        var grid = new GridBackground();
        Insert(0, grid);

        AddElement(GenerateEntryPointNode());
    }

    protected void AddSearchWindow(CutsceneGraphEditorWindow editorWindow)
    {
        CreateSearchWindow();
        _searchWindow.Configure(editorWindow, this);
        nodeCreationRequest = context =>
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    private StartEventNode GenerateEntryPointNode()
    {
        return new StartEventNode(new Vector2(100, 200));
    }

    internal void LoadGraphNodes(CutsceneGraph eventGraph)
    {
        GenerateNodes(eventGraph);
        ConnectNodes(eventGraph);
    }

    protected void GenerateNodes(CutsceneGraph eventGraph)
    {
        // GameEventGraph eventGraph = (GameEventGraph)graph;

        //     foreach (var eventData in eventGraph.dialogueEvents)
        //     {
        //         var tempNode = DialogueEventNode.ConvertEventToNode(eventData);
        //         AddElement(tempNode);
        //     }
        throw new NotImplementedException();
    }

    private void ConnectNodes(CutsceneGraph eventGraph)
    {
        List<CutsceneActionNode> Nodes = nodes.ToList().Cast<CutsceneActionNode>().ToList();
        if (eventGraph.nodeLinks.Count > 0)
            Nodes.Find(x => x.entryPoint).GUID = eventGraph.StartNodeGUID;

        for (var i = 0; i < Nodes.Count; i++)
        {
            //var k = i; //Prevent access to modified closure
            var connections = eventGraph.nodeLinks.Where(x => x.baseNodeGUID == Nodes[i].GUID).ToList();
            for (var j = 0; j < connections.Count; j++)
            {
                var targetNodeGUID = connections[j].targetNodeGUID;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);

                LinkNodesTogether((Port)Nodes[i].outputContainer[connections[j].basePortIndex], (Port)targetNode.inputContainer[connections[j].targetPortIndex]);
            }
        }
    }

    private void LinkNodesTogether(Port outputSocket, Port inputSocket)
    {
        var tempEdge = new Edge()
        {
            output = outputSocket,
            input = inputSocket
        };
        tempEdge?.input.Connect(tempEdge);

        Add(tempEdge);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node && startPort.portType == port.portType)
                compatiblePorts.Add(port);
        });
        return compatiblePorts;
    }
}
