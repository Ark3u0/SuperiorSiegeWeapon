using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.gamasutra.com/blogs/VivekTank/20180710/321793/Multiple_ways_of_doing_sprite_sheet_animation_in_Unity3D.php
public class NpcSpriteAnimation : MonoBehaviour
{
    public float idleInterval;
    public Sprite[] idleUp;
    public Sprite[] idleDown;
    public Sprite[] idleLeft;
    public Sprite[] idleRight;
    private Dictionary<SpriteDirection, Sprite[]> idle;
    private SpriteDirection direction;
    private bool directionChange;
    private int frame;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = FindSpriteRendererOnObject();
        frame = Random.Range(0, idleUp.Length);
        direction = SpriteDirection.UP;

        idle = new Dictionary<SpriteDirection, Sprite[]>();

        idle.Add(SpriteDirection.UP, idleUp);
        idle.Add(SpriteDirection.DOWN, idleDown);
        idle.Add(SpriteDirection.LEFT, idleLeft);
        idle.Add(SpriteDirection.RIGHT, idleRight);
    }

    void Start()
    {
        StartCoroutine(ActionState());
    }

    private SpriteRenderer FindSpriteRendererOnObject() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogError("[NpcSpriteAnimation] expected SpriteRenderer to exist on gameObject. Please add SpriteRenderer to gameObject and rebuild.");
            throw new System.Exception("[NpcSpriteAnimation] Missing dependency: (SpriteRenderer)");
        }
        return spriteRenderer;
    }

    void Update()
    {
        if (directionChange) {
            StopAllCoroutines();
            StartCoroutine(ActionState());
            directionChange = false;
        }
    }

    public void SetDirection(SpriteDirection newDirection) {
        if (direction != newDirection) {
            directionChange = true;
        }
        direction = newDirection;
    }

    private Sprite[] GetCurrentAnimationFrames() {
        return idle[direction];
    }

    IEnumerator ActionState()
    {
        Sprite[] frames = GetCurrentAnimationFrames();        
        while (frame < frames.Length)
        {
            spriteRenderer.sprite = frames[frame];
            frame++;
            yield return new WaitForSeconds(idleInterval);
            yield return 0;

        }
        frame = 0;
        StartCoroutine(ActionState());
    }
}
