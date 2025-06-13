using UnityEngine;
using UnityEngine.AI;

public class MoveToTargetBehavior : MonoBehaviour, ICharacterBehavior
{
    private VivCharacterController controller;
    private GameObject target;
    private float repathTimer;
    private const float REPATH_INTERVAL = 0.25f;
    public string GetBehaviorName() => "MoveToTarget";

    public void EnterBehavior(VivCharacterController controller, GameObject target, object optionalData)
    {
        this.controller = controller;
        this.target = target;
        this.repathTimer = 0f;

        if (target == null)
        {
            Debug.LogError("MoveToTargetBehavior started with a null target.");
            return;
        }

        if (controller.navMeshAgent != null && controller.navMeshAgent.enabled)
        {
            // Immediately calculate the offset destination and set the path
            controller.navMeshAgent.SetDestination(GetTargetDestination());
            controller.navMeshAgent.isStopped = false;
        }
    }

    public bool UpdateBehavior(VivCharacterController controller)
    {
        if (target == null)
        {
            Debug.LogWarning($"{controller.vivCharacter.characterName}: Target lost during {GetBehaviorName()}. Behavior complete.");
            return true;
        }

        // Re-path periodically
        repathTimer += Time.deltaTime;
        if (repathTimer >= REPATH_INTERVAL)
        {
            repathTimer = 0f;
            controller.navMeshAgent.SetDestination(GetTargetDestination());
        }

        if (!controller.navMeshAgent.pathPending)
        {
            if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance)
            {
                if (!controller.navMeshAgent.hasPath || controller.navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log($"{controller.vivCharacter.characterName}: Reached vicinity of target {target.name}.");
                    return true; // destination reached
                }
            }
        }

        return false; // Still moving.
    }

    public void ExitBehavior(VivCharacterController controller)
    {
        if (controller.navMeshAgent != null && controller.navMeshAgent.enabled)
        {
            controller.navMeshAgent.isStopped = true;
        }
    }

    // Helper method to get the target destination.
    private Vector3 GetTargetDestination()
    {
        if (target == null || controller == null)
        {
            return transform.position;
        }

        return target.transform.position;
    }
}