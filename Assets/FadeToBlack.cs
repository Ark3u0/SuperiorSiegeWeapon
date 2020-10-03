using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public bool StartBlack;
    public Image BlackImage;
    public float FadeTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        if (StartBlack)
        {
            BlackImage.canvasRenderer.SetAlpha(1f);
        }
        else
        {
            BlackImage.canvasRenderer.SetAlpha(0.0f);
        }
    }

    public void FadeIn()//turrn back
    {
        BlackImage.CrossFadeAlpha(1, 1, false);
    }

    public void FadeOut()//clear out the black
    {
        BlackImage.CrossFadeAlpha(0, 1, false);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
