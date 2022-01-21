using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loops the sprite this is attached to
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    #region fields
    private float objWidth;
    private float objHeight;
    private int objectsX;
    private int objectsY;
    #endregion

    public GameBoundaries bounds;

    void Start()
    {
        objWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        objHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        objectsX = (int)Mathf.Ceil(bounds.GameSizeX * 2 / objWidth);
        objectsY = (int)Mathf.Ceil(bounds.GameSizeY * 2 / objHeight);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(0.01f, 0.01f, 0);

        if (this.transform.position.x - this.objWidth * 1.5f > bounds.RightBorder)
        {
            this.transform.position = new Vector3(this.transform.position.x - objectsX * objWidth,
                this.transform.position.y, this.transform.position.z);
        }
        else if(this.transform.position.x + this.objWidth * 1.5f < bounds.LeftBorder)
        {
            this.transform.position = new Vector3(this.transform.position.x + objectsX * objWidth,
                this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.y - this.objHeight * 1.5f > bounds.TopBorder)
        {
            this.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y - objectsY * objHeight, this.transform.position.z);
        }
        else if (this.transform.position.y + this.objHeight * 1.5f < bounds.BottomBorder)
        {
            this.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y + objectsY * objHeight, this.transform.position.z);
        }
    }
}
