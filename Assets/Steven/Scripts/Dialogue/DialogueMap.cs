using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMap
{
    public string name;
    public List<InitialConditionsPath> initial;
    public Dictionary<string, Dialogue> map;

    public DialogueMap(DialogueList list) {
        map = new Dictionary<string, Dialogue>();
        name = list.name;
        initial = list.initial;

        foreach (Entry entry in list.entries) {
            map.Add(entry.key, entry.value);
        }
    }

    public Dialogue Initialize() {
        foreach (InitialConditionsPath initialConditionPath in initial) {
            if (initialConditionPath.conditions.Count == 0) {
                return map[initialConditionPath.path];
            }
        }
        return null;
    }

    public Dialogue Initialize(PlayerController player) {
        foreach (InitialConditionsPath initialConditionsPath in initial) {
            if (player.AreConditionsMet(initialConditionsPath.conditions)) {
                return map[initialConditionsPath.path];
            }
        }
        return null;
    }
}
