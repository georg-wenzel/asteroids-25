using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for definining the bounding colliders of the Game
/// </summary>
public class GameBoundaries : MonoBehaviour
{
    #region properties
    /// <summary>
    /// The top collider of the game
    /// </summary>
    public BoxCollider2D topCollider;
    /// <summary>
    /// The bottom collider of the game
    /// </summary>
    public BoxCollider2D bottomCollider;
    /// <summary>
    /// The left collider of the game
    /// </summary>
    public BoxCollider2D leftCollider;
    /// <summary>
    /// The right collider of the game
    /// </summary>
    public BoxCollider2D rightCollider;
    /// <summary>
    /// Size of the game space in the X dimension
    /// </summary>
    public float GameSizeX { get; private set; }
    /// <summary>
    /// Size of the game space in the Y dimension
    /// </summary>
    public float GameSizeY { get; private set; }
    #endregion

    #region methods
    public void Start()
    {
        //Calculate the size of the game space via the bounding boxes
        GameSizeX = (rightCollider.bounds.center - rightCollider.bounds.extents).x -
            (leftCollider.bounds.center + leftCollider.bounds.extents).x;
        GameSizeY = (topCollider.bounds.center - topCollider.bounds.extents).y -
            (bottomCollider.bounds.center + bottomCollider.bounds.extents).y;
    }
    #endregion

}
