using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectMeshInfo : MonoBehaviour
{
    [SerializeField] Mesh _mesh;
    [SerializeField] Gradient _vizualisationGradient;
    [SerializeField] bool _drawGizmos = true;

    //Variables for serialization
    [SerializeField] List<ListWrapper<Triangle>> _islandsSerialized;
    [SerializeField] List<ListWrapper<Triangle>> _edgesSerialized;

    //Variables for runtime
    private HashSet<Triangle>[] _islands;
    private HashSet<Triangle>[] _edges;

    public Mesh Mesh { get => _mesh; set => new System.NotImplementedException(); }
    public HashSet<Triangle>[] Islands
    {
        get
        {
            if (_islands == null) _islands = ConvertForRuntime(_islandsSerialized);
            return _islands;
        }
        set
        {
            throw new System.Exception("Cannot set the Island value");
        }
    }
    public HashSet<Triangle>[] Edges
    {
        get
        {
            if (_edges == null) _edges = ConvertForRuntime(_edgesSerialized);
            return _edges;
        }
        set
        {
            throw new System.Exception("Cannot set the Edge value");
        }
    }

    public void Setup()
    {
       _mesh = this.GetComponent<MeshFilter>().sharedMesh;
        if (_mesh == null ) throw new System.NullReferenceException();

        _islandsSerialized = ConvertForSerialization(GetIslands(_mesh.triangles));
        _edgesSerialized = ConvertForSerialization(GetIslandsEdges());
        SetupGradient();
    }

    public int GetIslandIndex(Triangle triangle)
    {
        for (int i = 0; i < Islands.Length; i++)
        {
            if (Islands[i].Contains(triangle)) return i;
        }
        return -1;
    }
    public int GetIslandIndex(int triangleIndex)
    {
        return GetIslandIndex(
            new Triangle(
                Mesh.triangles[triangleIndex * 3 + 0],
                Mesh.triangles[triangleIndex * 3 + 1],
                Mesh.triangles[triangleIndex * 3 + 2]));
    }

    private void Start()
    {
        _islands = ConvertForRuntime(_islandsSerialized);
        _edges = ConvertForRuntime(_edgesSerialized);
    }

    private void OnDrawGizmosSelected()
    {
        if (_drawGizmos)
        {
            DrawGizmosIslands();
        }
    }

    #region Gizmos
    public void DrawGizmosIslands()
    {
        for (int i = 0; i < Islands.Length; i++)
        {
            if (Islands.Length == 1) Gizmos.color = Color.white;
            else Gizmos.color = _vizualisationGradient.Evaluate((float)i / (float)(Islands.Length - 1));
            foreach (Triangle triangle in Islands[i])
            {
                Gizmos.DrawLine(Mesh.uv[triangle.X], Mesh.uv[triangle.Y]);
                Gizmos.DrawLine(Mesh.uv[triangle.X], Mesh.uv[triangle.Z]);
                Gizmos.DrawLine(Mesh.uv[triangle.Z], Mesh.uv[triangle.Y]);
            }
        }
    }
    public void DrawUVTriangles()
    {
        foreach (Triangle triangle in Triangle.SplitTrianglesList(Mesh.triangles))
        {
            Gizmos.DrawLine(Mesh.uv[triangle.X], Mesh.uv[triangle.Y]);
            Gizmos.DrawLine(Mesh.uv[triangle.X], Mesh.uv[triangle.Z]);
            Gizmos.DrawLine(Mesh.uv[triangle.Z], Mesh.uv[triangle.Y]);
        }
    }
    public void DrawUVVertice(float size = 0.005f)
    {
        foreach (Vector3 uv in Mesh.uv)
        {
            Gizmos.DrawSphere(uv, size);
        }
    }
    public void DrawEdges()
    {
        foreach (HashSet<Triangle> IslandBorder in Edges)
        {
            foreach (Triangle triangle in IslandBorder)
            {
                Gizmos.DrawLine(Mesh.uv[triangle.X], Mesh.uv[triangle.Y]);
                Gizmos.DrawLine(Mesh.uv[triangle.X], Mesh.uv[triangle.Z]);
                Gizmos.DrawLine(Mesh.uv[triangle.Z], Mesh.uv[triangle.Y]);
            }
        }
    }
    #endregion
    #region Private Setup Methods
    private HashSet<Triangle>[] GetIslands(int[] triangles)
    {
        List<HashSet<Triangle>> islandsList = new List<HashSet<Triangle>>();

        Triangle[] trianglesList = Triangle.SplitTrianglesList(triangles);
        List<HashSet<Triangle>> islands = new List<HashSet<Triangle>>();
        foreach (Triangle triangle in trianglesList) islands.Add(new HashSet<Triangle>() { triangle });
        MergeIslands(ref islands);
        return islands.ToArray();
    }
    private HashSet<Triangle>[] GetIslandsEdges()
    {
        List<HashSet<Triangle>> edges = new List<HashSet<Triangle>>();
        foreach (HashSet<Triangle> island in Islands)
        {
            HashSet<Triangle> islandEdge = new HashSet<Triangle>();
            Triangle[] triList = island.ToArray();

            for (int i = 0; i < triList.Length; i++)
            {
                int sharedEdges = 0;
                for (int j = 0; j < triList.Length; j++)
                {
                    if (i == j) continue;
                    sharedEdges += Triangle.TriangleCommonEdgeCount(triList[i], triList[j]);
                    if (sharedEdges > 2) break;
                }

                if (sharedEdges < 3) islandEdge.Add(triList[i]);
            }
            edges.Add(islandEdge);
        }

        return edges.ToArray();
    }
    private void MergeIslands(ref List<HashSet<Triangle>> islands)
    {
        if (islands.Count <= 1) return;

        int currentIndex = 0;
        while (currentIndex < islands.Count - 1)
        {
            bool foundConnection = false;
            for (int i = currentIndex + 1; i < islands.Count; i++)
            {
                if (ConnectedIslands(islands[currentIndex], islands[i]))
                {
                    islands[currentIndex].UnionWith(islands[i]);
                    islands.RemoveAt(i);
                    foundConnection = true;
                    break;
                }
            }
            if (!foundConnection) currentIndex++;
        }
    }
    private bool ConnectedIslands(HashSet<Triangle> island1, HashSet<Triangle> island2)
    {
        foreach (Triangle tri1 in island1)
        {
            foreach (Triangle tri2 in island2)
            {
                if (Triangle.TriangleCommonEdgeCount(tri1, tri2) > 1) return true;
            }
        }
        return false;
    }
    private void SetupGradient()
    {
        _vizualisationGradient = new Gradient();

        var colors = new GradientColorKey[6];
        colors[0] = new GradientColorKey(new Color(1.0f, 0.0f, 0.0f), 0.0f);
        colors[1] = new GradientColorKey(new Color(1.0f, 0.647f, 0.0f), 0.2f);
        colors[2] = new GradientColorKey(new Color(1.0f, 1.0f, 0.0f), 0.4f);
        colors[3] = new GradientColorKey(new Color(0.0f, 0.0502f, 0.0f), 0.6f);
        colors[4] = new GradientColorKey(new Color(0.0f, 0.0f, 1.0f), 0.8f);
        colors[5] = new GradientColorKey(new Color(0.502f, 0.0f, 0.502f), 1.0f);

        var alphas = new GradientAlphaKey[6];
        alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphas[1] = new GradientAlphaKey(1.0f, 0.2f);
        alphas[2] = new GradientAlphaKey(1.0f, 0.4f);
        alphas[3] = new GradientAlphaKey(1.0f, 0.6f);
        alphas[4] = new GradientAlphaKey(1.0f, 0.8f);
        alphas[5] = new GradientAlphaKey(1.0f, 1.0f);

        _vizualisationGradient.SetKeys(colors, alphas);
    } 
    
    private List<ListWrapper<Triangle>> ConvertForSerialization(HashSet<Triangle>[] hashArray)
    {
        List<ListWrapper<Triangle>> res = new List<ListWrapper<Triangle>>();
        foreach (HashSet<Triangle> hash in hashArray)
        {
            res.Add(new ListWrapper<Triangle>());
            res.Last().list = hash.ToList();
        }
        return res;
    }

    private HashSet<Triangle>[] ConvertForRuntime(List<ListWrapper<Triangle>> arrayArray)
    {
        List<HashSet<Triangle>> hashList = new List<HashSet<Triangle>>();
        foreach (ListWrapper<Triangle> islandArray in _islandsSerialized)
        {
            hashList.Add(new HashSet<Triangle>());
            hashList.Last().AddRange(islandArray.list);
        }
        return hashList.ToArray();
    }

    #endregion
}
