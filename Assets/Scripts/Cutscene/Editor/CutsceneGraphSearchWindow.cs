using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CutsceneGraphSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private EditorWindow _window;
    private CutsceneGraphView _graphView;
    protected Texture2D _indentationIcon;

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        // var tree = new List<SearchTreeEntry>
        //     {
        //         new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
        //         new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),
        //         new SearchTreeEntry(new GUIContent("Dialogue", _indentationIcon))
        //         {
        //             level = 2, userData = GameEventType.Dialogue
        //         }, 
        //         new SearchTreeEntry(new GUIContent("Choice", _indentationIcon))
        //         {
        //             level = 2, userData = GameEventType.Choice
        //         },
        throw new NotImplementedException();
    }

    public void Configure(EditorWindow window, CutsceneGraphView graphView)
    {
        _window = window;
        _graphView = graphView;

        //Transparent 1px indentation icon as a hack
        _indentationIcon = new Texture2D(1, 1);
        _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
        _indentationIcon.Apply();
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var mousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,
            context.screenMousePosition - _window.position.position);
        var graphMousePosition = _graphView.contentViewContainer.WorldToLocal(mousePosition);
        _graphView.CreateNode((CutsceneActionType)SearchTreeEntry.userData, graphMousePosition);
        return true;
    }
}