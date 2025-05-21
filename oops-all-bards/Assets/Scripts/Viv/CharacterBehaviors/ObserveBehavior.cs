using UnityEngine;
using Viv;

[RequireComponent(typeof(VivCharacterController))]
public class ObserveBehavior : MonoBehaviour, ICharacterBehavior
{
    private VivCharacterController controller;
    private GameObject observeTarget;
    private float observationTimer;
    private float observationDuration; // Could be set by ABL action parameters

    public string GetBehaviorName() => "Observe";

    public void EnterBehavior(VivCharacterController controller, GameObject target, object optionalData = null)
    {
        this.controller = controller;
        this.observeTarget = target;
        this.observationTimer = 0f;
        this.observationDuration = 10.0f; // Example: Observe for 10 seconds, or make this dynamic

        Debug.Log($"{controller.GetCharacterName()}: Entering ObserveBehavior, target: {target?.name ?? "None"}");
        controller.animator.SetBool("IsObserving", true); // Assuming "IsObserving" bool for anim

        // Reset perception cooldown for this observation session
        controller.ResetObservationCooldown();
    }

    public bool UpdateBehavior(VivCharacterController controller)
    {
        if (observeTarget == null)
        {
            Debug.LogWarning($"{controller.GetCharacterName()}: ObserveTarget became null. Exiting ObserveBehavior.");
            return true; // Behavior complete (or failed)
        }

        controller.FaceTarget(observeTarget); // Use public FaceTarget from controller

        // Perception logic (player in sensitive area)
        if (controller.CanTriggerObservation() && observeTarget.CompareTag("Player"))
        {
            if (controller.IsPlayerInSensitiveArea(observeTarget))
            {
                Debug.Log($"{controller.GetCharacterName()} observed Player in sensitive area during ObserveBehavior!");
                EventManager.Instance.TriggerInteraction(
                    controller.gameObject, // Observer is the actor
                    observeTarget,         // Observed is the target
                    InteractionTypes.Observe,
                    OutcomeStrings.Observe.Observe_SuspiciousAction_Sneaking
                );
                controller.StartObservationCooldown(); // Start cooldown
            }
        }

        observationTimer += Time.deltaTime;
        if (observationTimer >= observationDuration)
        {
            Debug.Log($"{controller.GetCharacterName()}: Observation duration complete for {observeTarget.name}.");
            return true; // Behavior complete
        }

        return false; // Behavior still running
    }

    public void ExitBehavior(VivCharacterController controller)
    {
        Debug.Log($"{controller.GetCharacterName()}: Exiting ObserveBehavior.");
        controller.animator.SetBool("IsObserving", false);
        // Any other cleanup
    }
}
