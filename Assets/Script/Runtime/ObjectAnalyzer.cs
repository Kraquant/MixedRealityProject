using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectAnalyzer : MonoBehaviour
{
    [SerializeField] List<MeshInfo> meshInfos = new List<MeshInfo>();
    private List<Color> colors = new List<Color>();

    private class Triangle
    {
        public Triangle(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        static public int TriangleCommonEdgeCount(Triangle tri1, Triangle tri2)
        {
            // Having a common edge is having 2 common elements
            int count = 0;
            if (tri1.X == tri2.X ||
                tri1.X == tri2.Y ||
                tri1.X == tri2.Z) count++;
            if (tri1.Y == tri2.X ||
                tri1.Y == tri2.Y ||
                tri1.Y == tri2.Z) count++;
            if (tri1.Z == tri2.X ||
                tri1.Z == tri2.Y ||
                tri1.Z == tri2.Z) count++;
            return count;
        }

        static public Triangle[] SplitTrianglesList(int[] triangles)
        {
            List<Triangle> result = new List<Triangle>();
            for (int i = 0; i < triangles.Length / 3; i++)
            {
                result.Add(new Triangle(triangles[3 * i], triangles[3 * i + 1], triangles[3 * i + 2]));
            }
            return result.ToArray();
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() << 2 ^ this.Z.GetHashCode() >> 2;
        }
    }

    private class MeshInfo
    {
        public Mesh Mesh { get; private set; }
        public HashSet<Triangle>[] Islands { get; private set; }
        public HashSet<Triangle>[] Edges { get; private set; }

        private Gradient _vizualisationGradient;

        public MeshInfo(Mesh mesh)
        {
            this.Mesh = mesh;
            Islands = GetIslands(mesh.triangles);
            Edges = GetIslandsEdges();
            SetupGradient();
            
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

        private HashSet<Triangle>[] GetIslands(int[] triangles)
        {
            List<HashSet<Triangle>> islandsList = new List<HashSet<Triangle>>();

            Triangle[] trianglesList = Triangle.SplitTrianglesList(triangles);
            List<HashSet<Triangle>> islands= new List<HashSet<Triangle>>();
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
            while(currentIndex < islands.Count-1)
            {
                bool foundConnection = false;
                for (int i = currentIndex+1; i < islands.Count; i++)
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

        public bool ConnectedIslands(HashSet<Triangle> island1,  HashSet<Triangle> island2)
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

        public void DrawGizmosIslands()
        {
            for (int i = 0; i < Islands.Length; i++)
            {
                if (Islands.Length == 1) Gizmos.color = Color.white;
                else Gizmos.color = _vizualisationGradient.Evaluate((float)i/(float)(Islands.Length-1));
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
            foreach(Triangle triangle in Triangle.SplitTrianglesList(Mesh.triangles))
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
    }

    public void GetMeshes()
    {
        meshInfos.Clear();
        colors.Clear();

        foreach (MeshFilter meshFilter in transform.GetComponentsInChildren<MeshFilter>())
        {
            meshInfos.Add(new MeshInfo(meshFilter.sharedMesh));
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (MeshInfo mesh in meshInfos)
        {
            mesh.DrawGizmosIslands();
            Gizmos.color = Color.cyan;
            mesh.DrawEdges();
        }
    }
}