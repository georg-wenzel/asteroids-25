/// <summary>
/// Interface for Observables providing the current Ship's HP
/// </summary>
public interface IHPObservable
{
    void register(IHPObserver observer);
    void unregister(IHPObserver observer);
    void notifyAll();
}
