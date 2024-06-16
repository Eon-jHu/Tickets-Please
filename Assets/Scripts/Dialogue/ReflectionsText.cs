using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReflectionsText : MonoBehaviour
{
    // Sentences dependent on the number of tickets player has.
    private string[] ReflectionSentences = new string[7]
    {
        "That could have gone better...",
        "Argh. Why did I say that?",
        "I guess... That wasn't too bad.",
        "I hope they don't think I was weird...",
        "Yeah. I said the best thing I could.",
        "Hmph. That was funny!",
        "They definitely thought I was a good ticket collector!"
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

        // Wait for 7 seconds before hiding reflection dialogr.
        yield return new WaitForSeconds(7.0f);

        // Hide reflections box again.
        ReflectionsBox.SetActive(false);
    }
}
