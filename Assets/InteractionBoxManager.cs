using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractionBoxManager : MonoBehaviour
{
    public GameObject interactionBox;
    public Sprite aButton;
    public Sprite rightTrigger;
    public Text interaction;
    public Image input;

    private Sprite GetSprite(string interactionText) {
        switch (interactionText.ToLower())
        {
            case "aim":
            case "kick":
                return rightTrigger;
            case "talk":
                return aButton;
            default:
                return null;
        }
    }

    public void ShowInteraction(string interactionText) {
        interactionBox.SetActive(true);
        interaction.text = interactionText;
        input.sprite = GetSprite(interactionText);
    }

    public void HideInteraction() {
        interactionBox.SetActive(false);
    }
}
