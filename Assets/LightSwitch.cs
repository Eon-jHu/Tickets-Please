using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : InteractableObject
{
    public GameObject darknessShade;

    private void Start()
    {
        darknessShade.SetActive(false);
    }

    protected override void OnInteract()
    {
        // Debug
        Debug.Log("Interacting with " + name + " - Lights Off");

        if (!HasInteracted)
        {
            // Toggle the light off
            darknessShade.SetActive(!darknessShade.activeSelf);

            if (darknessShade.activeSelf)
            {

                StoryManager.Instance.AddStoryEvent("the player has turned off the lights in the carriage");
            }
            else
            {

                StoryManager.Instance.StoryEvent = "";
            }   
        }
    }
}
