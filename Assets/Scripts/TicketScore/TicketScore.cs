using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicketScore : MonoBehaviour
{
    public static int TicketNumber = 0;
    Text Tickets;

    private void Start()
    {
        Tickets = GetComponent<Text>();
    }

    private void Update()
    {
        //CheckActiveState();
        Tickets.text = "" + TicketNumber;
    }

    private void CheckActiveState()
    {
        if (TicketNumber < 0)
        {
            Debug.Log("Text is hidden");
            Tickets.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Text is active");

            Tickets.gameObject.SetActive(true);
        }
    }
}

