using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the default behaviour for an asteroid
/// </summary>
public class BaseAsteroidBehaviour : MonoBehaviour
{
    #region fields
    /// The corresponding asteroid properties of the GameObject
    /// </summary>
    private AsteroidProperties properties;
    /// <summary>
    /// The rigid body of the object
    /// </summary>
    private Rigidbody2D rigidbody2d;
    /// <summary>
    /// The desired magnitude of velocity
    /// </summary>
    private float velocityMagnitude;
    /// <summary>
    /// internal health of the asteroid
    /// </summary>
    private int health;
    /// <summary>
    /// The bounds of the playing field
    /// </summary>
    private GameBoundaries bounds;
    #endregion

    #region methods
    public void Start()
    {
        //get components of this game object
        rigidbody2d = GetComponent<Rigidbody2D>();
        properties = GetComponent<AsteroidProperties>();

        Vector2 direction;
        //if no direction is given in the properties
        if (properties.InitialMovementDirection.magnitude > 0)
            //use this vector
            direction = properties.InitialMovementDirection.normalized;
        else
            //define a random movement vector this asteroid travels in
            direction = new Vector2(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f)).normalized;

        //add the initial force and store the desired magnitude of velocity
        rigidbody2d.AddForce(direction * properties.Speed, ForceMode2D.Impulse);
        velocityMagnitude = rigidbody2d.velocity.magnitude;

        //add a small random spin to the asteroid
        rigidbody2d.AddTorque(Random.Range(0.0f,5.0f));

        //get initial health from properties
        this.health = properties.InitialHealth;

        //inject game bounds
        bounds = GameObject.Find("GameView").GetComponent<GameBoundaries>();
    }

    void Update()
    {
        //if the magnitude of velocity changes from the desired (after a collision)
        var magnitudeDiff = Mathf.Abs(velocityMagnitude - rigidbody2d.velocity.magnitude);
        if (magnitudeDiff > 0.001f)
        {
            //make sure the velocity stays constant
            rigidbody2d.velocity = rigidbody2d.velocity.normalized * velocityMagnitude;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //On collision with a missile
        if (collision.collider.gameObject.tag.Equals("Missile"))
        {
            health--;
            if (health == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //General sanity check - if we hit the outer borders (asteroid escaped)
        if (collision.Equals(bounds.OuterBounds))
        {
            //Define a random target point inside the center 80% of the game field.
            Vector2 targetPoint = new Vector2(
                Random.Range(bounds.LeftBorder * 0.8f, bounds.RightBorder * 0.8f),
                Random.Range(bounds.BottomBorder * 0.8f, bounds.TopBorder * 0.8f));

            //Change velocity towards this point
            rigidbody2d.velocity = (targetPoint - rigidbody2d.position).normalized * velocityMagnitude;
        }
    }
    #endregion
}
