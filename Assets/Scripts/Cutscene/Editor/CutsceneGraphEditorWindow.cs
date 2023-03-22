using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CutsceneGraphEditorWindow : EditorWindow
{
    protected CutsceneGraphView _graphView;
    protected static CutsceneGraph _eventGraph;

    protected void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    protected void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    public static void OpenWindow(CutsceneGraph obj)
    {
        _eventGraph = obj;
        var window = GetWindow<CutsceneGraphEditorWindow>();
        window.titleContent = new GUIContent(_eventGraph.name);
    }

    protected void ConstructGraphView()
    {
        SetGraphView();
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
        _graphView.LoadGraphNodes(_eventGraph);
    }

    protected void SetGraphView()
    {
        _graphView = new CutsceneGraphView(this)
        {
            name = "Event Graph"
        };
    }

    void GenerateToolbar()
    {
        var toolbar = new Toolbar();
        var menu = new Menu();

        toolbar.Add(new Button(() => SaveData()) { text = "Save Data" });

        rootVisualElement.Add(toolbar);
    }

    private void SaveData()
    {
        _eventGraph.ClearData();
        SaveEdges();
        SaveNodes();
        EditorUtility.SetDirty(_eventGraph);
    }

    internal void SaveEdges()
    {
        List<Edge> edges = _graphView.edges.ToList();
        for (var i = 0; i < edges.Count; i++)
        {
            var outputNode = (edges[i].output.node as CutsceneActionNode);
            var inputNode = (edges[i].input.node as CutsceneActionNode);
            int targetPortIndex = 0;
            int basePortIndex = 0;

            for (int j = 0; j < outputNode.outputContainer.childCount; j++)
            {
                if (outputNode.outputContainer[j] == edges[i].output)
                    basePortIndex = j;
            }

            for (int j = 0; j < inputNode.inputContainer.childCount; j++)
            {
                if (inputNode.inputContainer[j] == edges[i].input)
                    targetPortIndex = j;
            }

            if (outputNode == null)
                continue;

            _eventGraph.nodeLinks.Add(new NodeLinkData
            {
                baseNodeGUID = outputNode.GUID,
                basePortName = edges[i].output.portName,
                basePortIndex = basePortIndex,
                targetNodeGUID = inputNode.GUID,
                targetPortIndex = targetPortIndex
            });
        }
    }

    internal void SaveNodes()
    {
        List<CutsceneActionNode> nodes = _graphView.nodes.ToList().Cast<CutsceneActionNode>().ToList();
        foreach (var node in nodes)
        {
            if (!node.entryPoint)
                SerializeNode(node);
            else
                _eventGraph.StartNodeGUID = node.GUID;
        }
    }

    protected void SerializeNode(CutsceneActionNode node)
    {
        throw new NotImplementedException();
    }
}