using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectAnalyzer))]
public class ObjectAnalyzerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectAnalyzer script = (ObjectAnalyzer)target;

        base.OnInspectorGUI();
        if (GUILayout.Button("Setup Object and Children"))
        {
            Debug.Log("Setting up meshes" + target.name);
            script.SetupObjectAndChildren();
            EditorUtility.SetDirty(script);
        }
    }

    
}
