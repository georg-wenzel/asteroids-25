using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for continously spawning Asteroids
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    #region fields
    /// <summary>
    /// The time since the last asteroid was spawned
    /// </summary>
    float time_since_last_asteroid = 0;
    /// <summary>
    /// The asteroid builder script
    /// </summary>
    AsteroidBuilder builder;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        builder = GetComponent<AsteroidBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        time_since_last_asteroid += Time.deltaTime;
        if(time_since_last_asteroid > 2.0f)
        {
            builder.Reset();
            //1 in 4 chance to build a large asteroid
            if (Random.Range(0, 10) < 3) builder.BuildLarge();
            builder.Spawn();
            time_since_last_asteroid = 0;
        }
    }
}
