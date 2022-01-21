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
    public Collider2D topCollider;
    /// <summary>
    /// The bottom collider of the game
    /// </summary>
    public Collider2D bottomCollider;
    /// <summary>
    /// The left collider of the game
    /// </summary>
    public Collider2D leftCollider;
    /// <summary>
    /// The right collider of the game
    /// </summary>
    public Collider2D rightCollider;
    /// <summary>
    /// The Game Space
    /// </summary>
    public Collider2D GameSpace;
    /// <summary>
    /// The very outer bounds of where anything should be
    /// </summary>
    public Collider2D OuterBounds;
    /// <summary>
    /// Size of the game space in the X dimension
    /// </summary>
    public float GameSizeX { get; private set; }
    /// <summary>
    /// Size of the game space in the Y dimension
    /// </summary>
    public float GameSizeY { get; private set; }
    /// <summary>
    /// The x position of the right border
    /// </summary>
    public float RightBorder { get; private set; }
    /// <summary>
    /// The x position of the left border
    /// </summary>
    public float LeftBorder { get; private set; }
    /// <summary>
    /// The y position of the top border
    /// </summary>
    public float TopBorder { get; private set; }
    /// <summary>
    /// The y position of the bottom border
    /// </summary>
    public float BottomBorder { get; private set; }
    #endregion

    #region methods
    public void Awake()
    {
        //Calculate the border coordinates
        RightBorder = rightCollider.bounds.center.x - rightCollider.bounds.extents.x;
        LeftBorder = leftCollider.bounds.center.x + leftCollider.bounds.extents.x;
        TopBorder = topCollider.bounds.center.y - topCollider.bounds.extents.y;
        BottomBorder = bottomCollider.bounds.center.y + bottomCollider.bounds.extents.y;
        //Calculate the size of the game space
        GameSizeX = RightBorder - LeftBorder;
        GameSizeY = TopBorder - BottomBorder;
    }
    #endregion

}
