using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Triangle
{
    [SerializeField] int _x;
    [SerializeField] int _y;
    [SerializeField] int _z;

    public Triangle(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }
    public int Z { get => _z; set => _z = value; }

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