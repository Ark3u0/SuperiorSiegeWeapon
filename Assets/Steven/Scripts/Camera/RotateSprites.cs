using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprites : MonoBehaviour
{
    private NpcSpriteAnimation[] npcSpriteAnimations;
    private PlayerSpriteAnimation[] playerSpriteAnimations;

    void Awake() {
        npcSpriteAnimations = FindObjectsOfType<NpcSpriteAnimation>();
        playerSpriteAnimations = FindObjectsOfType<PlayerSpriteAnimation>();
    }

    private void RotateNpcs() {
        foreach (NpcSpriteAnimation npcSpriteAnimation in npcSpriteAnimations) {
            // Camera position in X-Z axis and Y-aligned to sprite
            Vector3 camPositionXZ = new Vector3(transform.position.x, npcSpriteAnimation.transform.position.y, transform.position.z);

            // Lock sprite to look at camera
            npcSpriteAnimation.transform.rotation = Quaternion.LookRotation(camPositionXZ - npcSpriteAnimation.transform.position);

            // Measure difference between sprite forward transform and body forward/right transform
            float forwardAngleInDegrees = Vector3.Angle(npcSpriteAnimation.transform.parent.transform.forward, npcSpriteAnimation.transform.forward);
            float rightAngleInDegrees = Vector3.Angle(npcSpriteAnimation.transform.parent.transform.forward, npcSpriteAnimation.transform.right);

            if (forwardAngleInDegrees < 45f) {
                // Sprite face forward/down
                npcSpriteAnimation.SetDirection(SpriteDirection.DOWN);
            } else if (forwardAngleInDegrees > 135f) {
                npcSpriteAnimation.SetDirection(SpriteDirection.UP);
            } else {
                if (rightAngleInDegrees < 45f) {
                    // Sprite face left
                    npcSpriteAnimation.SetDirection(SpriteDirection.LEFT);
                } else {
                    // Sprite face right
                    npcSpriteAnimation.SetDirection(SpriteDirection.RIGHT);
                }
            }
        }
    }

    private void RotatePlayers() {
        foreach (PlayerSpriteAnimation playerSpriteAnimation in playerSpriteAnimations) {
            // Camera position in X-Z axis and Y-aligned to sprite
            Vector3 camPositionXZ = new Vector3(transform.position.x, playerSpriteAnimation.transform.position.y, transform.position.z);

            // Lock sprite to look at camera
            playerSpriteAnimation.transform.rotation = Quaternion.LookRotation(camPositionXZ - playerSpriteAnimation.transform.position);

            // Measure difference between sprite forward transform and body forward/right transform
            float forwardAngleInDegrees = Vector3.Angle(playerSpriteAnimation.transform.parent.transform.forward, playerSpriteAnimation.transform.forward);
            float rightAngleInDegrees = Vector3.Angle(playerSpriteAnimation.transform.parent.transform.forward, playerSpriteAnimation.transform.right);

            if (forwardAngleInDegrees < 45f) {
                // Sprite face forward/down
                playerSpriteAnimation.SetDirection(SpriteDirection.DOWN);
            } else if (forwardAngleInDegrees > 135f) {
                playerSpriteAnimation.SetDirection(SpriteDirection.UP);
            } else {
                if (rightAngleInDegrees < 45f) {
                    // Sprite face left
                    // playerSpriteAnimation.SetDirection(SpriteDirection.LEFT);
                    playerSpriteAnimation.SetDirection(SpriteDirection.RIGHT);
                    
                } else {
                    // Sprite face right
                    // playerSpriteAnimation.SetDirection(SpriteDirection.RIGHT);
                    playerSpriteAnimation.SetDirection(SpriteDirection.LEFT);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       RotateNpcs();
       RotatePlayers();
    }
}
