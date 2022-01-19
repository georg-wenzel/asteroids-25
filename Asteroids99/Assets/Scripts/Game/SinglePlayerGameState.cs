using UnityEngine;
using Networking;

/// <summary>
/// Script which keeps track of the Game State of the local player using provided observers
/// </summary>
[System.Serializable]
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

    public void IncreasePlayerKills()
    {
        this.PlayerGameState.PlayerKills += 1;
        HUD.UpdatePKs(this.PlayerGameState);
    }

    /// <summary>
    /// Invoked by the spaceship (observable) to update the current HP of the ship
    /// </summary>
    /// <param name="hp">The current HP of the ship</param>
    public void UpdateHP(int hp)
    {
        this.PlayerGameState.HP = hp;
        //If we just went game over, update the game state, and show the game over hud
        if (this.PlayerGameState.HP == 0 && !this.PlayerGameState.GameOver && !HUD.HasWon())
        {
            this.PlayerGameState.GameOver = true;
            HUD.GameOver();
            HUD.UpdateHP(this.PlayerGameState);
            Player.localPlayer.SetGameState(this.PlayerGameState);
        }
        //else, only update if the player is still above 0 HP (if the player has been game over, do not update the local game state) and has not won already
        else if (this.PlayerGameState.HP > 0 && !HUD.HasWon())
        {
            HUD.UpdateHP(this.PlayerGameState);
            Player.localPlayer.SetGameState(this.PlayerGameState);
        }
    }

    /// <summary>
    /// Invoked by AsteroidDeathObservables (asteroids) to notify the GameState, which adds additional score
    /// </summary>
    /// <param name="asteroid">The asteroid which died</param>
    public void NotifyDeath(GameObject asteroid)
    {
        //Only add score / update if player is not game over
        if (this.PlayerGameState.GameOver || HUD.HasWon()) return;

        int score = 100;
        //multiply score by size of asteroid (e.g. 3x scale => 3x score)
        if (asteroid.transform.localScale.magnitude > 1)
            score *= (int)asteroid.transform.localScale.x;
        this.PlayerGameState.Score += score;
        HUD.UpdateScore(this.PlayerGameState);
        Player.localPlayer.SetGameState(this.PlayerGameState);
    }
    #endregion
}