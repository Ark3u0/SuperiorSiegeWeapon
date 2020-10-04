using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public string npcName;
    public TextAsset dialogueJson;
    private DialogueReader dialogueReader;
    private DialogueManager dialogueManager;
    private EnvironmentSpriteAnimation exclamationSprite;
    public bool startWithAlert = false;

    void Awake() {
        dialogueManager = FindDialogueManagerInScene();
        dialogueReader = new DialogueReader(dialogueJson);
        exclamationSprite = GetComponentInChildren<EnvironmentSpriteAnimation>();

        if (startWithAlert) {
            ShowExclamationMark();
        } else {
            HideExclamationMark();
        }
    }

    private DialogueManager FindDialogueManagerInScene() {
        DialogueManager dm = GameObject.FindObjectOfType<DialogueManager>();
        if (dm == null) {
            Debug.LogError("[NpcController] expected DialogueManager to exist in scene. Please add DialogueManager and required dependencies to scene and rebuild.");
            throw new System.Exception("[NpcController] Missing dependency: (DialogueManager)");
        }
        return dm;
    }

    public bool ContinueConversation(PlayerController player) {
        return dialogueManager.DisplayNextSentence(player);
    }

    public void ShowExclamationMark() {
        exclamationSprite.transform.localScale = new Vector3(1, 1, 1);
    }

    public void HideExclamationMark() {
        exclamationSprite.transform.localScale = new Vector3(0, 0, 0);
    }

    public bool StartConversation(PlayerController player) {
        HideExclamationMark();

        Vector3 playerPosition = player.transform.position;
        transform.LookAt(new Vector3(playerPosition.x, transform.position.y, playerPosition.z));

        if (dialogueReader == null) return true;
        
        dialogueReader.ResetToInitial(player);
        return dialogueManager.StartDialogue(player, dialogueReader);
    }

    public void Answer(bool answer) {
        dialogueManager.Answer(answer);
    }

    // need a check for the end of the game with abuelita 

}
