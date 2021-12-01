using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoxes : MonoBehaviour
{
    
    public void UpdateEnemyBoxes(GameState[] gameStates)
    {
        if(gameStates.Length != 24)
        {
            Debug.Log("GameStates.Length must be 24");
            return;
        }

        for(int i=0; i<transform.childCount; i++)
        {
            EnemySquare script = (EnemySquare)transform.GetChild(i).GetComponent(typeof(EnemySquare));
            script.UpdateUI(gameStates[i]);
        }
    }

    public void SpawnAsteroidOnOtherPlayer(EnemySquare sourceSquare)
    {
        EnemySquare destination = GetRandomNeighbourSquare(sourceSquare);
        // TODO spawn asteroid in enemy square
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
