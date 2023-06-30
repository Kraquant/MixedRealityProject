using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectAnalyzer : MonoBehaviour
{
    [SerializeField] List<Mesh> meshList = new List<Mesh>();

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
        public bool TriangleCommonEdge(Triangle tri1, Triangle tri2)
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
            return count > 1;

        }
        static public Triangle[] SplitTrianglesList(int[] triangles)
        {
            List<Triangle> result = new List<Triangle>();
            for (int i = 0; i < triangles.Length / 3; i++)
            {
                result.Add(new Triangle(triangles[3 * i], triangles[3 * i + 1], triangles[3 * i + 2]));
            }
            Debug.Log(triangles.Length.ToString() + " " + result.Count.ToString());
            return result.ToArray();
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() << 2 ^ this.Z.GetHashCode() >> 2;
        }
    }

    public void GetMeshes()
    {
        meshList.Clear();
        colors.Clear();

        foreach (MeshFilter meshFilter in transform.GetComponentsInChildren<MeshFilter>())
        {
            meshList.Add(meshFilter.sharedMesh);
            colors.Add(Random.ColorHSV());
        }
    }

    

    private Triangle[][] GetIslands(int[] triangles)
    {
        List<List<Triangle>> islandsList = new List<List<Triangle>>();

        Triangle[] trianglesList = Triangle.SplitTrianglesList(triangles);

        for (int i = 0; i < trianglesList.Length; i++)
        {
            
        }
    }

    


    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < meshList.Count; i++)
        {
            Gizmos.color = colors[i];

            foreach(Vector3 uvPos in meshList[i].uv)
            {
                Gizmos.DrawSphere(uvPos, 0.005f);
            }
            foreach (Triangle triangle in Triangle.SplitTrianglesList(meshList[i].triangles))
            {
                Gizmos.DrawLine(meshList[i].uv[triangle.X], meshList[i].uv[triangle.Y]);
                Gizmos.DrawLine(meshList[i].uv[triangle.X], meshList[i].uv[triangle.Z]);
                Gizmos.DrawLine(meshList[i].uv[triangle.Y], meshList[i].uv[triangle.Z]);
            }
        }
    }
}
