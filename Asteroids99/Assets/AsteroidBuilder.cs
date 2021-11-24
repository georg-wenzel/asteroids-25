using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script which offers a public interface for building Asteroids in a builder-pattern like design.
/// </summary>
public class AsteroidBuilder : MonoBehaviour
{
    #region fields
    /* Since we cannot instantiate the GameObject without creating it in real space,
     * we instead hold all the desired properties and create it only when we mean to build it.
     */
    private List<string> scriptsToAttach;
    private int initialHealth;
    private int initialScale;
    private int initialSpeed;
    private Vector3 initialSpawn;
    #endregion

    #region properties
    /// <summary>
    /// A base Asteroid
    /// </summary>
    public GameObject AsteroidPrefab;
    #endregion

    #region methods
    void Start()
    {
        Reset();
    }

    /// <summary>
    /// Reset the current Asteroid settings to default
    /// </summary>
    public void Reset()
    {
        scriptsToAttach = new List<string>();
        initialHealth = 1;
        initialScale = 1;
        initialSpeed = 1;
        initialSpawn = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Build and spawn the created Asteroid
    /// </summary>
    /// <param name="startPosition">The starting position of the Asteroid</param>
    public void Spawn()
    {
        GameObject go = Instantiate(AsteroidPrefab, initialSpawn, new Quaternion());
        var props = go.GetComponent<AsteroidProperties>();
        props.InitialHealth = this.initialHealth;
        props.Scale = this.initialScale;
        props.Speed = this.initialSpeed;
    }

    /// <summary>
    /// Build a large Asteroid
    /// </summary>
    public void BuildLarge()
    {
        initialScale = 3;
        initialHealth = 3;
    }
    #endregion
}
