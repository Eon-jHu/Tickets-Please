using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Private serialized class variables.
    [SerializeField] private int MovementSpeed = 5;

    // Can Move Flag
    public bool bCanMove = true;

    // Private class variables.
    private Vector2 Movement;
    private Rigidbody2D RigidBody;
    private Animator PlayerAnim;

    // --------------- Functions --------------- //

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        PlayerAnim = GetComponent<Animator>();
        PlayerAnim.SetBool("IsWalking", false);
    }

    private void OnMovement(InputValue _Value) // Called when character moves.
    {
        Movement = _Value.Get<Vector2>(); // Assign character movement.

        // Setting the x and y values for the animation.
        if (Movement.x != 0 || Movement.y != 0)
        {
            PlayerAnim.SetFloat("x", Movement.x);
            PlayerAnim.SetFloat("y", Movement.y);

            PlayerAnim.SetBool("IsWalking", true); // If player is moving play the walk animation.
        }
        else
        {
            PlayerAnim.SetBool("IsWalking", false);
        }

    }

    private void FixedUpdate()
    {
        if (!bCanMove)
        {
            return;
        }

        // Handle character movement.
        RigidBody.MovePosition(RigidBody.position + Movement * MovementSpeed * Time.fixedDeltaTime);
    }
}
