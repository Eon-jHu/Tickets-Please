using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversableObject : InteractableObject
{
    public InitialDialogue m_InitialDialogue;

    protected override void OnInteract()
    {
        if (!HasInteracted)
        {
            base.OnInteract();

            Debug.Log(m_InitialDialogue.m_Name + " is speaking...");
            // Start Dialoguing
            EncounterManager.Instance.StartEncounter(m_InitialDialogue, this, EncounterState.NPCTalking);
        }
        // If alreading interacting, simulate "CONTINUE" button
        else
        {
            EncounterManager.Instance.ContinueEncounter();
        }
    }
}
