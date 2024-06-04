using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPCText : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public GameObject instructionPanel;
    public string[] dialogue;
    public int index;
    public GameObject continueButton;
    public float wordSpeed;
    public bool playerIsClose;
    private bool endOfText = false;
    public Image panelImage;

    // Add a variable to track whether the NPC is in an encounter
    private bool inEncounter = false;
    private bool isTyping = false;

    private string[] initialDialogue; // Store the initial dialogue for resetting

    void Start()
    {
        // Make a copy of the initial dialogue for this NPC
        initialDialogue = new string[dialogue.Length];
        dialogue.CopyTo(initialDialogue, 0);
    }

    void Update()
    {
        if (playerIsClose)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (dialoguePanel.activeInHierarchy)
                {
                    NextLine();
                }
                else
                {
                    dialoguePanel.SetActive(true);

                    // Check if this is the start of a new encounter
                    if (!inEncounter)
                    {
                        StartEncounter();
                    }

                    if (!isTyping)
                    {
                        StartCoroutine(Typing());
                    }
                }
            }

            if (dialogueText.text == dialogue[index] && index < (dialogue.Length))
            {
                continueButton.SetActive(true);
            }
            else if (index == dialogue.Length - 1)
            {
                endOfText = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dialoguePanel.activeInHierarchy)
                {
                    NextLine();
                }
            }
        }

        if (dialoguePanel.activeInHierarchy && instructionPanel.activeInHierarchy)
        {
            instructionPanel.SetActive(false);
        }
    }

    // Add a function to start a new encounter
    private void StartEncounter()
    {
        if (GameController.instance.State != GameState.FreeRoam)
        {
            return;
        }

        inEncounter = true;
        GameController.instance.m_PlayerController.interactAudioEffect.Play();

        // Reset the dialogue array to its initial state
        dialogue = new string[initialDialogue.Length];
        initialDialogue.CopyTo(dialogue, 0);

        index = 0;
        dialogueText.text = "";
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);

        // Mark the encounter as ended
        inEncounter = false;
    }

    IEnumerator Typing()
    {
        if (!isTyping)
        {
            isTyping = true;

            foreach (char letter in dialogue[index].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(wordSpeed);
            }

            isTyping = false;
        }
    }

    public void NextLine()
    {
        if (GameController.instance.State != GameState.FreeRoam)
        {
            return;
        }

        continueButton.SetActive(false);

        if (isTyping)
        {
            StopAllCoroutines();
            isTyping = false;
            dialogueText.text = dialogue[index];
            return;
        }

        if (index < (dialogue.Length - 1))
        {
            if (!isTyping)
            {
                index++;
                dialogueText.text = "";
                StartCoroutine(Typing());
            }
        }
        else
        {
            endOfText = true;
            ZeroText();

            if (endOfText)
            {
                Debug.Log("End of the text has been reached");

                // Enter combat if engageable and disable this object
                if (gameObject.GetComponent<Engageable>() != null)
                {
                    GameController.instance.m_PlayerController.TriggerOnEncountered(gameObject);
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameController.instance.State != GameState.Battle)
        {
            instructionPanel.SetActive(true);
            Debug.Log("Entered proximity");
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            instructionPanel.SetActive(false);
            playerIsClose = false;
            ZeroText();
        }
    }
}
