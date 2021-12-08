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
    /// <summary>
    /// A list of additional observers which the AsteroidSpawner injects into every asteroid.
    /// </summary>
    private List<IAsteroidDeathObserver> additionalObservers;
    #endregion

    #region properties
    /// <summary>
    /// The maximum number of asteroids which can be active at once.
    /// </summary>
    public int MaxAsteroids = 20;

    /// <summary>
    /// A list of additional observers which the AsteroidSpawner injects into every Asteroid.
    /// Component is used as a type here because the interface is not serializable by Unity (therefore does not show in the editor)
    /// However, on start, each component is checked for the IAsteroidDeathObserver implementation
    /// </summary>
    public List<GameObject> AdditionalDeathObservers = new List<GameObject>();
    #endregion

    #region methods
    void Start()
    {
        additionalObservers = new List<IAsteroidDeathObserver>();
        builder = GetComponent<AsteroidBuilder>();
        foreach(GameObject go in AdditionalDeathObservers)
        {
            //find the concrete script(s) that implement the IAsteroidDeathObserver interface
            foreach (MonoBehaviour script in go.GetComponents<MonoBehaviour>())
            {
                if(script is IAsteroidDeathObserver)
                {
                    additionalObservers.Add(script as IAsteroidDeathObserver);
                }
            }
        }
    }

    /// <summary>
    /// Spawns an attack asteroid on this players field. The asteroid counts against the maximum limit, but can be spawned
    /// even if the maximum amount of asteroids are on the field. This method can be called externally.
    /// </summary>
    public void SpawnAttackAsteroid()
    {
        builder.Reset();
        builder.BuildAttack();
        SpawnAndInjectObservers();
    }

    void Update()
    {
        time_since_last_asteroid += Time.deltaTime;
        if(time_since_last_asteroid > 1.0f && asteroid_count < MaxAsteroids)
        {
            builder.Reset();
            //1 in 5 chance to build a large asteroid
            if (Random.Range(0, 10) < 2) builder.BuildLarge();
            SpawnAndInjectObservers();
        }
    }

    /// <summary>
    /// Spawns the Asteroid currently stored in the builder, and injects all active death observers into it.
    /// </summary>
    private void SpawnAndInjectObservers()
    {
        GameObject obj = builder.Spawn();
        //Register this as an observer for the asteroid death
        obj.GetComponent<BaseAsteroidBehaviour>().register(this);
        //Register additional observers
        foreach (IAsteroidDeathObserver o in additionalObservers)
            obj.GetComponent<BaseAsteroidBehaviour>().register(o);
        time_since_last_asteroid = 0;
        asteroid_count++;
    }

    public void NotifyDeath(GameObject asteroid)
    {
        asteroid_count--;
    }
    #endregion
}
