using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EncounterState
{ 
    NPCTalking,
    PlayerResponding,
    Loss,
    Win,
    None
}


public class EncounterManager : MonoBehaviour
{
    [SerializeField]
    public DialogueManager m_DialogueManager;

    [SerializeField]
    public ResponseButton[] m_ResponseButtons;

    [SerializeField]
    public TimerSlider m_TimerSlider;

    [NonSerialized]
    public EncounterState m_EState = EncounterState.None;

    private bool m_NPCIsDoneTalking = false;

    [SerializeField]
    private Button m_ContinueButton;

    // ----------------------- Singleton -----------------------
    public static EncounterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<EncounterManager>();
        }
        else if (Instance != FindObjectOfType<EncounterManager>())
        {
            Destroy(FindObjectOfType<EncounterManager>());
        }
    }


    // OnDestroy is called when the object is destroyed
    private void OnDestroy()
    {
        Instance = null;
    }
    // ---------------------------------------------------------

    public void StartEncounter(Dialogue _dialogue, ConversableObject _npc, EncounterState _eState)
    {
        // Disable Player Movement
        PlayerController.Instance.m_PlayerMovement.bCanMove = false;

        // Set the state
        m_EState = _eState;

        if (m_EState == EncounterState.NPCTalking)
        {
            m_DialogueManager.StartDialogue(_dialogue, _npc);
        }
        // Finalizes encounter
        else
        {
            m_DialogueManager.ContinueDialogue(_dialogue, _npc);
        }
    }

    public void ContinueEncounter()
    {
        Debug.Log("Current Encounter State = " + m_EState);

        // Do nothing if waiting for player to respond
        if (m_EState != EncounterState.NPCTalking && m_EState != EncounterState.Win && m_EState != EncounterState.Loss)
        {
            return;
        }

        m_NPCIsDoneTalking = m_DialogueManager.DisplayNextSentence();

        // When the NPC is done talking
        if (m_NPCIsDoneTalking)
        {
            // If the state was they were yappin'
            if (m_EState == EncounterState.NPCTalking)
            {
                // If it contains a Response for the player to make
                Response[] responses = m_DialogueManager.m_DialogueSpace.m_ConversingObject.m_InitialDialogue.m_Responses;
                if (responses != null)
                {
                    // Start responding
                    m_EState = EncounterState.PlayerResponding;
                    StartResponding(responses);
                }
            }
            else
            {
                EndEncounter();
            }
        }
       
    }

    public void OnResponseButtonClick(int _buttonIndex)
    {
        // Check victory condition for the encounter/conversation
        if (m_ResponseButtons[_buttonIndex].m_IsSuccessfulReply)
        {
            m_EState = EncounterState.Win;
        }
        else
        {
            m_EState = EncounterState.Loss;
        }

        // If there is a (last) reply to that response...
        if (m_ResponseButtons[_buttonIndex].m_Response.m_ReplyToResponse != null)
        {
            // Start a new (final) encounter
            StartEncounter(m_ResponseButtons[_buttonIndex].m_Response.m_ReplyToResponse, m_DialogueManager.m_DialogueSpace.m_ConversingObject, m_EState);

            // Show continue button
            m_ContinueButton.gameObject.SetActive(true);
        }
        else
        {
            m_DialogueManager.EndDialogue();
        }

        // Hide the buttons and timer
        foreach (ResponseButton responseButton in m_ResponseButtons)
        {
            responseButton.gameObject.SetActive(false);
            m_TimerSlider.gameObject.SetActive(false);
        }
    }

    private void StartResponding(Response[] responses)
    {
        // Hide continue button
        m_ContinueButton.gameObject.SetActive(false);

        // Change and activate the buttons
        for (int i = 0; i < responses.Length; i++)
        {
            Debug.Log("Button Number: " + i);
            Debug.Log(responses[i].m_PlayerResponse);
            Debug.Log(responses[i].m_Success);

            m_ResponseButtons[i].gameObject.SetActive(true);
            m_ResponseButtons[i].m_Response = responses[i];
            m_ResponseButtons[i].m_ButtonText.text = responses[i].m_PlayerResponse;
            m_ResponseButtons[i].m_IsSuccessfulReply = responses[i].m_Success;
        }

        // NPC is done talking (double-set)
        m_NPCIsDoneTalking = true;

        // Activate the timer
        m_TimerSlider.gameObject.SetActive(true);
    }

    private void EndEncounter()
    {
        // Disable Game Objects
        m_TimerSlider.gameObject.SetActive(false);

        foreach (ResponseButton responseButton in m_ResponseButtons)
        {
            responseButton.gameObject.SetActive(false);
        }

        // Disable the speech bubble.

        // End Dialogue
        m_DialogueManager.EndDialogue();

        // Enable Player Movement
        PlayerController.Instance.m_PlayerMovement.bCanMove = true;

        Debug.Log("The encounter ended with code " + m_EState);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Disable all objects until it is dialogin' time
        m_TimerSlider.gameObject.SetActive(false);
        foreach (ResponseButton responseButton in m_ResponseButtons)
        {
            responseButton.gameObject.SetActive(false);
        }
    }

    void UpdateMode(EncounterState _eState)
    {
        switch (_eState)
        {
            case EncounterState.NPCTalking:
                break;

            case EncounterState.PlayerResponding:

                if (m_TimerSlider.remainingTime <= 0)
                {
                    m_EState = EncounterState.Loss;
                    EndEncounter();

                    Debug.Log("ENCOUNTER ENDED DUE TO TIMEOUT");
                }

                break;

            case EncounterState.Loss:
                break;

            case EncounterState.Win:
                break;

            case EncounterState.None:
                break;

            default:
                break;
        }
    }
}
