using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines basic properties of an asteroid
/// </summary>
public class AsteroidProperties : MonoBehaviour
{
    #region properties
    /// <summary>
    /// The scale of the asteroid (must be set before Start() is called)
    /// </summary>
    public float Scale = 1.0f;
    /// <summary>
    /// The Speed of the asteroid (must be set before Start() is called)
    /// </summary>
    public float Speed = 1.0f;
    /// <summary>
    /// The initial health of the asteroid (must be set before Start() is called)
    /// </summary>
    public int InitialHealth = 1;
    /// <summary>
    /// The initial direction this Asteroid travels in. If 0/0, it is randomly chosen.
    /// </summary>
    public Vector2 InitialMovementDirection;
    #endregion

    #region methods
    void Start()
    {
        //set scale of the asteroid according to properties
        transform.localScale = new Vector3(Scale, Scale, 1);
    }
    #endregion
}
