using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAsteroidBehaviour : MonoBehaviour
{
    #region fields
    /// <summary>
    /// The rigid body of the object
    /// </summary>
    private Rigidbody2D rigidbody2d;
    /// <summary>
    /// The player's spaceship
    /// </summary>
    private GameObject spaceship;
    #endregion

    #region methods
    void Start()
    {
        spaceship = GameObject.Find("Spaceship");
        rigidbody2d = GetComponent<Rigidbody2D>();
        StartCoroutine(NudgeTowardsSpaceship());
    }

    /// <summary>
    /// Coroutine which changes the velocity vector of the asteroid every 0.1 seconds towards the active position of the spaceship
    /// </summary>
    IEnumerator NudgeTowardsSpaceship()
    {
        //Only start changing after 1 seconds (roughly when the asteroid becomes visible, and when the base behaviour has 
        //initialized all necessary logic
        yield return new WaitForSeconds(1f);

        for (; ; )
        {
            Vector2 asteroidPos = this.transform.position;
            Vector2 spaceshipPos = spaceship.transform.position;

            Vector2 currentMovement = rigidbody2d.velocity;
            Vector2 desiredMovement = spaceshipPos - asteroidPos;

            Vector2 newMovement = 0.75f * currentMovement.normalized + 0.25f * desiredMovement.normalized;

            rigidbody2d.velocity = newMovement * currentMovement.magnitude;

            yield return new WaitForSeconds(.1f);
        }
    }
    #endregion
}
