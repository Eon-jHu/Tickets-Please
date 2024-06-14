using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversableObject : InteractableObject
{
    public InitialDialogue m_InitialDialogue;
    public bool PlayerIsClose;

    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject SpeechBubble;

    private bool NPCInteractionComplete = false;

    protected override void OnInteract()
    {
        if (!HasInteracted)
        {
            // Ensure player is within ranger of the NPC before talking.
            if (PlayerIsClose)
            {
                base.OnInteract();

                Debug.Log(m_InitialDialogue.m_Name + " is speaking...");

                // Set the current NPC for interaction to this instance only.
                dialogueManager.SetCurrentNPC(this);

                // Start Dialoguing
                EncounterManager.Instance.StartEncounter(m_InitialDialogue, this, EncounterState.NPCTalking);

                // Set interaction as complete.
                NPCInteractionComplete = true;
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
        // Check if the player is colliding with the NPC.
        if (other.CompareTag("Player"))
        {
            PlayerIsClose = true;
        }
    }

    protected override void Update()
    {
        base.Update();

        // If NPC interaction is finished, hide the speech bubble.
        if (NPCInteractionComplete)
        {
            SpeechBubble.SetActive(false);
        }
        else
        {
            // Do nothing, continue to show speech bubble.
        }
    }
}
