using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : CollidableObject
{
    // private class variables.
    private bool HasInteracted = false;

    // --------------- Functions --------------- //

    protected override void OnCollided(GameObject _CollidedObject)
    {
        // Check for a button push.
        if(Input.GetKey(KeyCode.E))
        {
            OnInteract();
        }

        // Reset HasInteracted when button is released.
        if (Input.GetKeyUp(KeyCode.E))
        {
            HasInteracted = false;
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
            // Logic for interacting multiple times if neccesary.
        }
      
    }
}
