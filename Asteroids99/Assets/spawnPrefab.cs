using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPrefab : MonoBehaviour
{

    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(-8,8);

        float y = Random.Range(-4,4);

        Instantiate(prefab, new Vector2(x,y), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
