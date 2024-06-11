using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // --------- Public class variables --------- //
    public float FollowSpeed = 3.0f;
    public float yOffset = 1.0f;
    public float xOffset = 0.0f;
    public Transform target;

    // Variables for clamping camera.
    public Vector2 MaxPosition;
    public Vector2 MinPosition;

    // Variables for offsetting for dialogue scenes
    public float dialogueYOffset = 20.0f;

    // --------- Functions --------- //

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x - xOffset, target.position.y + yOffset, -10.0f);

        // Clamp camera to minimum and maximum for room size (ensures camera stays within room bounds).
      //  newPos.x = Mathf.Clamp(newPos.x, MinPosition.x, MaxPosition.x); // Clamping x position.
      //  newPos.y = Mathf.Clamp(newPos.y, MinPosition.y, MaxPosition.y); // Clamping y position.

        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }

    // Drops the camera down during dialogue
    public void DropForDialogue(bool _doDrop)
    {
        if (_doDrop)
        {
            FollowSpeed *= 2.0f;
            yOffset -= dialogueYOffset;
            MinPosition.y -= dialogueYOffset;
            return;
        }

        yOffset += dialogueYOffset;
        MinPosition.y += dialogueYOffset;
        FollowSpeed /= 2.0f;
    }
}
