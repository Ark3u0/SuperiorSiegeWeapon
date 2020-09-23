using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private DialogueManager dialogueManager;

    void Start() {
        dialogueManager = new DialogueManager();
    }

    public void TriggerDialogue() 
    {
        // TODO: Singleton pattern
       dialogueManager.StartDialogue(dialogue);
    }

}
