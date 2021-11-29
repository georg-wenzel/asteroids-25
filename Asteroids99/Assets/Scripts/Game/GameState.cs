using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour, IHPObserver, IAsteroidDeathObserver
{
    #region properties
    /// <summary>
    /// Inject the player's spaceship here to bootstrap the HP observer.
    /// </summary>
    public GameObject PlayerSpaceship;
    /// <summary>
    /// Whether or not the player is still playing.
    /// </summary>
    public bool GameOver { get; private set; } = false;
    /// <summary>
    /// The current HP of the player's spaceship.
    /// </summary>
    public int HP { get; private set; }
    /// <summary>
    /// The current player's score.
    /// </summary>
    public int Score { get; private set; }
    #endregion

    #region methods
    public void Start()
    {
        //Add self as observer for the spaceship's HP observable
        PlayerSpaceship.GetComponent<SpaceshipHealthManager>().register(this);
    }

    /// <summary>
    /// Invoked by the spaceship (observable) to update the current HP of the ship
    /// </summary>
    /// <param name="hp">The current HP of the ship</param>
    public void UpdateHP(int hp)
    {
        this.HP = hp;
        if(this.HP == 0)
            this.GameOver = true;
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
        this.Score += score;
    }
    #endregion
}
