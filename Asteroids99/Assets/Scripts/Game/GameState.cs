/// <summary>
/// Simple model class which defines basic information about an Asteroids player.
/// </summary>
public class GameState
{
    #region properties
    /// <summary>
    /// Whether or not the player is still playing.
    /// </summary>
    public bool GameOver { get; set; }
    /// <summary>
    /// The current HP of the player's spaceship.
    /// </summary>
    public int HP { get; set; }
    /// <summary>
    /// The current player's score.
    /// </summary>
    public int Score { get; set; }
    #endregion

    #region ctor
    public GameState(bool gameOver, int hp, int score)
    {
        this.GameOver = GameOver;
        this.HP = hp;
        this.Score = score;
    }
    #endregion
}
