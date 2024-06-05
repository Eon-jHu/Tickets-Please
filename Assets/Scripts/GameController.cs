using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ticketing,
    Dialogue,
    Chase
}


public class GameController : MonoBehaviour
{
    private GameState m_State;

    [SerializeField]
    public PlayerController m_PlayerController;

    [SerializeField]
    public Camera m_Camera;

    private InteractableObject[] m_Entities;
    
    // [SerializeField] CameraShake m_CameraShake;

    public GameState State { get { return m_State; } }
    public void SetState(GameState _state) { m_State = _state; }

    // ----------------------- Singleton -----------------------
    public static GameController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<GameController>();
        }
        else if (Instance != FindObjectOfType<GameController>())
        {
            Destroy(FindObjectOfType<GameController>());
        }
    }

    // OnDestroy is called when the object is destroyed
    private void OnDestroy()
    {
        Instance = null;
    }
    // ---------------------------------------------------------

    // Subscribe to the created event
    private void Start()
    {
        // Observer/Subscriber Pattern
        //m_PlayerController.OnEncountered += StartBattle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case GameState.Ticketing:
            {

                    break;
            }
            case GameState.Dialogue:
            {
                    
                    break;
            }
            case GameState.Chase:
            {
                    
                    break;
            }
            default:
            {
                    
                    break;
            }
        }
    }
}
