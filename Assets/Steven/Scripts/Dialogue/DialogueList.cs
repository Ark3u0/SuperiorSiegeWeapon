using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueList
{
    public string name;
    public string initial;
    public List<Entry> entries;

    public override string ToString() 
    {
        List<string> dialogueEntries = new List<string>();
        foreach (Entry dialogueEntry in entries) {
            dialogueEntries.Add(dialogueEntries.ToString());
        }

        return $"{{name: {name}, initial: {initial}, dialogue: {string.Join(",", dialogueEntries)}}}";
    }
}
