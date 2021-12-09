using UnityEngine;

/// <summary>
/// Script which keeps track of the Game State of the local player using provided observers
/// </summary>
public class SinglePlayerGameState : MonoBehaviour, IHPObserver, IAsteroidDeathObserver
{
    #region properties
    /// <summary>
    /// Inject the player's spaceship here to bootstrap the HP observer.
    /// </summary>
    public GameObject PlayerSpaceship;
    /// <summary>
    /// The Game state of the active player
    /// </summary>
    public GameState PlayerGameState { get; private set; }
    /// <summary>
    /// The player's HUD
    /// </summary>
    public GameHUD HUD;
    #endregion

    #region methods
    public void Start()
    {
        //Add self as observer for the spaceship's HP observable
        PlayerSpaceship.GetComponent<SpaceshipHealthManager>().register(this);
        //Initialize a new player state with default values
        PlayerGameState = new GameState(false, 3, 0);
    }

    /// <summary>
    /// Invoked by the spaceship (observable) to update the current HP of the ship
    /// </summary>
    /// <param name="hp">The current HP of the ship</param>
    public void UpdateHP(int hp)
    {
        this.PlayerGameState.HP = hp;
        if (this.PlayerGameState.HP == 0)
        {
            this.PlayerGameState.GameOver = true;
            HUD.GameOver();
        }
        HUD.UpdateHP(this.PlayerGameState);
    }

    /// <summary>
    /// Invoked by AsteroidDeathObservables (asteroids) to notify the GameState, which adds additional score
    /// </summary>
    /// <param name="asteroid">The asteroid which died</param>
    public void NotifyDeath(GameObject asteroid)
    {
        int score = 100;
        //multiply score by size of asteroid (e.g. 3x scale => 3x score)
        if (asteroid.transform.localScale.magnitude > 1)
            score *= (int)asteroid.transform.localScale.x;
        this.PlayerGameState.Score += score;
        HUD.UpdateScore(this.PlayerGameState);
    }
    #endregion
}