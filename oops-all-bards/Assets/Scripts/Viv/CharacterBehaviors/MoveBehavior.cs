using UnityEngine;
using UnityEngine.AI;
using Viv;

[RequireComponent(typeof(VivCharacterController))]
public class MoveBehavior : MonoBehaviour, ICharacterBehavior
{
    private VivCharacterController controller;
    private Vector3 destination;
    private bool hasReachedDestination;

    public string GetBehaviorName() => "Move";

    /// <summary>
    /// Called when this behavior becomes active.
    /// Initializes movement towards the specified destination.
    /// </summary>
    /// <param name="controller">The main VivCharacterController instance.</param>
    /// <param name="target">Optional target GameObject (not directly used by MoveBehavior for destination, but could be used for target-relative movement in future). Set to null if moving to a point.</param>
    /// <param name="optionalData">Should be a Vector3 representing the world position to move to.</param>
    public void EnterBehavior(VivCharacterController controller, GameObject target = null, object optionalData = null)
    {
        this.controller = controller;
        this.hasReachedDestination = false;

        if (optionalData is Vector3 dest)
        {
            this.destination = dest;
            Debug.Log($"{controller.GetCharacterName()}: Entering {GetBehaviorName()} behavior. Destination: {this.destination}", this);

            if (controller.navMeshAgent != null && controller.navMeshAgent.enabled && controller.navMeshAgent.isOnNavMesh)
            {
                controller.navMeshAgent.SetDestination(this.destination);
                controller.navMeshAgent.isStopped = false; // Ensure agent is not stopped
                // The VivCharacterController.UpdateAnimatorSpeed() should handle animation based on navMeshAgent.velocity
            }
            else
            {
                Debug.LogError($"{controller.GetCharacterName()}: NavMeshAgent is not available or not on NavMesh for {GetBehaviorName()}. Behavior will complete immediately.", this);
                this.hasReachedDestination = true; // Cannot move, so complete immediately
            }
        }
        else
        {
            Debug.LogError($"{controller.GetCharacterName()}: {GetBehaviorName()} expects a Vector3 destination in optionalData. No valid destination provided. Behavior will complete immediately.", this);
            this.hasReachedDestination = true; // Mark as complete due to error
        }
    }

    /// <summary>
    /// Called every frame while this behavior is active.
    /// Checks if the character has reached the destination.
    /// </summary>
    /// <param name="controller">The main VivCharacterController instance.</param>
    /// <returns>True if the destination is reached or an error occurred, false otherwise.</returns>
    public bool UpdateBehavior(VivCharacterController controller)
    {
        if (hasReachedDestination) // If already marked as complete (e.g., due to an error in EnterBehavior)
        {
            return true;
        }

        if (controller.navMeshAgent != null && controller.navMeshAgent.enabled && controller.navMeshAgent.isOnNavMesh)
        {
            // Check if the agent is close enough to the destination
            if (!controller.navMeshAgent.pathPending)
            {
                if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance)
                {
                    // Ensure the agent has a path or has very low velocity (is actually stopped)
                    if (!controller.navMeshAgent.hasPath || controller.navMeshAgent.velocity.sqrMagnitude < 0.1f) // Adjusted for more reliable stopping
                    {
                        Debug.Log($"{controller.GetCharacterName()}: Reached destination for {GetBehaviorName()}.", this);
                        hasReachedDestination = true;
                        return true; // Destination reached
                    }
                }
            }
        }
        else
        {
            // NavMeshAgent became unavailable during update
            Debug.LogWarning($"{controller.GetCharacterName()}: NavMeshAgent became unavailable during {GetBehaviorName()}. Completing behavior.", this);
            hasReachedDestination = true;
            return true;
        }

        return false; // Still moving or path pending
    }

    /// <summary>
    /// Called when this behavior is deactivated or interrupted.
    /// Cleans up by stopping movement and resetting NavMeshAgent path.
    /// </summary>
    /// <param name="controller">The main VivCharacterController instance.</param>
    public void ExitBehavior(VivCharacterController controller)
    {
        Debug.Log($"{controller.GetCharacterName()}: Exiting {GetBehaviorName()} behavior.", this);
        if (controller.navMeshAgent != null && controller.navMeshAgent.enabled && controller.navMeshAgent.isOnNavMesh)
        {
            if (!hasReachedDestination) // If exiting before completion, stop the agent
            {
                controller.navMeshAgent.isStopped = true;
                controller.navMeshAgent.ResetPath(); // Clear the path
                Debug.Log($"{controller.GetCharacterName()}: {GetBehaviorName()} interrupted, NavMeshAgent stopped and path reset.", this);
            }
        }
        // Animation speed should be handled by VivCharacterController's general UpdateAnimatorSpeed based on velocity.
        // If stopped, velocity will be 0, setting Speed to 0.
    }
}
