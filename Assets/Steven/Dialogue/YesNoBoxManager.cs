using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesNoBoxManager : MonoBehaviour
{
    public Animator boxAnimator;
    public Animator cursorAnimator;
    public void StartAnswer() 
    {
        boxAnimator.SetBool("IsOpen", true);
        cursorAnimator.SetBool("Answer", true);
    }

    public void EndAnswer()
    {
        boxAnimator.SetBool("IsOpen", false);
    }

    public void SetAnswer(bool answer)
    {   
        cursorAnimator.SetBool("Answer", answer);
    }

    public bool IsOpen()
    {
        return boxAnimator.GetBool("IsOpen");
    }

    public bool GetAnswer()
    {
        return cursorAnimator.GetBool("Answer");
    }
}
