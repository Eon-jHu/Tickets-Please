using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
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

    // --------- Functions --------- //

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x - xOffset, target.position.y + yOffset, -10.0f);

        // Clamp camera to minimum and maximum for room size (ensures camera stays within room bounds).
        newPos.x = Mathf.Clamp(newPos.x, MinPosition.x, MaxPosition.x); // Clamping x position.
        newPos.y = Mathf.Clamp(newPos.y, MinPosition.y, MaxPosition.y); // Clamping y position.

        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);

        Debug.Log("Follow Cam Position: " + transform.position);
    }
}
