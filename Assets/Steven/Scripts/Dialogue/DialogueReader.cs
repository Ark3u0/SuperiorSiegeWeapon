using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueReader : MonoBehaviour
{
    
    public TextAsset dialogueJson;
    private DialogueMap root;
    private Dialogue current;

    public void Awake()
    {
        Debug.Log(dialogueJson.text);
        DialogueList list = JsonUtility.FromJson<DialogueList>(dialogueJson.text);
        Debug.Log(list.ToString());
        root = new DialogueMap(list);
        current = root.map[root.initial];
    }

    public void ResetToInitial()
    {
        current = root.map[root.initial];
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
