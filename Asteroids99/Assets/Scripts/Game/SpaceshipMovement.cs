using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script for controlling Spaceship Movement
/// </summary>
public class SpaceshipMovement : MonoBehaviour
{
    #region fields
    /// <summary>
    /// The current velocity of the spaceship (in screen dimensions)
    /// </summary>
    private Vector2 velocity;
    private PlayerInput input;
    #endregion

    #region properties
    /// <summary>
    /// A modifier for how fast the spaceship accelerates.
    /// </summary>
    public float translationSpeed = 1.0f;
    /// <summary>
    /// A modifier for how fast the spaceship turns.
    /// </summary>
    public float rotationSpeed = 1.0f;
    /// <summary>
    /// A modifier for how fast built velocity decays.
    /// </summary>
    public float speedDecay = 1.0f;
    #endregion

    #region methods
    void Start()
    {
        //set the initial velocity to 0
        velocity = new Vector2(0,0);
        input = this.GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        //each frame, the velocity decays by a static amount, and if the decay changes the sign of the dimension, that dimension's velocity changes to 0.
        float decreaseX = -0.01f * Mathf.Sign(velocity.x);
        float decreaseY = -0.01f * Mathf.Sign(velocity.y);
        Vector2 new_velocity = this.velocity + new Vector2(decreaseX, decreaseY);
        velocity = new Vector2((new_velocity.x * velocity.x > 0) ? new_velocity.x : 0,
                                (new_velocity.y * velocity.y > 0) ? new_velocity.y : 0);


        Vector2 inputVector = input.actions["Move"].ReadValue<Vector2>();
        Vector2 movementVector = new Vector2(transform.up.x * inputVector.y, transform.up.y * inputVector.y);
        float rotationDegrees = -inputVector.x * 8f;

        //update the velocity, clamping it to a maximum magnitude (max speed is the same in multiple dimensions as in one)
        this.velocity = Vector2.ClampMagnitude(this.velocity + movementVector * translationSpeed * 0.025f, 0.75f);
        //apply this frame's rotation and translation.
        transform.Rotate(new Vector3(0, 0, rotationDegrees * rotationSpeed));
        transform.Translate(velocity, Space.World);
    }
    #endregion
}
