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
    /// <summary>
    /// The boundaries of the game.
    /// </summary>
    private GameBoundaries bounds;
    /// <summary>
    /// Whether or not this object has had contact before.
    /// </summary>
    private bool firstContact = true;
    #endregion

    #region properties
    /// <summary>
    /// If set to true, ignores the first collision
    /// </summary>
    public bool IgnoreFirstContact = false;
    #endregion

    #region methods

    public void Start()
    {
        //get the attached 2d collider
        ownCollider = this.GetComponent<Collider2D>();

        //inject game boundaries
        bounds = GameObject.Find("GameView").GetComponent<GameBoundaries>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IgnoreFirstContact && firstContact)
        {
            firstContact = false;
            return;
        }

        //we have to calculate the current size of the collider in the frame of the collision, because it changes with varying orientation of the object
        //Give 10% leeway to own size to avoid triggering a collision on the other side after movement
        var ownSizeX = ownCollider.bounds.extents.x * 1.10f;
        var ownSizeY = ownCollider.bounds.extents.y * 1.10f;

        if (collision.Equals(bounds.topCollider))
        {
            this.transform.SetPositionAndRotation(new Vector3(
                this.transform.position.x,
                bounds.bottomCollider.bounds.center.y + bounds.bottomCollider.bounds.extents.y + 0.001f + ownSizeY,
                this.transform.position.z),
                this.transform.rotation);
        }
        else if (collision.Equals(bounds.bottomCollider))
        {
            this.transform.SetPositionAndRotation(new Vector3(
                this.transform.position.x,
               bounds.topCollider.bounds.center.y - bounds.topCollider.bounds.extents.y - 0.001f - ownSizeY,
               this.transform.position.z),
               this.transform.rotation);
        }
        else if (collision.Equals(bounds.leftCollider))
        {
            this.transform.SetPositionAndRotation(new Vector3(
                bounds.rightCollider.bounds.center.x - bounds.rightCollider.bounds.extents.x - 0.001f - ownSizeX,
                this.transform.position.y,
                this.transform.position.z),
                this.transform.rotation);
        }
        else if (collision.Equals(bounds.rightCollider))
        {
            this.transform.SetPositionAndRotation(new Vector3(
                bounds.leftCollider.bounds.center.x + bounds.leftCollider.bounds.extents.x + 0.001f + ownSizeX,
                this.transform.position.y,
                this.transform.position.z),
                this.transform.rotation);
        }
    }
    #endregion
}
