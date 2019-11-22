using UnityEngine;
using System.Collections.Generic;

public static class LineInterpolation
{
    public static List<Vector3> GetQuadraticPoints(
            Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int num)
    {
        var points = new List<Vector3>();
        for (int i = 0; i < num; ++i)
        {
            var t = (float)i / (num - 1);
            var l1 = GetQuadraticPoint(p1, p2, p3, 0.5f * (1f + t));
            var l2 = GetQuadraticPoint(p2, p3, p4, 0.5f * t);
            points.Add((l1 + l2) * 0.5f);
        }
        return points;
    }

    private static Vector3 GetQuadraticPoint(
            Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p1, p2, t), Vector3.Lerp(p2, p3, t), t);
    }
}