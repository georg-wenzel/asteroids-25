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
    #endregion

    #region properties
    /// <summary>
    /// Defines the two methods for boundary detection: OnBorderContact (when the object collider touches the game boundary) or OnBecomesInvisible (once the sprite becomes completely invisible)
    /// </summary>
    public enum BoundaryDetectionMethod
    {
        OnBorderContact, OnLeavesGameSpace
    }
    public BoundaryDetectionMethod DetectionType;
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
        //handler for boundary detection method: on border contact
        if (DetectionType != BoundaryDetectionMethod.OnBorderContact) return;

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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.Equals(bounds.GameSpace) && DetectionType == BoundaryDetectionMethod.OnLeavesGameSpace)
        {
            if (transform.position.x < bounds.LeftBorder)
                transform.SetPositionAndRotation(new Vector3(
                    bounds.RightBorder + ownCollider.bounds.extents.x,
                    transform.position.y,
                    transform.position.z),
                    transform.rotation);
            else if (transform.position.x > bounds.RightBorder)
                transform.SetPositionAndRotation(new Vector3(
                    bounds.LeftBorder - ownCollider.bounds.extents.x,
                    transform.position.y,
                    transform.position.z),
                    transform.rotation);
            if (transform.position.y > bounds.TopBorder)
                transform.SetPositionAndRotation(new Vector3(
                    transform.position.x,
                    bounds.BottomBorder - ownCollider.bounds.extents.y,
                    transform.position.z),
                    transform.rotation);
            else if (transform.position.y < bounds.BottomBorder)
                transform.SetPositionAndRotation(new Vector3(
                    transform.position.x,
                    bounds.TopBorder + ownCollider.bounds.extents.y,
                    transform.position.z),
                    transform.rotation);
        }
    }
    #endregion
}
