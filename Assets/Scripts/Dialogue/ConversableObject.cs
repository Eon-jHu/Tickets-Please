using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ConversableObject : InteractableObject
{
    // Public class variables
    public bool m_ShouldUseChatGPT = true;

    public InitialDialogue m_InitialDialogue;
    public Dialogue m_TimeoutDialogue;
    public Temperament m_Temperament;

    [NonSerialized]
    public bool PlayerIsClose = false;

    [NonSerialized]
    public bool NPCInteractionComplete = false;

    // Serialised class variables
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameObject SpeechBubble;

    // Local variables
    private float m_InteractionCooldown = 0.5f;
    private float m_LastInteractTime = -1f;

    // --------------- Functions --------------- //
    private void Start()
    {
        // Generate a random Temperament for this NPC
        m_Temperament = new Temperament();
        m_Temperament.SetRandomTemperament();
    }

    protected override void OnInteract()
    {
        // Cooldown
        if (Time.time < m_LastInteractTime + m_InteractionCooldown)
        {
            return;
        }
        m_LastInteractTime = Time.time;


        if (NPCInteractionComplete)
        {
            return;
        }

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
                if (m_ShouldUseChatGPT)
                {
                    _ = EncounterManager.Instance.StartEncounter(m_InitialDialogue, this, EncounterState.NPCTalking);
                }
                else
                {
                    EncounterManager.Instance.StartEncounterNoLLM(m_InitialDialogue, this, EncounterState.NPCTalking);
                }

                // Speech Bubble Set to Disable
                SpeechBubble.SetActive(false);
            }
        }
        // If alreading interacting, simulate "CONTINUE" button
        else
        {
            EncounterManager.Instance.ContinueEncounter();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player is colliding with the NPC.
        if (collision.CompareTag("Player"))
        {
            PlayerIsClose = true;
        }

        base.OnCollided();
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
