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

    [SerializeField]
    public Button m_ContinueButton;

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
        // TODO: start encounters

        m_DialogueManager.StartDialogue(_dialogue, _npc);
    }

    public void ContinueEncounter()
    {
        m_DialogueManager.DisplayNextSentence();
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
