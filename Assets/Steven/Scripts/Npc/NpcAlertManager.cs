using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAlertManager : MonoBehaviour
{
    public TextAsset alertsJson;

    private AlertMap alertsByCondition;
    private PlayerController player;
    private Dictionary<string, NpcController> npcByName;

    void Awake()
    {
        alertsByCondition = new AlertMap(JsonUtility.FromJson<AlertList>(alertsJson.text));
        
        NpcController[] npcs = GameObject.FindObjectsOfType<NpcController>();
        player = GameObject.FindObjectOfType<PlayerController>();

        npcByName = new Dictionary<string, NpcController>();
        
        foreach (NpcController npc in npcs) {
            npcByName.Add(npc.npcName.ToLower(), npc);
        }
    }

    public void TriggerAlerts(string condition) {
        Alert alert = alertsByCondition.GetAlertForCondition(condition);
        if (alert.composite != null) {
            bool conditionsMet = player.AreConditionsMet(alert.composite);
            if (conditionsMet && alert.triggers != null) {
                foreach (string trigger in alert.triggers) {
                    npcByName[trigger.ToLower()].ShowExclamationMark();
                }
            }
        } else {
            if (alert.triggers != null) {
                foreach (string trigger in alert.triggers) {
                    npcByName[trigger.ToLower()].ShowExclamationMark();
                }
            }
        }
    }
}
