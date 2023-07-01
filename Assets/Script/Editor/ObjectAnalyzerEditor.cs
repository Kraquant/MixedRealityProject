using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

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
        }
    }

    
}
