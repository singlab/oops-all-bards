using UnityEngine;
using Viv;

[RequireComponent(typeof(VivCharacterController))]
public class AggressiveConfrontationBehavior : MonoBehaviour, ICharacterBehavior
{
    private VivCharacterController controller;
    private GameObject confrontTarget;

    private enum Phase { None, Approaching, Yelling, InAggroDialogue }
    private Phase currentPhase;
    private float phaseTimer;

    private float yellAnimationApproxDuration = 2.0f;

    public string GetBehaviorName() => "AggressiveConfrontation";

    // Assign in the inspector
    public int dialogueIDToStart;

    public void EnterBehavior(VivCharacterController controller, GameObject target, object optionalData = null)
    {
        this.controller = controller;
        this.confrontTarget = target;
        this.phaseTimer = 0f;

        Debug.Log($"{controller.GetCharacterName()}: Entering AggressiveConfrontationBehavior, target: {target?.name}");

        if (confrontTarget == null)
        {
            Debug.LogError("AggressiveConfrontationBehavior started with null target.");
            currentPhase = Phase.None; // Will complete immediately
            return;
        }

        // Check distance and decide if approach is needed
        float distance = Vector3.Distance(controller.transform.position, confrontTarget.transform.position);
        if (distance > controller.interactionDistance)
        {
            TransitionPhase(Phase.Approaching);
            controller.navMeshAgent.SetDestination(confrontTarget.transform.position);
        }
        else
        {
            TransitionPhase(Phase.Yelling); // Already in range
        }
    }

    public bool UpdateBehavior(VivCharacterController controller)
    {
        phaseTimer += Time.deltaTime;

        switch (currentPhase)
        {
            case Phase.Approaching:
                controller.FaceTarget(confrontTarget); // Face while approaching
                controller.animator.SetFloat("Speed", controller.navMeshAgent.velocity.magnitude / controller.navMeshAgent.speed);
                if (!controller.navMeshAgent.pathPending && controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance)
                {
                    if (!controller.navMeshAgent.hasPath || controller.navMeshAgent.velocity.sqrMagnitude < 0.1f)
                    {
                        TransitionPhase(Phase.Yelling);
                    }
                }
                break;

            case Phase.Yelling:
                controller.FaceTarget(confrontTarget);
                controller.animator.SetFloat("Speed", 0f);
                // Yell anim triggered in TransitionPhase. Wait for it to "finish" (timer for now).
                if (phaseTimer >= yellAnimationApproxDuration)
                {
                    TransitionPhase(Phase.InAggroDialogue);
                }
                break;

            case Phase.InAggroDialogue:
                controller.FaceTarget(confrontTarget);
                controller.animator.SetFloat("Speed", 0f);
                return true; // Behavior complete, scene change will handle the rest

            case Phase.None: // Should not happen if EnterBehavior sets a phase
                return true; // Complete (or fail)
        }
        return false; // Behavior still running
    }

    public void ExitBehavior(VivCharacterController controller)
    {
        Debug.Log($"{controller.GetCharacterName()}: Exiting AggressiveConfrontationBehavior.");
        controller.animator.SetFloat("Speed", 0f); // Ensure speed is reset
        if (controller.navMeshAgent.enabled && controller.navMeshAgent.isOnNavMesh) controller.navMeshAgent.ResetPath();
        currentPhase = Phase.None;
        // Any other cleanup
    }

    private void TransitionPhase(Phase newPhase)
    {
        Debug.Log($"{controller.GetCharacterName()}: AggroConfront Phase: {currentPhase} -> {newPhase}");
        currentPhase = newPhase;
        phaseTimer = 0f;

        switch (newPhase)
        {
            case Phase.Approaching:
                if (controller.navMeshAgent.enabled) controller.navMeshAgent.updateRotation = true;
                controller.navMeshAgent.isStopped = false;
                break;
            case Phase.Yelling:
                if (controller.navMeshAgent.enabled)
                {
                    controller.navMeshAgent.ResetPath(); // Stop movement
                    controller.navMeshAgent.updateRotation = false; // For FaceTarget
                }
                controller.animator.SetTrigger("Yell");
                break;
            case Phase.InAggroDialogue:
                if (controller.navMeshAgent.enabled) controller.navMeshAgent.updateRotation = false;
                if (confrontTarget != null && DialogueManager.Instance != null)
                {
                    if (dialogueIDToStart != -1)
                    {
                        DialogueManager.Instance.StartDialogue(dialogueIDToStart);
                    }
                }
                break;
        }
    }
}
