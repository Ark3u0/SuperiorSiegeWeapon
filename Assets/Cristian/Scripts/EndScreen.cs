using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndScreen : MonoBehaviour
{
    public float MoveRate;
    public GameObject BackGroundImage;
    private bool Movebackground;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartMovingBackground());
    }

    // Update is called once per frame
    void Update()
    {
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
        
        if (BackGroundImage.transform.position.y < -75f)
        {
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
        Debug.Log("load to credits " );

        //now load the credits screen 
        SceneManager.LoadScene("CG_PrototypeLevel_Credits");


    }

    public void LoadCredits()
    {
       // SceneManager.LoadScene("CG_PrototypeLevel_MainMenue");
    }
}
