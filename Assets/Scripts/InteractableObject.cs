using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Class variables
    [NonSerialized]
    protected bool HasInteracted = false;

    [NonSerialized]
    protected bool PlayerIsClose = false;

    // --------------- Functions --------------- //

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
    protected void OnCollided()
    {
        // Check for a button press
        if (Input.GetKeyDown(KeyCode.E) && PlayerIsClose)
        {
            OnInteract();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player is colliding with the NPC.
        if (collision.CompareTag("Player"))
        {
            PlayerIsClose = true;
        }

        OnCollided();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the player is colliding with the NPC.
        if (collision.CompareTag("Player"))
        {
            PlayerIsClose = false;
        }
    }

    protected void Update()
    {
        if (PlayerIsClose)
        {
            OnCollided();
        }
    }
}
