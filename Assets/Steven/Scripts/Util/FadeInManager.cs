using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=cmYpkqrqhrA
public class FadeInManager : MonoBehaviour
{
    public Image fadeImage;
    public Color startColor;
    public Color endColor;
    public float duration;
    public bool isFading;

    void Start() {
        FadeIn();
    }

    public void FadeIn() {
        StartCoroutine(BeginFade());
    }

    private IEnumerator BeginFade() 
    {
        isFading = true;

        float timer = 0f;

        while (timer <= duration) 
        {
            fadeImage.color = Color.Lerp(startColor, endColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
