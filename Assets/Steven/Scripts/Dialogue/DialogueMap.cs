using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMap
{
    public string name;
    public string initial;
    public Dictionary<string, Dialogue> map;

    public DialogueMap(DialogueList list) {
        map = new Dictionary<string, Dialogue>();
        name = list.name;
        initial = list.initial;

        foreach (Entry entry in list.entries) {
            map.Add(entry.key, entry.value);
        }
    }
}
