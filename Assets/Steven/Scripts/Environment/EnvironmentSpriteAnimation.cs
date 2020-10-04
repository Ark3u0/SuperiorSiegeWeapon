using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.gamasutra.com/blogs/VivekTank/20180710/321793/Multiple_ways_of_doing_sprite_sheet_animation_in_Unity3D.php
public class EnvironmentSpriteAnimation : MonoBehaviour
{
    public float frameInterval;
    public Sprite[] frames;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = FindSpriteRendererOnObject();
    }

    void Start()
    {
        StartCoroutine(ActionState());
    }

    private SpriteRenderer FindSpriteRendererOnObject() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogError("[EnvironmentSpriteAnimation] expected SpriteRenderer to exist on gameObject. Please add SpriteRenderer to gameObject and rebuild.");
            throw new System.Exception("[EnvironmentSpriteAnimation] Missing dependency: (SpriteRenderer)");
        }
        return spriteRenderer;
    }

    IEnumerator ActionState()
    {  
        int frame = 0;
        while (frame < frames.Length)
        {
            spriteRenderer.sprite = frames[frame];
            frame++;
            yield return new WaitForSeconds(frameInterval);
            yield return 0;
        }
        
        StartCoroutine(ActionState());
    }
}
