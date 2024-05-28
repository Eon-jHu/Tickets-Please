using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Private serialized class variables.
    [SerializeField] private int MovementSpeed = 5;

    // Private class variables.
    private Vector2 Movement;
    private Rigidbody2D RigidBody;

    // --------------- Functions --------------- //

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();  
    }

    private void OnMovement(InputValue _Value) // Called when character moves.
    {
        Movement = _Value.Get<Vector2>(); // Assign character movement.
    }

    private void FixedUpdate()
    {
        // Handle character movement.
        RigidBody.MovePosition(RigidBody.position + Movement * MovementSpeed * Time.fixedDeltaTime);
    }
}
