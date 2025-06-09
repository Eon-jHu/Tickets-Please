using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishCondition : MonoBehaviour
{
    [SerializeField] private GameObject TicketPromptText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player is colliding with the NPC.
        if (collision.CompareTag("Player"))
        {
            // If player has collected enough tickets.
            if (TicketScore.TicketNumber >= 9)
            {
                // Player can exit, load final scene.
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

                // Reset ticket score value so game can be replayed.
                TicketScore.TicketNumber = -1;
            }
            else
            {
                // Display to player not enough tickets.
                Debug.Log("Not enough tickets collected");
            }
        }
    }

    protected void Update()
    {

        if (TicketScore.TicketNumber < 9)
        {
            TicketPromptText.SetActive(true);
        }
        else
        {
            TicketPromptText.SetActive(false);
        }
    }
}
