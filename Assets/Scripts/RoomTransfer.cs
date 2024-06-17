using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransfer : MonoBehaviour
{
    // ------------- Public class variables ------------- //

    public Vector2 CameraChangeMin;
    public Vector2 CameraChangeMax;
    public Vector3 PlayerChange;

    // ------------- Private class variables ------------- //

    private CameraFollow Cam;

    // ------------- Functions ------------- //
    private void Start()
    {
        // Reference to camera from camera follow class.
        Cam = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D _Collision)
    {
        // Checking for collision with room switching bounds & player.
        if (_Collision.CompareTag("Player"))
        {
            // Create the new bounds of the room.
            Cam.MinPosition = CameraChangeMin;
            Cam.MaxPosition = CameraChangeMax;

            // Moving the player to adjust for new room.
            _Collision.transform.position += PlayerChange;

            // Door Opening Sound
            AudioManager.Instance.DoorOpening.Play();
        }
    }
}
