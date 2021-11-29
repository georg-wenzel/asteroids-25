using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for continously spawning Asteroids
/// </summary>
public class AsteroidSpawner : MonoBehaviour, IAsteroidDeathObserver
{
    #region fields
    /// <summary>
    /// The time since the last asteroid was spawned
    /// </summary>
    float time_since_last_asteroid = 0;
    /// <summary>
    /// The number of currently active asteroids
    /// </summary>
    int asteroid_count = 0;
    /// <summary>
    /// The asteroid builder script
    /// </summary>
    AsteroidBuilder builder;
    #endregion

    #region properties
    /// <summary>
    /// The maximum number of asteroids which can be active at once.
    /// </summary>
    public int MaxAsteroids = 20;
    #endregion

    #region methods
    void Start()
    {
        builder = GetComponent<AsteroidBuilder>();
    }

    void Update()
    {
        time_since_last_asteroid += Time.deltaTime;
        if(time_since_last_asteroid > 1.0f && asteroid_count < MaxAsteroids)
        {
            builder.Reset();
            //1 in 5 chance to build a large asteroid
            if (Random.Range(0, 10) < 2) builder.BuildLarge();
            GameObject obj = builder.Spawn();
            //Register this as an observer for the asteroid death
            obj.GetComponent<BaseAsteroidBehaviour>().register(this);

            time_since_last_asteroid = 0;
            asteroid_count++;
        }
    }

    public void NotifyDeath(GameObject asteroid)
    {
        asteroid_count--;
    }
    #endregion
}
