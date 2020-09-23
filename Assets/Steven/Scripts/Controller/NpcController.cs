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
        dialogueTree.ResetTree();
        return dialogueManager.StartDialogue(dialogueTree);
    }

    public void SetAnswer(bool answer) {
        dialogueManager.SetAnswer(answer);
    }

}
