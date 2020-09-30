using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.gamasutra.com/blogs/VivekTank/20180710/321793/Multiple_ways_of_doing_sprite_sheet_animation_in_Unity3D.php
public class Animation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
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

    private Dictionary<SpriteDirection, Sprite[]> walk;
    private Dictionary<SpriteDirection, Sprite[]> idle;
    private Dictionary<SpriteDirection, Sprite[]> kick;
    private int frame;

    void Awake()
    {
        frame = 0;

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
    }

    void Start()
    {
        StartCoroutine(Idle());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StopAllCoroutines();
            StartCoroutine(Idle());
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StopAllCoroutines();
            StartCoroutine(Kick());
        }
        if (Input.GetKeyDown(KeyCode.W))
      {
            StopAllCoroutines();
            StartCoroutine(Walk());
        }
    }
    IEnumerator Idle()
    {
        frame = 0;
        while (frame < idle.Length)
        {
            spriteRenderer.sprite = idle[frame];
            frame++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;
                
        }
        StartCoroutine(Idle());
    }
    IEnumerator Walk()
    {
        frame = 0;
        while (frame < walk.Length)
        {
            spriteRenderer.sprite = walk[frame];
            frame++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;
        }
        StartCoroutine(Walk());
    }

    IEnumerator Kick()
    {
        frame = 0;
        while (frame < kick.Length)
        {
            spriteRenderer.sprite = kick[frame];
            frame++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;

        }
        StartCoroutine(Kick());
    }
}
