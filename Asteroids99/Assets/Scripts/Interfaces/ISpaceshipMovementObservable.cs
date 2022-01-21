/// <summary>
/// Interface for Observables providing the current Ship's movement vector
/// </summary>
public interface ISpaceshipMovementObservable
{
    void register(ISpaceshipMovementObserver observer);
    void unregister(ISpaceshipMovementObserver observer);
    void notifyAll();
}
