using UnityEngine;

public interface ICharacterBehavior
{
    /// <summary>
    /// Called when this behavior becomes active.
    /// Use this to initialize the behavior, set targets, start timers, etc.
    /// </summary>
    /// <param name="controller">The main VivCharacterController instance providing context.</param>
    /// <param name="target">Optional target GameObject for this behavior.</param>
    /// <param name="optionalData">Optional data packet for more complex initialization (e.g., a Vector3 for MoveToPosition).</param>
    void EnterBehavior(VivCharacterController controller, GameObject target = null, object optionalData = null);

    /// <summary>
    /// Called every frame while this behavior is active.
    /// Contains the core logic of the behavior.
    /// Should return true if the behavior has completed, false otherwise.
    /// </summary>
    /// <param name="controller">The main VivCharacterController instance.</param>
    /// <returns>True if behavior is complete, false otherwise.</returns>
    bool UpdateBehavior(VivCharacterController controller);

    /// <summary>
    /// Called when this behavior is deactivated or interrupted.
    /// Use this for cleanup, resetting flags, stopping animations, etc.
    /// </summary>
    /// <param name="controller">The main VivCharacterController instance.</param>
    void ExitBehavior(VivCharacterController controller);

    /// <summary>
    /// Returns a descriptive name for the behavior, useful for logging and WME updates.
    /// </summary>
    string GetBehaviorName();
}