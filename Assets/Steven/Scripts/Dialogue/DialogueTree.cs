using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueTree : MonoBehaviour
{
    
    public TextAsset dialogueJson;

    private Dialogue root;
    private Dialogue dialogue;

    public void Awake()
    {
        root = JsonUtility.FromJson<Dialogue>(dialogueJson.text);
        dialogue = root;
    }

    public void ResetTree()
    {
        dialogue = root;
    }

    public void TakePath(bool answer) {
        dialogue = answer ? GetYesPath() : GetNoPath();
    }

    public Dialogue GetPath(bool answer) {
        return answer ? GetYesPath() : GetNoPath();
    }

    public Dialogue GetYesPath() {
        if (dialogue.options == null) {
            return null;
        }

        DialogueOption yesOption = dialogue.options.Find(opt => opt.text.ToLower().Equals("yes"));
        if (yesOption != null) {
            return yesOption.result;
        }
        return null;
    }

    public Dialogue GetNoPath() {
        if (dialogue.options == null) {
            return null;
        }

        DialogueOption noOption = dialogue.options.Find(opt => opt.text.ToLower().Equals("no"));
        if (noOption != null) {
            return noOption.result;
        }
        return null;
    }

    public List<string> GetSentences() {
        return dialogue.sentences;
    }

    public bool HasSentences() {
        return dialogue.sentences != null && dialogue.sentences.Count > 0;
    }

    public string GetName() {
        return dialogue.name;
    }

    public bool IsAnswerable() {
        return dialogue.options != null && dialogue.options.Count > 0;
    }
}
