using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script which controls shooting of the missiles
/// </summary>
public class SpaceshipShooting : MonoBehaviour
{
    #region fields
    /// The player input
    /// </summary>
    private PlayerInput input;
    //Coroutine when the space button is held
    private Coroutine holdFire;
    #endregion

    #region properties
    /// <summary>
    /// The missile prefab
    /// </summary>
    public GameObject Missile;
    #endregion

    #region methods
    private void Start()
    {
        input = this.GetComponent<PlayerInput>();
        input.actions["Fire"].started += SpaceshipShooting_started;
        input.actions["fire"].canceled += SpaceshipShooting_canceled;
    }

    private void SpaceshipShooting_canceled(InputAction.CallbackContext obj)
    {
        StopCoroutine(holdFire);
    }

    private void SpaceshipShooting_started(InputAction.CallbackContext obj)
    {
        Fire();
        holdFire = StartCoroutine(HoldFire());
    }

    private IEnumerator HoldFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            Fire();
        }
            
    }

    private void Fire()
    {
        //Instantiate new missile
        var go = Instantiate(Missile, this.transform.position + this.transform.up.normalized * 0.01f, this.transform.rotation);
    }
    #endregion
}
