using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectAnalyzer : MonoBehaviour
{
    public void SetupObjectAndChildren()
    {
        foreach (MeshFilter meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            ObjectMeshInfo meshInfo = meshFilter.gameObject.AddComponent<ObjectMeshInfo>();
            meshInfo.Setup();
        }
    }
}