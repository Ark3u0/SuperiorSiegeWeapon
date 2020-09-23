using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public DialogueManager dialogueManager;

    public bool ContinueConversation() {
        return dialogueManager.DisplayNextSentence();
    }

    public bool StartConversation(Transform player) {
        transform.LookAt(player);
        return dialogueManager.StartDialogue(dialogueTree);
    }

}
