using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversableObject : InteractableObject
{
    [SerializeField] private Dialogue m_Dialogue;

    protected override void OnInteract()
    {
        if (!HasInteracted)
        {
            base.OnInteract();

            Debug.Log(m_Dialogue.m_Name + " is speaking...");
            // Start Dialoguing
            EncounterManager.Instance.StartEncounter(m_Dialogue, this, EncounterState.NPCTalking);
        }
        // If alreading interacting, simulate "CONTINUE" button
        else
        {
            EncounterManager.Instance.ContinueEncounter();
        }
    }
}
