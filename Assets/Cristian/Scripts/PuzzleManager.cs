using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private PlayerController playerController;

    void Awake() {
        playerController = FindPlayerControllerInScene();
        Puzzle[] puzzles = FindObjectsOfType<Puzzle>();

        foreach (Puzzle puzzle in puzzles) {
            puzzle.SetAddConditionCallback((condition) => AddConditionCallback(condition));
            puzzle.SetAreConditionsMetCallback((conditions) => AreConditionsMetCallback(conditions));
        }
    }

    public void AddConditionCallback(string condition) {
        playerController.AddCondition(condition);
    }

    public bool AreConditionsMetCallback(List<string> conditions) {
        return playerController.AreConditionsMet(conditions);
    }

    private PlayerController FindPlayerControllerInScene() {
        PlayerController pa = FindObjectOfType<PlayerController>();
        if (pa == null) {
            Debug.LogError("[PuzzleManager] expected PlayerController to exist in scene. Please add PlayerController to scene and rebuild.");
            throw new System.Exception("[PuzzleManager] Missing dependency: (PlayerController)");
        }
        return pa;
    }
}
