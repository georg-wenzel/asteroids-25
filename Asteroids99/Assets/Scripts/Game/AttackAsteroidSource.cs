using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script is attached to attack asteroids. It carries the source ID of the attack asteroid, and reports back to the server when the asteroid is destroyed.
/// </summary>
public class AttackAsteroidSource : MonoBehaviour
{
    #region properties
    public int SourcePlayerID { get; set; }
    #endregion

    private void OnDestroy()
    {
        Debug.Log("Attack Asteroid hit from player ID " + this.SourcePlayerID);
        //TODO report back to server via command
        //ex. UpdateHitCount(this.SourcePlayerID)
    }
}
