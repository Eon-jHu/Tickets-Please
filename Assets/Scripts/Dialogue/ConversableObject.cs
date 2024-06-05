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
            DialogueManager.Instance.StartDialogue(m_Dialogue);
        }
        // If alreading interacting, simulate "CONTINUE" button
        else
        {
            DialogueManager.Instance.DisplayNextSentence();
        }
    }
}
