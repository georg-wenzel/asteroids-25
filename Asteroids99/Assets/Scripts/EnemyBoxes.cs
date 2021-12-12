using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking;

[System.Serializable]
public class EnemyBoxes : MonoBehaviour
{

    void Awake()
    {
        GameObject.Find("Main Camera Lobby").GetComponent<AudioListener>().enabled = false;
        GameObject.Find("LobbyUI").SetActive(false);
    }

    void Start()
    {
        List<GameState> gameStates = new List<GameState>();
        for(int i=0; i<25; i++)
        {
            gameStates.Add(new GameState(false, 3, 0));
        }
        UpdateEnemySquares(gameStates);
    }

    public void UpdateEnemySquares(List<GameState> gameStates)
    {
        for(int i=0; i<gameStates.Count; i++)
        {
            EnemySquare enemySquare = (EnemySquare)transform.GetChild(i).GetComponent(typeof(EnemySquare));
            enemySquare.UpdateUI(gameStates[i]);
        }
    }

    /*
        Set localPlayer in EnemyBoxes.
        Get all EnemyPlayers.
        Asssign each enemyPlayer to an EnemySquare.
    */
    // public void InitializeEnemyBoxes()
    // {
    //     Match match = Player.localPlayer.currentMatch;
    //     Player localPlayer;
    //     List<Player> enemyPlayers = new List<Player>();
    //     foreach (Player pl in match.players)
    //     {
    //         if(pl == Player.localPlayer)
    //         {
    //             localPlayer = pl;
    //         }
    //         else
    //         {
    //             Debug.Log("Added enemy player " + pl);
    //             enemyPlayers.Add(pl);
    //         }
    //     }
    //     if (enemyPlayers.Count > 24)
    //     {
    //         return;
    //     }
    //     for(int i=0; i<enemyPlayers.Count; i++)
    //     {
    //         EnemySquare enemySquare = (EnemySquare)transform.GetChild(i).GetComponent(typeof(EnemySquare));
    //         enemySquare.localPlayer = enemyPlayers[i];
    //         Debug.Log(enemyPlayers[i] + " has HP " + enemyPlayers[i].gameState.HP);
    //         enemySquare.UpdateUI();
    //     } 
    // }

    public void SpawnAsteroidOnOtherPlayer(EnemySquare sourceSquare)
    {
        EnemySquare destination = GetRandomNeighbourSquare(sourceSquare);
        Player.localPlayer.SpawnAttackAsteroid(destination.localPlayer);
    }

    public EnemySquare GetRandomNeighbourSquare(EnemySquare enemySquare)
    {
        List<EnemySquare> allEnemySquares = new List<EnemySquare>();
        for(int i=0; i<transform.childCount; i++)
        {
            allEnemySquares.Add((EnemySquare)transform.GetChild(i).GetComponent(typeof(EnemySquare)));
        }

        /* get 2 neighbours
        // if a neighbour is already GameOver, take next neighbour
        */
        int currentSquareIndex = allEnemySquares.IndexOf(enemySquare);
        EnemySquare neighbour_1 = getPrevEnemenySquare(currentSquareIndex, allEnemySquares);
        EnemySquare neighbour_2 = getNextEnemenySquare(currentSquareIndex, allEnemySquares);
        
        // select random neighbour (one of the 2 from above)
        EnemySquare selectedNeighbour;
        System.Random rand = new System.Random();
        if (rand.Next(0, 2) == 0)
            selectedNeighbour = neighbour_1;
        else
            selectedNeighbour = neighbour_2;
        
        Debug.Log(enemySquare.gameObject.name + " " + selectedNeighbour.gameObject.name);
        return selectedNeighbour;
    }

    private EnemySquare getNextEnemenySquare(int currentSquareIndex, List<EnemySquare> allEnemySquares)
    {
        EnemySquare nextNeighbour;
        if (currentSquareIndex == 23)
            nextNeighbour = allEnemySquares[0];
        else
        {
            nextNeighbour = allEnemySquares[currentSquareIndex+1];
            if(nextNeighbour.currentGameState.GameOver)
                return getNextEnemenySquare(currentSquareIndex+1, allEnemySquares);
        }
        return nextNeighbour;
    }

    private EnemySquare getPrevEnemenySquare(int currentSquareIndex, List<EnemySquare> allEnemySquares)
    {
        EnemySquare prevNeighbour;
        if (currentSquareIndex == 0)
            prevNeighbour = allEnemySquares[23];
        else
        {
            prevNeighbour = allEnemySquares[currentSquareIndex-1];
            if(prevNeighbour.currentGameState.GameOver)
                return getPrevEnemenySquare(currentSquareIndex-1, allEnemySquares);
        }
        return prevNeighbour;
    }


}
