using UnityEngine;

/// <summary>
/// Interface for Asteroid observables notifying observers when they die
/// </summary>
public interface IAsteroidDeathObservable
{
    void register(IAsteroidDeathObserver observer);
    void unregister(IAsteroidDeathObserver observer);
    void notifyAll();
}
