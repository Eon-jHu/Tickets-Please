using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerMovement m_PlayerMovement;

    [SerializeField] public GameObject m_TicketUI;

    // ----------------------- Singleton -----------------------
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<PlayerController>();
        }
        else if (Instance != FindObjectOfType<PlayerController>())
        {
            Destroy(FindObjectOfType<PlayerController>());
        }
    }

    private void Start()
    {
        m_TicketUI.SetActive(false);
    }

    // OnDestroy is called when the object is destroyed
    private void OnDestroy()
    {
        Instance = null;
    }
    // ---------------------------------------------------------
}
