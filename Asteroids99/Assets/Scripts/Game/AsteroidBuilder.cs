using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    private float initialSpeed;
    private Color color;
    private bool isFriendly;
    private Vector2 initialSpawn;
    private Vector2 targetDirection;
    private bool addAttackedSound;
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

    /// <summary>
    /// The sound an attack asteroid plays when it enters the enemy field
    /// </summary>
    public AudioClip AttackAsteroidSound;

    public AudioMixerGroup mixerGroup;
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
        isFriendly = false;
        addAttackedSound = false;
        color = new Color(1, 1, 1, 1);

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
    /// <returns>The instantiated game object</returns>
    /// </summary>
    public GameObject Spawn()
    {
        GameObject go = Instantiate(AsteroidPrefab, initialSpawn, new Quaternion());
        var props = go.GetComponent<AsteroidProperties>();
        props.InitialHealth = this.initialHealth;
        props.Scale = this.initialScale;
        props.Speed = this.initialSpeed;
        props.InitialMovementDirection = this.targetDirection;

        foreach(string scriptName in scriptsToAttach)
        {
            go.AddComponent(System.Type.GetType(scriptName));
        }

        if(isFriendly)
        {
            //Friendly asteroid collision layer
            go.layer = 10;
        }
        if(addAttackedSound)
        {
            AudioSource sound = go.AddComponent<AudioSource>();
            sound.clip = AttackAsteroidSound;
            sound.outputAudioMixerGroup = mixerGroup;
            sound.Play();
        }

        props.GetComponent<SpriteRenderer>().color = color;
        return go;
    }

    /// <summary>
    /// Build a large Asteroid
    /// </summary>
    public void BuildLarge()
    {
        initialScale = 3;
        initialHealth = 3;
    }

    /// <summary>
    /// Build an attack asteroid
    /// </summary>
    public void BuildAttack()
    {
        color = new Color(1, 0.3f, 0.3f, 1);
        initialSpeed *= 2f;
        scriptsToAttach.Add("AttackAsteroidBehaviour");
        scriptsToAttach.Add("AttackAsteroidSource");
        addAttackedSound = true;
    }

    /// <summary>
    /// Build an asteroid which can move from one edge of the screen to another
    /// </summary>
    public void BuildPersistent()
    {
        scriptsToAttach.Add("BoundaryTeleport");
    }

    /// <summary>
    /// Build a friendly asteroid
    /// </summary>
    public void BuildFriendly()
    {
        isFriendly = true;
        color = new Color(0.3f, 1f, 0.3f, 1);
    }

    /// <summary>
    /// Build an asteroid with a specific target velocity direction (relative to its spawn)
    /// <param name="direction">The target direction of the asteroid</param>
    public void BuildTargeted(Vector2 direction)
    {
        this.targetDirection = direction;
    }

    /// <summary>
    /// Build an asteroid which spawns at the specified point
    /// </summary>
    /// <param name="spawn">The specified point for the asteroid to spawn</param>
    public void BuildSpecificSpawn(Vector2 spawn)
    {
        this.initialSpawn = spawn;
    }
    #endregion
}
