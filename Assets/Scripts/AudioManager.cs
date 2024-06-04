using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;
    [SerializeField] AudioClip[] m_WorldMusic;
    [SerializeField] AudioClip[] m_WorldFlippedMusic;
    [SerializeField] AudioClip[] m_BattleMusic;
    [SerializeField] AudioClip[] m_FinalMusic;
    [SerializeField] AudioClip[] m_FinalRoamMusic;

    public bool dontChangeMusic = false;

    public void SetMusic(GameState _gameState)
    {
        if (m_AudioSource == null)
        {
            return;
        }

        switch (_gameState)
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

        }
    }
}
