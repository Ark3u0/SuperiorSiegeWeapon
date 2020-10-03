using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditScreen : MonoBehaviour, Fadeable
{
    public float MoveRate;
    public Image BackGroundImage;
    public FadeOutManager fadeOutManager;
    public float scrollTo;
    private bool Movebackground;
    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartMovingBackground());
    }

    // Update is called once per frame
    void Update()
    {
        startY = BackGroundImage.transform.position.y;
        if (Movebackground)
        {
            MoveImage();
        }
    }

    public void MoveImage()
    {
        BackGroundImage.transform.position = new Vector3(BackGroundImage.transform.position.x,
        BackGroundImage.transform.position.y + MoveRate, BackGroundImage.transform.position.z);

        Debug.Log("Y Value of the image " + BackGroundImage.transform.position.y);
        
        if (BackGroundImage.transform.position.y > scrollTo) {
            Movebackground = false;
            StartCoroutine(EndMovingBackground());
        }
    }

    private IEnumerator StartMovingBackground()
    {
        yield return new WaitForSeconds(3f);
        Movebackground = true;
    }

    private IEnumerator EndMovingBackground()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("load to main menu ");

        GameObject endingMusic = GameObject.Find("EndingMusic");
        if (endingMusic) {
            AudioSource music = endingMusic.GetComponent<AudioSource>();
            while (music.isPlaying) {
                yield return null;
            }
            Destroy(endingMusic);
        }
        
        fadeOutManager.FadeOut(this);
    }

    public void RunPostFade() {
        SceneManager.LoadScene("CG_PrototypeLevel_MainMenu");
    }
}
