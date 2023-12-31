using Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking;

/// <summary>
/// Script is attached to attack asteroids. It carries the source ID of the attack asteroid, and reports back to the server when the asteroid is destroyed.
/// </summary>
public class AttackAsteroidSource : MonoBehaviour
{
    #region properties
    public int SourcePlayerID { get; set; }
    #endregion

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //when this asteroid hits the local spaceship
        if (collision.gameObject.tag.Equals("Spaceship"))
        {
            Debug.Log("Attack Asteroid hit from player ID " + this.SourcePlayerID);
            //TODO report back to server via command
            Player.localPlayer.AttackAsteroidKilledPlayer(this.SourcePlayerID);
        }
    }
}
