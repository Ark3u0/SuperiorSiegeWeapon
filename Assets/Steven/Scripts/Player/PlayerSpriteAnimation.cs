using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.gamasutra.com/blogs/VivekTank/20180710/321793/Multiple_ways_of_doing_sprite_sheet_animation_in_Unity3D.php
public class PlayerSpriteAnimation : MonoBehaviour
{
    public float walkInterval;
    public float idleInterval;
    public float kickInterval;
    public Sprite[] walkUp;
    public Sprite[] walkDown;
    public Sprite[] walkLeft;
    public Sprite[] walkRight;
    public Sprite[] idleUp;
    public Sprite[] idleDown;
    public Sprite[] idleLeft;
    public Sprite[] idleRight;
    public Sprite[] kickUp;
    public Sprite[] kickDown;
    public Sprite[] kickLeft;
    public Sprite[] kickRight;

    private SpriteRenderer spriteRenderer;
    private Dictionary<SpriteState, Dictionary<SpriteDirection, Sprite[]>> animations;
    private Dictionary<SpriteDirection, Sprite[]> walk;
    private Dictionary<SpriteDirection, Sprite[]> idle;
    private Dictionary<SpriteDirection, Sprite[]> kick;
    private int frame;
    private SpriteDirection direction;
    private SpriteState state;
    private bool directionChange;
    private bool stateChange;

    void Awake()
    {
        spriteRenderer = FindSpriteRendererOnObject();

        frame = 0;
        direction = SpriteDirection.UP;

        directionChange = false;
        stateChange = false;

        animations = new Dictionary<SpriteState, Dictionary<SpriteDirection, Sprite[]>>();
        walk = new Dictionary<SpriteDirection, Sprite[]>();
        idle = new Dictionary<SpriteDirection, Sprite[]>();
        kick = new Dictionary<SpriteDirection, Sprite[]>();

        walk.Add(SpriteDirection.UP, walkUp);
        walk.Add(SpriteDirection.DOWN, walkDown);
        walk.Add(SpriteDirection.LEFT, walkLeft);
        walk.Add(SpriteDirection.RIGHT, walkRight);

        idle.Add(SpriteDirection.UP, idleUp);
        idle.Add(SpriteDirection.DOWN, idleDown);
        idle.Add(SpriteDirection.LEFT, idleLeft);
        idle.Add(SpriteDirection.RIGHT, idleRight);

        kick.Add(SpriteDirection.UP, kickUp);
        kick.Add(SpriteDirection.DOWN, kickDown);
        kick.Add(SpriteDirection.LEFT, kickLeft);
        kick.Add(SpriteDirection.RIGHT, kickRight);

        animations.Add(SpriteState.WALK, walk);
        animations.Add(SpriteState.IDLE, idle);
        animations.Add(SpriteState.KICK, kick);
    }

    private SpriteRenderer FindSpriteRendererOnObject() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            Debug.LogError("[PlayerSpriteAnimation] expected SpriteRenderer to exist on gameObject. Please add SpriteRenderer to gameObject and rebuild.");
            throw new System.Exception("[PlayerSpriteAnimation] Missing dependency: (SpriteRenderer)");
        }
        return spriteRenderer;
    }

    void Start()
    {
        StartCoroutine(ActionState());
    }

   

    void Update()
    {
        if (stateChange) {
            frame = 0;

            StopAllCoroutines();
            StartCoroutine(ActionState());

            stateChange = false;
            directionChange = false;
            return;
        }

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

    public void SetState(SpriteState newState) {
        if (state != newState) {
            stateChange = true;
        }
        state = newState;
    }


    private Sprite[] GetCurrentAnimationFrames() {
        return animations[state][direction];
    }

    private float GetInterval() {
        switch (state) {
            case SpriteState.WALK:
                return walkInterval;
            case SpriteState.KICK:
                return kickInterval;
            case SpriteState.IDLE:
                return idleInterval;
            default:
                return 0.07f;
        }
    }

    IEnumerator ActionState()
    {
        Sprite[] frames = GetCurrentAnimationFrames();        
        while (frame < frames.Length)
        {
            spriteRenderer.sprite = frames[frame];
            frame++;
            yield return new WaitForSeconds(GetInterval());
            yield return 0;

        }
        frame = 0;
        StartCoroutine(ActionState());
    }
}