using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement of a missile
/// </summary>
public class MissileMovement : MonoBehaviour
{
    #region properties 
    /// <summary>
    /// The travel speed of the missile (Must be set before Start() is called)
    /// </summary>
    public float Speed = 1.0f;
    /// <summary>
    /// The boundaries of the game
    /// </summary>
    public GameBoundaries bounds;
    #endregion

    #region methods
    void Start()
    {
        //Apply movement force in the direction the missile is facing.
        GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.up.x, transform.up.y) * 300 * Speed);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.Equals(bounds.bottomCollider) || collision.Equals(bounds.topCollider) || collision.Equals(bounds.leftCollider) ||
            collision.Equals(bounds.rightCollider))
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}
