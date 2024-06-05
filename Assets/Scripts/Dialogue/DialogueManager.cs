using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public DialogueSpace m_DialogueSpace;
    public TextMeshProUGUI m_NameText;
    public TextMeshProUGUI m_DialogueText;
    public Animator m_Animator;

    private Queue<string> m_SentencesQueue;
    private Dialogue m_CurrentDialogue = null;
    private string m_CurrentSentence = null;
    private bool m_IsTyping = false;


    // ----------------------- Singleton -----------------------
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<DialogueManager>();
        }
        else if (Instance != FindObjectOfType<DialogueManager>())
        {
            Destroy(FindObjectOfType<DialogueManager>());
        }
    }


    // OnDestroy is called when the object is destroyed
    private void OnDestroy()
    {
        Instance = null;
    }
    // ---------------------------------------------------------

    public void StartDialogue(Dialogue _dialogue, ConversableObject _npc)
    {
        Debug.Log("Entering dialogue...");

        // Enable the Dialogue Space
        m_DialogueSpace.Enable(_npc);

        // Set animation options
        m_Animator.SetBool("IsOpen", true);

        // Clears sentences from previous dialogues
        m_SentencesQueue.Clear();

        // Set the current dialogue
        m_CurrentDialogue = _dialogue;

        // Set the name
        m_NameText.text = _dialogue.m_Name;

        // Queue up the sentences
        foreach (string sentence in _dialogue.m_Sentences)
        {
            m_SentencesQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (m_CurrentDialogue == null)
        {
            Debug.Log("ERROR: NO DIALOGUE INSERTED!");
            EndDialogue();
            return;
        }

        if (m_SentencesQueue.Count <= 0)
        {
            Debug.Log(m_CurrentDialogue.m_Name + " has nothing more to say.");
            EndDialogue();
            return;
        }

        // If TYPING, finish the current line instead
        if (m_IsTyping)
        {
            FinishTyping();
            return;
        }

        m_CurrentSentence = m_SentencesQueue.Dequeue();

        // Stop animating previous text
        StopAllCoroutines();

        // Start new coroutine to type sentence
        StartCoroutine(TypeSentence(m_CurrentSentence, m_CurrentDialogue.m_TextSpeed));
    }

    IEnumerator TypeSentence (string _sentence, float _textSpeed)
    {
        // Enable typing flag
        m_IsTyping = true;

        // Clear dialogue box
        m_DialogueText.text = "";
        foreach (char letter in _sentence.ToCharArray())
        {
            m_DialogueText.text += letter;
            yield return new WaitForSeconds(_textSpeed);
        }

        while (m_DialogueText.text.Length != _sentence.Length)
        {
            Debug.Log("... Typing ...");
        }

        m_IsTyping = false;
    }

    public void FinishTyping()
    {
        StopAllCoroutines();
        m_IsTyping = false;
        m_DialogueText.text = m_CurrentSentence;
    }

    public void EndDialogue()
    {
        Debug.Log("Exiting dialogue...");

        m_Animator.SetBool("IsOpen", false);

        // Disable the Dialogue Space
        m_DialogueSpace.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_SentencesQueue = new Queue<string>();
    }
}
