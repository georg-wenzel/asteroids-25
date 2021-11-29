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

        //==================== SPAWN THE METEOR OUTSIDE THE ARENA, TOWARDS A TARGET POINT INSIDE OF IT ====================
        //this implementation >should< be relatively robust to resizing of the GameView.

        //set the initial spawn point to 0/0
        initialSpawn = new Vector2(0,0);

        //Define a random target point inside the center 80% of the game field.
        Vector2 targetPoint = new Vector2(
            Random.Range(GameView.LeftBorder * 0.8f, GameView.RightBorder * 0.8f),
            Random.Range(GameView.BottomBorder * 0.8f, GameView.TopBorder * 0.8f));

        //Spawn the meteor outside the arena at random
        int randx = Random.Range(0, 3);
        int randy = Random.Range(0, 3);
        //prevent 0/0 spawn (would be center of the field
        if (randx + randy == 0) randx = Random.Range(1, 3);

        //pick a random corner based on above random variables to spawn the asteroid
        if (randx == 1)
            initialSpawn.x = GameView.LeftBorder - 2 * GameView.leftCollider.bounds.size.x;
        else if(randx == 2)
            initialSpawn.x = GameView.RightBorder + 2 * GameView.rightCollider.bounds.size.x;
        if (randy == 1)
            initialSpawn.y = GameView.BottomBorder - 2 * GameView.bottomCollider.bounds.size.y;
        else if (randy == 2)
            initialSpawn.y = GameView.TopBorder + 2 * GameView.topCollider.bounds.size.y;

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
