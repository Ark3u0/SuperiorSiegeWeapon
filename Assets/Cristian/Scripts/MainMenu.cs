using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //https://www.youtube.com/watch?v=-GWjA6dixV4

    public void Playgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenue()
    {
        SceneManager.LoadScene("CG_PrototypeLevel_MainMenue");
    }

    public void LoadEndScreen()
    {
      //  SceneManager.LoadScene("Final_EndScren");
    }

    public void LoadCredits()
    {
       // SceneManager.LoadScene("Final_Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
