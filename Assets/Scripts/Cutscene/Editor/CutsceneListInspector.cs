using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class EventListAssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        CutsceneGraph obj = EditorUtility.InstanceIDToObject(instanceId) as CutsceneGraph;
        if (obj != null)
        {
            CutsceneGraphEditorWindow.OpenWindow(obj);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(CutsceneGraph))]
public class CutsceneListInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            CutsceneGraphEditorWindow.OpenWindow((CutsceneGraph)target);
        }
    }
}