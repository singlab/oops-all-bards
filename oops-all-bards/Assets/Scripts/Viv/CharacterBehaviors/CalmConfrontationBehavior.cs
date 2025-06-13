using UnityEngine;
using Viv;

[RequireComponent(typeof(VivCharacterController))]
public class CalmConfrontationBehavior : MonoBehaviour, ICharacterBehavior
{
    private VivCharacterController controller;
    private GameObject confrontTarget;

    private enum Phase { None, MovingToTarget, InDialogue, Done }
    private Phase currentPhase;

    // Assign in the inspector
    public int dialogueIDToStart;

    public string GetBehaviorName() => "CalmConfrontation";

    public void EnterBehavior(VivCharacterController controller, GameObject target, object optionalData = null)
    {
        this.controller = controller;
        this.confrontTarget = target;
        this.phaseTimer = 0f; // Reset timer used by this behavior instance

        if (confrontTarget == null)
        {
            Debug.LogError($"{controller.GetCharacterName()}: CalmConfrontationBehavior started with null target. Aborting.", this);
            currentPhase = Phase.Done; // Mark as done to exit immediately
            return;
        }

        Debug.Log($"{controller.GetCharacterName()}: Entering {GetBehaviorName()} with target {confrontTarget.name}");

        // Check distance and decide if approach is needed
        float distance = Vector3.Distance(controller.transform.position, confrontTarget.transform.position);
        if (distance > controller.interactionDistance)
        {
            TransitionPhase(Phase.MovingToTarget);
            controller.navMeshAgent.SetDestination(confrontTarget.transform.position);
        }
        else
        {
            TransitionPhase(Phase.InDialogue); // Already in range, start dialogue directly
        }
    }

    private float phaseTimer; // For internal timing if needed

    public bool UpdateBehavior(VivCharacterController controller)
    {
        if (confrontTarget == null && currentPhase != Phase.Done)
        {
            Debug.LogWarning($"{controller.GetCharacterName()}: Target lost during {GetBehaviorName()}. Exiting.");
            return true; // Behavior complete (failed)
        }

        phaseTimer += Time.deltaTime;

        switch (currentPhase)
        {
            case Phase.MovingToTarget:
                controller.FaceTarget(confrontTarget); // Face while approaching
                if (!controller.navMeshAgent.pathPending && controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance)
                {
                    if (!controller.navMeshAgent.hasPath || controller.navMeshAgent.velocity.sqrMagnitude < 0.1f)
                    {
                        Debug.Log($"{controller.GetCharacterName()}: Reached target for calm confrontation.");
                        TransitionPhase(Phase.InDialogue);
                    }
                }
                break;

            case Phase.InDialogue:
                controller.FaceTarget(confrontTarget);
                controller.animator.SetFloat("Speed", 0f); // Ensure stationary for dialogue
                if (DialogueManager.Instance != null && !DialogueManager.Instance.isInDialogue)
                {
                    Debug.Log($"{controller.GetCharacterName()}: Calm confrontation dialogue with {confrontTarget.name} finished.");
                    TransitionPhase(Phase.Done);
                }
                break;

            case Phase.Done:
                return true; // Signal behavior completion
        }
        return false; // Behavior still running
    }

    public void ExitBehavior(VivCharacterController controller)
    {
        Debug.Log($"{controller.GetCharacterName()}: Exiting {GetBehaviorName()}.");
        controller.animator.SetFloat("Speed", 0f); // Ensure speed is reset
        // Reset NavMeshAgent path if it was moving
        if (currentPhase == Phase.MovingToTarget && controller.navMeshAgent.enabled && controller.navMeshAgent.isOnNavMesh)
        {
            controller.navMeshAgent.ResetPath();
        }
        currentPhase = Phase.None;
    }

    private void TransitionPhase(Phase newPhase)
    {
        Debug.Log($"{controller.GetCharacterName()} ({GetBehaviorName()}): Phase {currentPhase} -> {newPhase}");
        currentPhase = newPhase;
        phaseTimer = 0f;

        switch (newPhase)
        {
            case Phase.MovingToTarget:
                if (controller.navMeshAgent.enabled) controller.navMeshAgent.updateRotation = true;
                controller.navMeshAgent.isStopped = false;
                break;
            case Phase.InDialogue:
                if (controller.navMeshAgent.enabled && controller.navMeshAgent.isOnNavMesh) controller.navMeshAgent.ResetPath(); // Stop movement
                if (controller.navMeshAgent.enabled) controller.navMeshAgent.updateRotation = false; // For FaceTarget control during dialogue
                controller.animator.SetTrigger("Talk"); // Assuming a generic "Talk" trigger for dialogue

                if (confrontTarget != null && DialogueManager.Instance != null)
                {
                    if (dialogueIDToStart != -1) // Already fetched in EnterBehavior
                    {
                        DialogueManager.Instance.StartDialogue(dialogueIDToStart);
                    }
                    else
                    {
                        Debug.LogWarning($"{controller.GetCharacterName()}: No dialogue ID for calm confrontation. Behavior will end.");
                        currentPhase = Phase.Done; // Cannot proceed
                    }
                }
                else
                {
                    Debug.LogError($"{controller.GetCharacterName()}: Cannot start calm confrontation dialogue (target or DialogueManager null). Behavior will end.");
                    currentPhase = Phase.Done; // Cannot proceed
                }
                break;
        }
    }
}
