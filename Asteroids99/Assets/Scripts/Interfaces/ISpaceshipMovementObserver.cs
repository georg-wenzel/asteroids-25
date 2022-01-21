using UnityEngine;
/// <summary>
/// Interface for Observers of the Ship's current movementt
/// </summary>
public interface ISpaceshipMovementObserver
{
    void PublishSpaceshipMovement(Vector2 movement);
}