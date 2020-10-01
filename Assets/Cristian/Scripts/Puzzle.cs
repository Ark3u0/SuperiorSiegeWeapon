using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    private Action<string> addCondition;
    private Func<List<string>, bool> areConditionsMet;

    public void SetAddConditionCallback(Action<string> addConditionCallback) {
        addCondition = addConditionCallback;
    }

    public void SetAreConditionsMetCallback(Func<List<string>, bool> areConditionsMetCallback) {
        areConditionsMet = areConditionsMetCallback;
    }

    protected void AddCondition(string condition) {
        addCondition.Invoke(condition);
    }

    protected bool AreConditionsMet(List<string> conditions) {
        return areConditionsMet.Invoke(conditions);
    }
}
