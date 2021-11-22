using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script which controls shooting of the missiles
/// </summary>
public class SpaceshipShooting : MonoBehaviour
{
    #region fields
    /// <summary>
    /// The remaining cooldown until another missile can be fired
    /// </summary>
    private float cooldown = 0.0f;
    #endregion

    #region properties
    /// <summary>
    /// The missile prefab
    /// </summary>
    public GameObject Missile;
    #endregion

    #region methods
    void Update()
    {
        //Fire on spacebar press if 0.2 or more seconds have passed since the last shot
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(cooldown <= 0.0f)
            {
                Fire();
                cooldown = 0.2f;
            }
        }

        if (cooldown > 0.0f)
            cooldown -= Time.deltaTime;
    }

    private void Fire()
    {
        //Instantiate new missile
        var go = Instantiate(Missile, this.transform.position + this.transform.up.normalized * 0.01f, this.transform.rotation);
        //Make sure it uses the same game bounds as the spaceship
        go.GetComponent<MissileMovement>().bounds = this.GetComponent<BoundaryTeleport>().bounds;
    }
    #endregion
}
