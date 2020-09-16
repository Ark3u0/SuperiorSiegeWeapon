using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprites : MonoBehaviour
{
    public GameObject[] sprites;

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject sprite in sprites) {
            // Camera position in X-Z axis and Y-aligned to sprite
            Vector3 camPositionXZ = new Vector3(transform.position.x, sprite.transform.position.y, transform.position.z);

            // Lock sprite to look at camera
            sprite.transform.rotation = Quaternion.LookRotation(camPositionXZ - sprite.transform.position);

            // Measure difference between sprite forward transform and body forward/right transform
            float forwardAngleInDegrees = Vector3.Angle(sprite.transform.parent.transform.forward, sprite.transform.forward);
            float rightAngleInDegrees = Vector3.Angle(sprite.transform.parent.transform.forward, sprite.transform.right);

            SpriteSheet spriteSheet = sprite.GetComponent<SpriteSheet>();
            SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();

            if (forwardAngleInDegrees < 45f) {
                // Sprite face forward

                Debug.Log("FORWARD");
                spriteRenderer.sprite = spriteSheet.forward;
                spriteRenderer.flipX = false;
            } else if (forwardAngleInDegrees > 135f) {
                // Sprite face backward

                Debug.Log("BACKWARD");
                spriteRenderer.sprite = spriteSheet.backward;
                spriteRenderer.flipX = false;
            } else {
                if (rightAngleInDegrees < 45f) {
                    // Sprite face left

                    Debug.Log("LEFT");
                    spriteRenderer.sprite = spriteSheet.right;
                    spriteRenderer.flipX = false;
                } else {
                    // Sprite face right

                    Debug.Log("RIGHT");
                    spriteRenderer.sprite = spriteSheet.right;
                    spriteRenderer.flipX = true;
                }
            }
        }
    }
}
