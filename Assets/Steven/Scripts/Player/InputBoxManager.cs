using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputBoxManager : MonoBehaviour
{
    public GameObject[] inputBoxes;
    public Sprite[] inputs;

    private GameObject GetInputBox(List<string> possibleInputs) {
        if (possibleInputs.Count > inputBoxes.Length) {
            return inputBoxes[inputBoxes.Length - 1];
        }
        return inputBoxes[possibleInputs.Count - 1];
    }

    private void ShowCorrectInputBox(List<string> possibleInputs) {
        for (int i = 0; i < inputBoxes.Length; i++)
        {
            if (i + 1 == possibleInputs.Count) {
                inputBoxes[i].SetActive(true);
            } else {
                inputBoxes[i].SetActive(false);
            }
        }
    }

    private Sprite GetInputSprite(string input)
    {
        switch (input.ToLower()) {
            case "talk":
                return inputs[0]; // A Button
            case "angle ↓":
                return inputs[1]; // Left Bumper\
            case "angle ↑":
                return inputs[2]; // Right Bumper
            case "aim":
                return inputs[3]; // Right Trigger
            case "kick":
                return inputs[3]; // Right Trigger
            case "target":
                return inputs[4]; // Left Thumbstick
            
            default:
                return null;
        }
    }

    public void ShowInputs(List<string> possibleInputs) {
        ShowCorrectInputBox(possibleInputs);
        GameObject interactionBox = GetInputBox(possibleInputs);

        for (int i = 0; i < interactionBox.transform.childCount; i++ )
        {
            Transform interaction = interactionBox.transform.GetChild(i);
            Image inputButton = interaction.GetChild(0).GetComponent<Image>();
            Text inputText = interaction.GetChild(1).GetComponent<Text>();

            inputText.text = possibleInputs[i];
            inputButton.sprite = GetInputSprite(possibleInputs[i]);
        }
    }

    public void HideInputs() {
        foreach (GameObject inputBox in inputBoxes) {
            inputBox.SetActive(false);
        }
    }
}
