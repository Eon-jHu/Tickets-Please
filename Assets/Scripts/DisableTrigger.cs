using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTrigger : MonoBehaviour
{
    void OnDisable()
    {
        PlayerController.Instance.m_TicketUI.SetActive(true);
    }
}
