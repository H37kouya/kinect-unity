using UnityEngine;
using System.Collections.Generic;

public class ParticleTrail : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public int pointNum = 100;
    public float interpolateLength = 0.2f;

    private List<Vector3> points_ = new List<Vector3>();
    public List<Vector3> points
    {
        get { return points_; }
    }

    void Start()
    {
        points_.Clear();
    }

    void Update()
    {
        AddPoint(transform.position);

        if (points_.Count > 3)
        {
            var n = Mathf.CeilToInt((points_[1] - points_[2]).magnitude / interpolateLength);
            if (n < 2) n = 2;
            foreach (var point in LineInterpolation.GetQuadraticPoints(
                points_[0], points_[1], points_[2], points_[3], n))
            {
                Emit(point);
            }
        }
        else
        {
            Emit(points_[0]);
        }
    }

    void AddPoint(Vector3 point)
    {
        if (points_.Count >= pointNum)
        {
            points_.RemoveAt(pointNum - 1);
        }
        points_.Insert(0, point);
    }

    void Emit(Vector3 point)
    {
        particleSystem.Emit(
            point,
            Random.onUnitSphere * particleSystem.startSpeed,
            particleSystem.startSize,
            particleSystem.startLifetime,
            particleSystem.startColor);
    }
}