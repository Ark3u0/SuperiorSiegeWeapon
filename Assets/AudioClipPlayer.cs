using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
    //public audioClip
    private AudioSource myAudio;

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }




    // Update is called once per frame
    void Update()
    {
        
    }

    void playAudioSources()
    {
       // myAudio.PlayOneShot();
    }
}
