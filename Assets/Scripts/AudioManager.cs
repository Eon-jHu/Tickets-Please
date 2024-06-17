using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource HeartbeatSound;
    public AudioSource EarRinging;
    public AudioSource DoorOpening;

    // ----------------------- Singleton -----------------------
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<AudioManager>();
        }
        else if (Instance != FindObjectOfType<AudioManager>())
        {
            Destroy(FindObjectOfType<AudioManager>());
        }
    }


    // OnDestroy is called when the object is destroyed
    private void OnDestroy()
    {
        Instance = null;
    }
    // ---------------------------------------------------------
}
