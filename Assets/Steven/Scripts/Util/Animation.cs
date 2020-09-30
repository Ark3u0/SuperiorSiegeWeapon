using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.gamasutra.com/blogs/VivekTank/20180710/321793/Multiple_ways_of_doing_sprite_sheet_animation_in_Unity3D.php
public class SpriteAnimation : MonoBehaviour
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

    private Dictionary<Dictionary, Sprite[]> walk;
    private Dictionary<string, Sprite[]> idle;
    private Dictionary<string, Sprite[]> kick;

    public enum Direction {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    void Awake()
    {
        walk = new Dictionary<string, Sprite[]>();
        idle = new Dictionary<string, Sprite[]>();
        kick = new Dictionary<string, Sprite[]>();

        walk.Add(UP, walkUp);
        walk.Add(DOWN, walkDown);
        walk.Add(LEFT, walkLeft);
        walk.Add(RIGHT, walkRight);

        idle.Add(UP, idleUp);
        idle.Add(DOWN, idleDown);
        idle.Add(LEFT, idleLeft);
        idle.Add(RIGHT, idleRight);

        kick.Add(UP, kickUp);
        kick.Add(DOWN, kickDown);
        kick.Add(LEFT, kickLeft);
        kick.Add(RIGHT, kickRight);
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
        int i;
        i = 0;
        while (i < idle.Length)
        {
            spriteRenderer.sprite = idle[i];
            i++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;
                
        }
        StartCoroutine(Idle());
    }
    IEnumerator Walk()
    {
        int i;
        i = 0;
        while (i < walk.Length)
        {
            spriteRenderer.sprite = walk[i];
            i++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;
        }
        StartCoroutine(Walk());
    }

    IEnumerator Kick()
    {
        int i;
        i = 0;
        while (i < kick.Length)
        {
            spriteRenderer.sprite = kick[i];
            i++;
            yield return new WaitForSeconds(0.07f);
            yield return 0;

        }
        StartCoroutine(Kick());
    }
}
