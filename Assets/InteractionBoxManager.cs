using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBoxManager : MonoBehaviour
{
    public GameObject interactionBox;

    public void ShowInteraction() {
        interactionBox.SetActive(true);
    }

    public void HideInteraction() {
        interactionBox.SetActive(false);
    }
}
