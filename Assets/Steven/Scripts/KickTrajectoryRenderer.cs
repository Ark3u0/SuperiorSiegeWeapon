using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reference: https://www.youtube.com/watch?v=iLlWirdxass
public class KickTrajectoryRenderer : MonoBehaviour
{
    public Vector3 position = new Vector3(0, 0, 0);
    public Vector3 heading = new Vector3(0, 0, 0);
    public float velocity = 1f;
    public float angle = 1f;
    public int resolution = 10;
    public bool rendering = false;
    float gravity = 1f;
    float radianAngle = 1f;

    LineRenderer lineRenderer;

    void Awake() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics2D.gravity.y);
    }

    // Populating Line Renderer w/ appropriate settings:
    // - point positions to draw lines between
    // - # of points + 1
    void RenderArc()
    {
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.SetPositions(CalculateArcArray());
    }

    // Create an array of Vector3 positions
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        radianAngle = Mathf.Deg2Rad * angle;

        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;

        for (int i = 0; i <= resolution; i++) {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    // Calculate posiiton in space of each vertex
    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float r = t * maxDistance; // r is distance traveled in xz plane

        float x = r * heading.normalized.x;
        float z = r * heading.normalized.z;

        float y = r * Mathf.Tan(radianAngle) - ((gravity * r * r) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        return new Vector3(position.x + x, position.y + y, position.z + z); 
    }

    // Update is called once per frame
    void Update()
    {
        if (this.rendering) {
            RenderArc();
        } else {
            lineRenderer.positionCount = 0;
        }
        
    }
}
