using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReflectionsText : MonoBehaviour
{
    // Sentences dependent on the number of tickets player has.
    private string[] ReflectionSentences = new string[9]
    {
        "Aaaah... Why did I say that...",
        "I hope they don't think I was weird...",
        "I think... I could've been a bit nicer...",
        "That was hard to understand... But I think that was good...",
        "That wasn't too bad...",
        "Some people are just like me too...",
        "It's getting easier to talk...",
        "Talking is important... For everyone...",
        "... Did I make a difference...?"
    };

    [SerializeField] private GameObject ReflectionsBox;
    [SerializeField] private Text ReflectionText;

    private int PreviousTicketNumber = -1;

    void Start()
    {
        ReflectionsBox.SetActive(false); 
    }

    void Update()
    {
        // Check that tickets have been collected, and there has been change since last ticket was collected.
        if (TicketScore.TicketNumber > 0 && TicketScore.TicketNumber != PreviousTicketNumber)
        {
            // Update the previous ticket number.
            PreviousTicketNumber = TicketScore.TicketNumber;

            // Update text and start timer for reflection text.
            StartCoroutine(DisplayReflection(TicketScore.TicketNumber - 1));
        }
    }

    IEnumerator DisplayReflection(int ReflectionSentencesIndex)
    {
        ReflectionsBox.SetActive(true);

        // Display reflection sentence based on tickets collecteted.
        ReflectionText.text = ReflectionSentences[ReflectionSentencesIndex];

        // Wait for X seconds before hiding reflection dialogue.
        yield return new WaitForSeconds(3.0f);

        // Hide reflections box again.
        ReflectionsBox.SetActive(false);
    }
}
