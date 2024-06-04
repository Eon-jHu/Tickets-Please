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
    GameState m_State;
    [SerializeField] public PlayerController m_PlayerController;
    [SerializeField] Camera m_Camera;
    // [SerializeField] CameraShake m_CameraShake;
    [SerializeField] public AudioManager m_AudioManager;
    [SerializeField] InteractableObject[] m_Entities;

    public GameState State { get { return m_State; } }
    public void SetState(GameState _state) { m_State = _state; }

    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameController>();
        }
        else if (instance != FindObjectOfType<GameController>())
        {
            Destroy(FindObjectOfType<GameController>());
        }
    }

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
