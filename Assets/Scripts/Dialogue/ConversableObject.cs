using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversableObject : InteractableObject
{
    public InitialDialogue m_InitialDialogue;
    public bool playerIsClose;

    protected override void OnInteract()
    {
        if (!HasInteracted)
        {
            if (playerIsClose)
            {
                base.OnInteract();

                Debug.Log(m_InitialDialogue.m_Name + " is speaking...");
                // Start Dialoguing
                EncounterManager.Instance.StartEncounter(m_InitialDialogue, this, EncounterState.NPCTalking);
            }
        }
        // If alreading interacting, simulate "CONTINUE" button
        else
        {
            EncounterManager.Instance.ContinueEncounter();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered proximity");
            playerIsClose = true;
        }
    }
}
