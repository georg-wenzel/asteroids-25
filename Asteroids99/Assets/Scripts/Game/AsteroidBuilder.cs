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
    private Vector2 initialSpawn;
    private Vector2 targetDirection;
    #endregion

    #region properties
    /// <summary>
    /// A base Asteroid
    /// </summary>
    public GameObject AsteroidPrefab;

    /// <summary>
    /// The Game Boundaries
    /// </summary>
    public GameBoundaries GameView;
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
        initialSpawn = new Vector2(0,0);

        //==================== SPAWN THE METEOR OUTSIDE THE ARENA, TOWARDS A TARGET POINT INSIDE OF IT ====================
        //this implementation >should< be relatively robust to resizing of the GameView.

        //Define a random target point in the game field.
        float boundLeft = GameView.leftCollider.bounds.center.x + GameView.leftCollider.bounds.extents.x;
        float boundRight = GameView.rightCollider.bounds.center.x - GameView.rightCollider.bounds.extents.x;
        float boundTop = GameView.topCollider.bounds.center.y - GameView.topCollider.bounds.extents.y;
        float boundBottom = GameView.leftCollider.bounds.center.y + GameView.bottomCollider.bounds.extents.y;
        Vector2 targetPoint = new Vector2(Random.Range(boundLeft, boundRight), Random.Range(boundBottom, boundTop));

        //Spawn the meteor outside the arena at random
        int randx = Random.Range(0, 3);
        int randy = Random.Range(0, 3);
        //prevent 0/0 spawn
        if (randx + randy == 0) randx = Random.Range(1, 3);
        
        if(randx == 1)
            initialSpawn.x = boundLeft - GameView.GameSizeX / 2;
        else if(randx == 2)
            initialSpawn.x = boundRight + GameView.GameSizeX / 2;
        if(randy == 1)
            initialSpawn.y = boundBottom - GameView.GameSizeY / 2;
        else if(randy == 2)
            initialSpawn.y = boundTop + GameView.GameSizeY / 2;

        //point the velocity vector towards the target point (inside the arena)
        targetDirection = (targetPoint - initialSpawn).normalized;
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
        props.InitialMovementDirection = this.targetDirection;
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
