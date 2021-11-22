using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script which allows objects to disappear from one side of the sreen and appear on the other.
/// </summary>
public class BoundaryTeleport : MonoBehaviour
{
    #region fields
    /// <summary>
    /// The collider of the object
    /// </summary>
    private Collider2D ownCollider;
    #endregion

    #region properties
    /// <summary>
    /// The boundaries of the game.
    /// </summary>
    public GameBoundaries bounds;
    #endregion

    public void Start()
    {
        //get the attached 2d collider
        ownCollider = this.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //we have to calculate the current size of the collider in the frame of the collision, because it changes with varying orientation of the object
        //Give 10% leeway to own size to avoid triggering a collision on the other side after movement
        var ownSizeX = ownCollider.bounds.extents.x * 2 * 1.1f;
        var ownSizeY = ownCollider.bounds.extents.y * 2 * 1.1f;

        if (collision.Equals(bounds.topCollider))
        {
            this.transform.Translate(new Vector3(0, - bounds.GameSizeY + ownSizeY, 0), Space.World);
        }
        else if (collision.Equals(bounds.bottomCollider))
        {
            this.transform.Translate(new Vector3(0, bounds.GameSizeY - ownSizeY, 0), Space.World);
        }
        else if (collision.Equals(bounds.leftCollider))
        {
            this.transform.Translate(new Vector3(bounds.GameSizeX - ownSizeX, 0, 0), Space.World);
        }
        else if (collision.Equals(bounds.rightCollider))
        {
            this.transform.Translate(new Vector3(- bounds.GameSizeX + ownSizeX, 0, 0), Space.World);
        }
    }
}
