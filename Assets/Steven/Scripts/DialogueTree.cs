using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueTree 
{
    public Dialogue dialogue;

    public bool endsWithQuestion;

    public DialogueTree answerYes;
    public DialogueTree answerNo;
}
