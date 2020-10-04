using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMap
{
    public Dictionary<string, Alert> map;

    public AlertMap(AlertList list) {
        map = new Dictionary<string, Alert>();
    
        foreach (AlertEntry entry in list.alerts) {
            map.Add(entry.condition, entry.alert);
        }
    }

    public Alert GetAlertForCondition(string condition) {
        return map[condition];
    }
}
