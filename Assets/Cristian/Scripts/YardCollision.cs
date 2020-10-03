using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YardCollision : MonoBehaviour
{

    public bool TargetHit = false;
    public Material HitColor;

    private AudioSource AudioPlayer;
    private AudioClip PreSolvedSound;
    private AudioClip SolvedSound;

    private void Start()
    {
        AudioPlayer = GetComponent<AudioSource>();
        SolvedSound = Resources.Load<AudioClip>("Sound-Generator-Fixed");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9f)// 9 is the ball layer
        {
            Debug.Log(gameObject.name + "is hit");
            //Target is hit true
            TargetHit = true;
            GetComponent<MeshRenderer>().sharedMaterial = HitColor;
            AudioPlayer.Stop();
           AudioPlayer.clip = SolvedSound;

           AudioPlayer.Play();
        }
        else
        {
            Debug.Log("Ball not in well");
        }
    }
}
