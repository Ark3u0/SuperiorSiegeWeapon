using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public TextAsset dialogueJson;
    private DialogueReader dialogueReader;
    private DialogueManager dialogueManager;

    void Awake() {
        dialogueManager = FindDialogueManagerInScene();
        dialogueReader = new DialogueReader(dialogueJson);
    }

    private DialogueManager FindDialogueManagerInScene() {
        DialogueManager dm = GameObject.FindObjectOfType<DialogueManager>();
        if (dm == null) {
            Debug.LogError("[NpcController] expected DialogueManager to exist in scene. Please add DialogueManager and required dependencies to scene and rebuild.");
            throw new System.Exception("[NpcController] Missing dependency: (DialogueManager)");
        }
        return dm;
    }

    public bool ContinueConversation() {
        return dialogueManager.DisplayNextSentence();
    }

    public bool StartConversation(Transform player) {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (dialogueReader == null) return true;
        
        dialogueReader.ResetToInitial();
        return dialogueManager.StartDialogue(dialogueReader);
    }

    public void Answer(bool answer) {
        dialogueManager.Answer(answer);
    }

}
