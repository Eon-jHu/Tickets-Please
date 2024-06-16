using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public PlayerMovement m_PlayerMovement;

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


    // OnDestroy is called when the object is destroyed
    private void OnDestroy()
    {
        Instance = null;
    }
    // ---------------------------------------------------------
}
