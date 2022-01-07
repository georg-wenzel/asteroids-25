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
    /// The time the last asteroid was spawned
    /// </summary>
    float last_asteroid = 0;
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
        StartCoroutine(SpawnNewAsteroid());
    }

    /// <summary>
    /// Spawns an attack asteroid on this players field. The asteroid counts against the maximum limit, but can be spawned
    /// even if the maximum amount of asteroids are on the field. This method can be called externally.
    /// </summary>
    /// <param name="sourceID">The source of the attack asteroid</param>
    public void SpawnAttackAsteroid(int sourceID)
    {
        builder.Reset();
        builder.BuildAttack();
        builder.BuildPersistent();
        var asteroid = SpawnAndInjectObservers();
        asteroid.GetComponent<AttackAsteroidSource>().SourcePlayerID = sourceID;
    }

    /// <summary>
    /// Spawns the Asteroid currently stored in the builder, and injects all active death observers into it.
    /// </summary>
    private GameObject SpawnAndInjectObservers()
    {
        GameObject obj = builder.Spawn();
        //Register this as an observer for the asteroid death
        obj.GetComponent<BaseAsteroidBehaviour>().register(this);
        //Register additional observers
        foreach (IAsteroidDeathObserver o in additionalObservers)
            obj.GetComponent<BaseAsteroidBehaviour>().register(o);
        last_asteroid = Time.time;
        asteroid_count++;
        return obj;
    }

    /// <summary>
    /// When this object is notified of an asteroids death, decrease the asteroid count
    /// </summary>
    /// <param name="asteroid">The asteroid that died</param>
    public void NotifyDeath(GameObject asteroid)
    {
        //Decrease asteroid count
        asteroid_count--;

        //If it was a large asteroid
        if(asteroid.transform.localScale.x > 1)
        {
            //1 in 3 chance to spawn an attack asteroid
            if(Random.Range(0,3) == 0)
            {
                //build and spawn a friendly asteroid aiming towards the missile impact direction
                builder.Reset();
                builder.BuildFriendly();
                builder.BuildTargeted(asteroid.GetComponent<BaseAsteroidBehaviour>().ImpactDirection);
                builder.BuildSpecificSpawn(asteroid.transform.position);
                //Spawn this asteroid without injecting death observers, as it does not interact like usual asteroids
                GameObject go = builder.Spawn();
                //Give this asteroid the "FriendlyAsteroid" tag to make it easier to identify downstream
                go.tag = "FriendlyAsteroid";
            }
        }
    }

    /// <summary>
    /// Coroutine to spawn a new asteroid every 1 second (= if none was spawned in the last 0.9 seconds)
    /// </summary>
    IEnumerator SpawnNewAsteroid()
    {
        for(; ;)
        {
            if (Time.time - last_asteroid >= 0.9f && asteroid_count < MaxAsteroids)
            {
                builder.Reset();
                //1 in 5 chance to build a large asteroid
                if (Random.Range(0, 10) < 2) builder.BuildLarge();
                builder.BuildPersistent();
                SpawnAndInjectObservers();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion
}
