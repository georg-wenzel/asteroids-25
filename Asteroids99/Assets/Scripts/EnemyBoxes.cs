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
        List<string> playerNames = new List<string>();
        List<int> playerIndices = new List<int>();
        for(int i=1; i<26; i++)
        {
            gameStates.Add(new GameState(false, 3, 0));
            playerNames.Add("Player " + i);
            playerIndices.Add(i);
        }
        UpdateEnemySquares(gameStates,playerNames,playerIndices);
    }

    public void UpdateEnemySquares(List<GameState> gameStates, List<string> playerNames, List<int> playerIndices)
    {
        for(int i=0; i<gameStates.Count; i++)
        {
            var childCount = transform.childCount;
            if(childCount > i)
            {
                EnemySquare enemySquare = (EnemySquare) transform.GetChild(i).GetComponent(typeof(EnemySquare));
                enemySquare.playerIndex = playerIndices[i];
                enemySquare.UpdateUI(gameStates[i], playerNames[i]);
            }

        }
    }

    public void SpawnAsteroidOnOtherPlayer(EnemySquare sourceSquare)
    {
        if(!sourceSquare.isGameOver)
        {
            Player.localPlayer.SpawnAttackAsteroid(sourceSquare.playerIndex);
            return;
        }
        EnemySquare destination = GetRandomNeighbourSquare(sourceSquare);
        if (destination == null)
            return;
        Player.localPlayer.SpawnAttackAsteroid(destination.playerIndex);
    }

    public EnemySquare GetRandomNeighbourSquare(EnemySquare enemySquare)
    {
        List<EnemySquare> allEnemySquares = new List<EnemySquare>();
        for(int i=0; i<transform.childCount; i++)
        {
            EnemySquare es = (EnemySquare)transform.GetChild(i).GetComponent(typeof(EnemySquare));
            allEnemySquares.Add(es);
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
        
        if(selectedNeighbour == null)
            return null;
        Debug.Log(enemySquare.gameObject.name + " " + selectedNeighbour.gameObject.name);
        return selectedNeighbour;
    }

    private EnemySquare getNextEnemenySquare(int currentSquareIndex, List<EnemySquare> allEnemySquares, int tries = 0)
    {
        if(tries > 24)
            return null;
        EnemySquare nextNeighbour;
        if (currentSquareIndex == 23)
            nextNeighbour = allEnemySquares[0];
        else
        {
            nextNeighbour = allEnemySquares[currentSquareIndex+1];
        }
        if(nextNeighbour.isGameOver)
                return getNextEnemenySquare(currentSquareIndex+1, allEnemySquares, tries+1);
        return nextNeighbour;
    }

    private EnemySquare getPrevEnemenySquare(int currentSquareIndex, List<EnemySquare> allEnemySquares, int tries = 0)
    {
        if(tries > 24)
            return null;
        EnemySquare prevNeighbour;
        if (currentSquareIndex == 0)
            prevNeighbour = allEnemySquares[23];
        else
        {
            prevNeighbour = allEnemySquares[currentSquareIndex-1];
        }
        if(prevNeighbour.isGameOver)
                return getPrevEnemenySquare(currentSquareIndex-1, allEnemySquares, tries+1);
        return prevNeighbour;
    }


}
