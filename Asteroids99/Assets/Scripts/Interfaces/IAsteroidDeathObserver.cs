using UnityEngine;
/// <summary>
/// Interface for Observers of Asteroid deaths
/// </summary>
public interface IAsteroidDeathObserver
{
    void NotifyDeath(GameObject asteroid);
}
