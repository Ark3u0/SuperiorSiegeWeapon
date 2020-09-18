﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reference: https://www.youtube.com/watch?v=iLlWirdxass
public class KickTrajectoryRenderer : MonoBehaviour
{
    public CameraTarget cameraTarget;
    public Camera cam;
    public GameObject renderTarget;
    public int resolution = 10;
    public bool rendering = false;
    float gravity = 1f;

    LineRenderer lineRenderer;

    void Awake() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics2D.gravity.y);
    }

    public void Render(Vector3 start, Vector3 target, float angle)
    {
        cam.GetComponent<FollowCameraTarget>().SetCameraTarget(cameraTarget);
        renderTarget.SetActive(true);
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.SetPositions(CalculateArcArray(start, target, angle));
    }

    // Create an array of Vector3 positions
    Vector3[] CalculateArcArray(Vector3 start, Vector3 target, float angle)
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        float angleInRadians = Mathf.Deg2Rad * angle;
        
        float velocity = CalculateVelocityMagnitude(angleInRadians, start, target);

        for (int i = 0; i <= resolution; i++) {
            Vector3 arcPoint = CalculateArcPoint(i, angleInRadians, velocity, start, target);
        
            if (i > 0) 
            {
                RaycastHit hit;
                Vector3 aToB = (arcPoint - arcArray[i - 1]);

                if (Physics.Raycast(arcArray[i - 1], aToB.normalized, out hit, aToB.magnitude)) {
                    PlaceTargetAt(hit);
                    cameraTarget.UpdateTargetPosition((start + target + arcArray[i / 2]) / 3f);

                    lineRenderer.positionCount = i;
                    return arcArray;
                }
            }
            

            arcArray[i] = arcPoint;
        }

        PlaceTargetAt(target);
        cameraTarget.UpdateTargetPosition((start + target + arcArray[lineRenderer.positionCount / 2]) / 3f);

        return arcArray;
    }

    void PlaceTargetAt(Vector3 point) {
        renderTarget.transform.position = new Vector3(point.x, Vector3.up.y * .02f, point.z);
        renderTarget.transform.LookAt(point + Vector3.up);
    }

    void PlaceTargetAt(RaycastHit hit) {
        renderTarget.transform.position = hit.point;
        renderTarget.transform.LookAt(hit.point + hit.normal);
        renderTarget.transform.Translate(hit.normal * .02f, Space.World);
    }

    public float CalculateVelocityMagnitude(float angleInRadians, Vector3 start, Vector3 target) {
        Vector3 displacement = target - start;

        return Mathf.Sqrt(displacement.magnitude * gravity / Mathf.Sin(2 * angleInRadians));
    }

    public Vector3 CalculateVelocityDirection(int step, float angleInRadians, float velocity, Vector3 start, Vector3 target)
    {
        Vector3 displacement = target - start;
        float t = (float)step / (float)resolution;
        

        float r = t * displacement.magnitude; // r is distance traveled in xz plane

        float x = r * displacement.normalized.x;
        float z = r * displacement.normalized.z;

        float y = r * Mathf.Tan(angleInRadians) - ((gravity * r * r) / (2 * velocity * velocity * Mathf.Cos(angleInRadians) * Mathf.Cos(angleInRadians)));
        
        return new Vector3(x, y, z);
    }

    // Calculate posiiton in space of each vertex
    Vector3 CalculateArcPoint(int step, float angleInRadians, float velocity, Vector3 start, Vector3 target)
    {   
        return start + CalculateVelocityDirection(step, angleInRadians, velocity, start, target);
    }

    public void StopRendering() {
        renderTarget.SetActive(false);
        lineRenderer.positionCount = 0;
    }
}
