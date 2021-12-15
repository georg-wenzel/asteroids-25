using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple model class which defines basic information about an Asteroids player.
/// </summary>
[System.Serializable]
public class GameState
{

    #region properties
    /// <summary>
    /// Whether or not the player is still playing.
    /// </summary>
    public bool GameOver;
    /// <summary>
    /// The current HP of the player's spaceship.
    /// </summary>
    public int HP;
    /// <summary>
    /// The current player's score.
    /// </summary>
    public int Score;
    
    #endregion

    #region ctor
    public GameState(bool gameOver, int hp, int score)
    {
        this.GameOver = gameOver;
        this.HP = hp;
        this.Score = score;
    }

    public GameState()
    {

    }
    #endregion

}
