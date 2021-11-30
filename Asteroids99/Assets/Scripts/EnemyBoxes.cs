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


}
