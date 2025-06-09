// EncounterManager.cs:
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq; // <-- HEY! SENPAI! Look! You need this!

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

    private readonly List<Response> ChatResponses = new();

    private Task GenerateResponsesTask;

    private bool m_UseLLM = true;

    // ----------------------- Singleton -----------------------
    public static EncounterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            // Senpai, for a singleton, it's usually 'Instance = this;'
            // But FindObjectOfType might work if there's only one... I guess.
            Instance = FindObjectOfType<EncounterManager>();
        }
        else if (Instance != FindObjectOfType<EncounterManager>()) // And here, 'Instance != this' and 'Destroy(gameObject)'
        {
            Destroy(FindObjectOfType<EncounterManager>());
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    // ---------------------------------------------------------

    public void StartEncounterNoLLM(Dialogue _dialogue, ConversableObject _npc, EncounterState _eState)
    {
        // Disable Player Movement
        PlayerController.Instance.m_PlayerMovement.bCanMove = false;

        // Set the state
        m_EState = _eState;

        // Set using LLM
        m_UseLLM = false;

        if (m_EState == EncounterState.NPCTalking)
        {
            m_DialogueManager.StartDialogue(_dialogue, _npc);
        }
        // Timeout dialogue
        else
        {
            m_DialogueManager.ContinueDialogue(_dialogue, _npc);
        }
    }

    public async Task StartEncounter(Dialogue _dialogue, ConversableObject _npc, EncounterState _eState)
    {
        // Disable Player Movement
        PlayerController.Instance.m_PlayerMovement.bCanMove = false;

        // Set the state
        m_EState = _eState;

        // Set using LLM
        m_UseLLM = true;

        if (m_EState == EncounterState.NPCTalking)
        {
            string prompt = StoryManager.Instance.CraftDialoguePrompt(_npc, PlayerController.Instance);
            string chatGPTResponse = await ChatGPTManager.Instance.AskChatGPT(_npc.m_InitialDialogue.m_Name, prompt);

            if (!string.IsNullOrEmpty(chatGPTResponse))
            {
                // Debug
                Debug.Log($"EncounterManager: ChatGPT generated dialogue for {_npc.m_InitialDialogue.m_Name}: \"{chatGPTResponse}\"");

                // Split prompt into sentences
                _dialogue.m_Sentences = ChatGPTManager.Instance.SplitIntoSentences(chatGPTResponse);

                // Async get responses in the meantime
                GenerateResponsesTask = GetResponses(_npc);

                // And finaly, start the dialogue
                m_DialogueManager.StartDialogue(_dialogue, _npc);

            }
        }
        // Timeout dialogue
        else
        {
            m_DialogueManager.ContinueDialogue(_dialogue, _npc);
        }
    }

    public async Task GetResponses(ConversableObject _npc)
    {
        string quotedResponses = await ChatGPTManager.Instance.AskChatGPT(_npc.m_InitialDialogue.m_Name, StoryManager.Instance.CraftResponsesPrompt());
        string[] splitResponses = ChatGPTManager.Instance.ExtractQuotedStrings(quotedResponses);

        // For each of the [4] response options
        int index = 0;
        foreach (string s in splitResponses)
        {
            // Create a new response dialogue sequence
            Response newResponse = new()
            {
                m_PlayerResponse = s,
                m_ReplyToResponse = new(),
                m_Success = true,
                m_AttitudeValueModifier = index == 0 ? -1 : index >= 3 ? 1 : 0
            };

            string prompt = StoryManager.Instance.CraftDialoguePromptContinued(newResponse);
            string chatGPTResponse = await ChatGPTManager.Instance.AskChatGPT(_npc.m_InitialDialogue.m_Name, prompt);
            newResponse.m_ReplyToResponse.m_Sentences = ChatGPTManager.Instance.SplitIntoSentences(chatGPTResponse);

            ChatResponses.Add(newResponse);

            // Increment index
            index++;

            // Debug
            Debug.Log($"EncounterManager: ChatGPT generated response: \"{newResponse.m_PlayerResponse}\" with reply: \"{string.Join(" ", newResponse.m_ReplyToResponse.m_Sentences)}\"");
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
                // If we're not using LLMs
                if (!m_UseLLM)
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
                    if (GenerateResponsesTask != null)
                    {
                        // If the responses have finished loading
                        while (!GenerateResponsesTask.IsCompleted)
                        {
                            // Wait for the task to complete
                            Debug.Log("Waiting for response task to complete...");
                        }

                        Response[] responses = ChatResponses.ToArray();

                        // Start responding
                        m_EState = EncounterState.PlayerResponding;
                        StartResponding(responses);

                    }
                    else
                    {
                        // If the responses are still loading, wait for them
                        Debug.Log("No response task detected.");
                    }
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
            if (!m_UseLLM)
            {
                // If there is a (last) reply to that response...
                if (m_ResponseButtons[_buttonIndex].m_Response.m_ReplyToResponse != null)
                {
                    // Start a new (final) encounter
                    StartEncounterNoLLM(
                        m_ResponseButtons[_buttonIndex].m_Response.m_ReplyToResponse,
                        m_DialogueManager.m_DialogueSpace.m_ConversingObject,
                        m_EState);
                }
            }
            else
            {
                // Change player attitude based on the response
                PlayerController.Instance.PlayerAttitude.AttitudeValue += m_ResponseButtons[_buttonIndex].m_Response.m_AttitudeValueModifier;

                // Start a new (final) encounter
                _ = StartEncounter(
                    m_ResponseButtons[_buttonIndex].m_Response.m_ReplyToResponse,
                    m_DialogueManager.m_DialogueSpace.m_ConversingObject,
                    m_EState);
            }

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

    private void TimeoutEncounter()
    {
        // Disable Game Objects
        foreach (ResponseButton responseButton in m_ResponseButtons)
        {
            responseButton.gameObject.SetActive(false);
        }

        // Start a new (final) encounter
        StartEncounterNoLLM(m_DialogueManager.m_DialogueSpace.m_ConversingObject.m_TimeoutDialogue, m_DialogueManager.m_DialogueSpace.m_ConversingObject, m_EState);

        // Show continue button
        m_ContinueButton.gameObject.SetActive(true);
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

        // Set Interaction Complete to TRUE
        m_DialogueManager.CurrentNPC.NPCInteractionComplete = true;

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

    void Update()
    {
        switch (m_EState)
        {
            case EncounterState.NPCTalking:
                break;

            case EncounterState.PlayerResponding:

                if (m_TimerSlider.remainingTime <= 0)
                {
                    m_EState = EncounterState.Loss;
                    TimeoutEncounter();

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