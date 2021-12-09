using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic of a missile
/// </summary>
public class MissileLogic : MonoBehaviour
{
    #region fields
    /// <summary>
    /// The boundaries of the game
    /// </summary>
    private GameBoundaries bounds;
    #endregion

    #region properties 
    /// <summary>
    /// The travel speed of the missile (Must be set before Start() is called)
    /// </summary>
    public float Speed = 1.0f;
    /// <summary>
    /// The sound that plays when the missile fires
    /// </summary>
    public AudioClip Fire;
    /// <summary>
    /// The Game Object to play a local audio clip.
    /// </summary>
    public GameObject LocalAudioPrefab;
    #endregion

    #region methods
    void Start()
    {
        //Apply movement force in the direction the missile is facing.
        GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.up.x, transform.up.y) * 0.3f * Speed);

        bounds = GameObject.Find("GameView").GetComponent<GameBoundaries>();

        GameObject go = GameObject.Instantiate(LocalAudioPrefab);
        go.transform.position = this.transform.position;
        go.GetComponent<LocalAudioScript>().Clip = Fire;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.Equals(bounds.bottomCollider) || collision.Equals(bounds.topCollider) || collision.Equals(bounds.leftCollider) ||
            collision.Equals(bounds.rightCollider))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If colliding with asteroids, destroy this game object
        if(collision.collider.gameObject.tag.Equals("Asteroid"))
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
}
