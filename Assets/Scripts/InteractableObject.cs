using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Private class variables.
    protected bool HasInteracted = false;

    // --------------- Functions --------------- //

    protected virtual void OnCollided()
    {
        // Check for a button press
        if(Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }

    protected virtual void OnInteract()
    {
        // Check if object has been interacted with previously.
        if (!HasInteracted) 
        {
            HasInteracted = true;
            Debug.Log("Interacting with " + name);
        }
        else
        {
            Debug.Log("AlreadyInteracted");
        }
    }
}
