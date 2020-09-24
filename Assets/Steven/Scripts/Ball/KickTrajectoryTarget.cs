using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickTrajectoryTarget : MonoBehaviour
{
    public float scalingPeriod = 1f;
    public float alphaPeriod = 1f;
    // public float rotationSpeed = 100f;
    public float scalingMagnitude = 3f;

    // Update is called once per frame
	void Update () {
		// infinitely rotate this target and scale about the Y axis in world space
		// transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);
        float scale = Mathf.Sin(2 * Mathf.PI * Time.time * scalingPeriod) * scalingMagnitude;
        transform.localScale = new Vector3(1f + scale, 1f + scale);

        // oscillate between semi-transparency
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, .7f + Mathf.Sin(2 * Mathf.PI * Time.time * alphaPeriod) * .3f);
	}
}
