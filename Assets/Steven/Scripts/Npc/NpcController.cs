using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public DialogueReader dialogueReader;
    public DialogueManager dialogueManager;

    public bool ContinueConversation() {
        return dialogueManager.DisplayNextSentence();
    }

    public bool StartConversation(Transform player) {
        transform.LookAt(player);

        if (dialogueReader == null) return true;
        
        dialogueReader.ResetToInitial();
        return dialogueManager.StartDialogue(dialogueReader);
    }

    public void Answer(bool answer) {
        dialogueManager.Answer(answer);
    }

}
