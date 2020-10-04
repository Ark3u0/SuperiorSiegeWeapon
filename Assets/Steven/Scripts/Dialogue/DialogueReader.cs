using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueReader
{
    private DialogueMap root;
    private Dialogue current;

    public DialogueReader(TextAsset asset)
    {
        DialogueList list = JsonUtility.FromJson<DialogueList>(asset.text);
        
        root = new DialogueMap(list);
        current = root.Initialize();
    }

    public bool CheckForConditionGain()
    {
        return current.condition != null;
    }

    public void GainCondition(PlayerController player, NpcAlertManager npcAlertManager)
    {
        player.AddCondition(current.condition);
        Debug.Log(current.condition);
        npcAlertManager.TriggerAlerts(current.condition);
    }

    public bool CheckForNextDialogue()
    {
        return current.next != null;
    }

    public void NextDialogue() {
        current = root.map[current.next];
    }

    public void ResetToInitial(PlayerController player)
    {
        current = root.Initialize(player);
    }

    public void TakePath(bool answer) {
        current = answer ? GetYesPath() : GetNoPath();
    }

    public Dialogue GetPath(bool answer) {
        return answer ? GetYesPath() : GetNoPath();
    }

    public Dialogue GetYesPath() {
        if (current.select == null) {
            return null;
        }

        return root.map[current.select.yes];
    }

    public Dialogue GetNoPath() {
        if (current.select == null) {
            return null;
        }

        return root.map[current.select.no];
    }

    public List<string> GetSentences() {
        return current.sentences;
    }

    public bool HasSentences() {
        return current.sentences != null && current.sentences.Count > 0;
    }

    public string GetName() {
        return root.name;
    }

    public bool IsAnswerable() {
        return current.select != null && current.select.yes != null && current.select.no != null;
    }
}
