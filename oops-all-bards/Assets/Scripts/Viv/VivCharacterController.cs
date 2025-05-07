using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Viv;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(VivCharacter))]
public class VivCharacterController : MonoBehaviour
{
    #region Enums and State
    // --- Public Enums ---
    public enum CharacterAIState
    {
        Idle,           // Doing nothing specific, waiting for commands or triggers
        MovingToTarget, // Actively moving towards a position or GameObject
        Observing,      // Watching a specific target
        Interacting,    // Generic state for short interactions (e.g., picking up item)
        ExecutingAction,// State for actions with duration (e.g., setting trap animation)
        InDialogue,     // Engaged via DialogueManager
        Patrolling,     // Moving between waypoints
        Attacking,
        Fleeing,
        Searching,
        Guarding,
        Confronting,    // Added explicitly
        Questioning,    // Added explicitly
        SettingTrap     // Added explicitly
    }

    // --- Public Properties / State ---
    [Header("State Info")]
    [SerializeField]
    private CharacterAIState currentState = CharacterAIState.Idle;
    public CharacterAIState CurrentState => currentState;
    #endregion

    #region Inspector Fields
    // --- Perception Settings ---
    [Header("Perception Settings")]
    [Tooltip("Tag used on trigger colliders for sensitive areas.")]
    [SerializeField] private string sensitiveAreaTag = "SensitiveArea";
    [Tooltip("How often the 'Observe' event can trigger for sensitive area presence (seconds).")]
    [SerializeField] private float observationTriggerCooldown = 5.0f; // Cooldown to prevent spam
    // [Tooltip("Standard interaction distance.")]
    // [SerializeField] private float interactionDistance = 2.0f;
    [Tooltip("How fast the character turns to face targets.")]
    [SerializeField] private float rotationSpeed = 5.0f;


    // --- Component References ---
    [Header("Component References (Auto-assigned)")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected VivCharacter vivCharacter;
    #endregion

    #region Internal State
    // --- Internal State ---
    protected GameObject currentTargetObject;      // GameObject target for actions like Observe, Confront, Attack
    protected Vector3 currentMoveDestination;   // Position target for movement
    protected CharacterAIState stateAfterMoving = CharacterAIState.Idle; // What state to enter after reaching destination
    protected float stateTimer;                 // Timer for states that might time out
    private bool canTriggerObservationEvent = true; // Cooldown flag for observation trigger
    #endregion

    #region Initialization
    // --- Setup ---
    protected virtual void Awake()
    {
        // Cache required components
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        vivCharacter = GetComponent<VivCharacter>();

        // Basic validation
        if (animator == null || navMeshAgent == null || vivCharacter == null)
        {
            Debug.LogError($"VivCharacterController on {gameObject.name} is missing required components!", this);
            enabled = false; // Disable if components are missing
        }
    }

    protected virtual void Start()
    {
        // Set initial state
        TransitionToState(CharacterAIState.Idle);
    }
    #endregion

    #region State Machine Core
    // --- Core State Machine Logic ---
    protected virtual void Update()
    {
        stateTimer += Time.deltaTime;

        // Execute logic based on the current state
        switch (currentState)
        {
            case CharacterAIState.Idle: Update_Idle(); break;
            case CharacterAIState.MovingToTarget: Update_MovingToTarget(); break;
            case CharacterAIState.Observing: Update_Observing(); break;
            case CharacterAIState.ExecutingAction: Update_ExecutingAction(); break;
            case CharacterAIState.InDialogue: Update_InDialogue(); break;
            case CharacterAIState.Patrolling: Update_Patrolling(); break;
            case CharacterAIState.Attacking: Update_Attacking(); break;
            case CharacterAIState.Confronting: Update_Confronting(); break;
            case CharacterAIState.Questioning: Update_Questioning(); break;
            case CharacterAIState.SettingTrap: Update_SettingTrap(); break;
            // Add cases for other states
            default: break;
        }

        UpdateAnimator(); // Update general animator parameters
    }

    protected virtual void UpdateAnimator()
    {
        // Update Animator speed based on NavMeshAgent velocity
        if (animator != null && navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            // Use speed relative to agent's configured max speed for normalized value
            float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
            animator.SetFloat("Speed", speed); // Assumes a "Speed" float parameter exists
        }
        else if (animator != null)
        {
            animator.SetFloat("Speed", 0f); // Set speed to 0 if no agent or not moving
        }
    }

    // --- State Transition ---
    protected virtual void TransitionToState(CharacterAIState newState)
    {
        if (currentState == newState) return; // Already in this state

        OnExitState(currentState); // Logic for exiting the old state
        currentState = newState;   // Change state
        stateTimer = 0f;           // Reset timer
        OnEnterState(currentState); // Logic for entering the new state
    }

    protected virtual void OnEnterState(CharacterAIState state)
    {
        Debug.Log($"{vivCharacter?.characterName ?? gameObject.name}: Entering State - {state}");
        // Setup for the new state (e.g., set animation bools)
        animator?.SetBool("IsObserving", state == CharacterAIState.Observing);
        // Reset NavMeshAgent path if entering Idle? Only if it was previously moving.
        if (state == CharacterAIState.Idle && navMeshAgent.hasPath)
        {
            navMeshAgent?.ResetPath();
        }
        // Reset observation cooldown flag when entering observing state
        if (state == CharacterAIState.Observing)
        {
            canTriggerObservationEvent = true;
            CancelInvoke(nameof(ResetObservationTrigger)); // Cancel any pending reset
        }
        // Reset target rotation when moving
        if (navMeshAgent != null) navMeshAgent.updateRotation = (state == CharacterAIState.MovingToTarget || state == CharacterAIState.Patrolling);

    }
    protected virtual void OnExitState(CharacterAIState state)
    {
        Debug.Log($"{vivCharacter?.characterName ?? gameObject.name}: Exiting State - {state}");
        // Cleanup from the old state (e.g., reset animation bools)
        if (state == CharacterAIState.Observing) animator?.SetBool("IsObserving", false);
        // Ensure agent stops if exiting movement abruptly
        // if((state == CharacterAIState.MovingToTarget || state == CharacterAIState.Patrolling) && navMeshAgent.hasPath) {
        //    navMeshAgent?.ResetPath();
        // }
    }
    #endregion

    #region State Implementation Methods
    // --- State Update Methods (Implement Logic Here) ---

    protected virtual void Update_Idle()
    {
        // Waiting for a command. Could play random idles.
    }

    protected virtual void Update_MovingToTarget()
    {
        // Check if destination is reached
        if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh && !navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // Check if velocity is near zero (ensures agent has actually stopped)
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.1f)
                {
                    OnDestinationReached(); // Handle arrival
                }
            }
        }
        // Optional: Add path validation or timeout logic
    }

    protected virtual void Update_Observing()
    {
        if (currentTargetObject == null)
        {
            Debug.LogWarning($"{vivCharacter.characterName}: Observing state entered without a target.");
            TransitionToState(CharacterAIState.Idle);
            return;
        }

        FaceTarget(currentTargetObject); // Keep looking at the target

        // Suspicious Action Check (Player in Sensitive Area)
        if (canTriggerObservationEvent && currentTargetObject.CompareTag("Player"))
        {
            bool playerInSensitiveArea = CheckIfPlayerInSensitiveArea(currentTargetObject);

            if (playerInSensitiveArea)
            {
                Debug.Log($"{vivCharacter.characterName} observes Player ({currentTargetObject.name}) in sensitive area tagged '{sensitiveAreaTag}'!");
                EventManager.Instance.TriggerInteraction(
                    gameObject,
                    currentTargetObject,
                    InteractionTypes.Observe,
                    OutcomeStrings.Observe.Observe_SuspiciousAction_Sneaking
                );
                canTriggerObservationEvent = false;
                Invoke(nameof(ResetObservationTrigger), observationTriggerCooldown);

                // Decide what to do next? Continue observing? Confront?
                // TransitionToState(CharacterAIState.Idle); // Example
            }
        }
    }

    protected virtual void Update_ExecutingAction()
    {
        // Used for actions with duration (e.g., playing an animation)
        // Needs specific logic based on the action being executed
        Debug.LogWarning("Update_ExecutingAction needs implementation based on the specific action.");
        // Example: Check if a 'SetTrap' animation is done
        // if (IsAnimationFinished("SetTrapAnim")) {
        //     InstantiateTrap(); // Perform the action effect
        //     OnActionExecutionComplete();
        //     TransitionToState(CharacterAIState.Idle);
        // }
    }

    protected virtual void Update_InDialogue()
    {
        // Wait for DialogueManager to signal dialogue end
        if (DialogueManager.Instance == null || !DialogueManager.Instance.isInDialogue)
        {
            TransitionToState(CharacterAIState.Idle);
        }
        // Optional: Face the target if dialogue involves another character
        if (currentTargetObject != null) FaceTarget(currentTargetObject);
    }

    protected virtual void Update_Patrolling()
    {
        // Similar to MovingToTarget, but needs waypoint logic
        if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh && !navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.1f)
                {
                    // Reached Patrol Point
                    // !! IMPLEMENT: Get next waypoint and call MoveToPosition(nextWaypoint, CharacterAIState.Patrolling)
                    Debug.Log($"{vivCharacter.characterName} reached patrol point. Needs logic for next point.");
                    TransitionToState(CharacterAIState.Idle); // Placeholder: Stop patrolling after one point
                }
            }
        }
    }

    protected virtual void Update_Attacking()
    {
        // !! IMPLEMENT combat logic !!
        // - Face target
        // - Check range
        // - Trigger attack animations/damage dealing via CombatManager/stats
        // - Check if target defeated
        // - Check if should flee
        Debug.LogWarning("Update_Attacking needs implementation.");
        if (currentTargetObject != null) FaceTarget(currentTargetObject);
        // Example: Transition if target is gone or defeated
        // if (currentTargetObject == null || IsTargetDefeated(currentTargetObject)) {
        //     TransitionToState(CharacterAIState.Idle);
        // }
    }

    protected virtual void Update_Confronting()
    {
        // State entered after reaching target for confrontation
        Debug.Log($"{vivCharacter.characterName}: Now confronting {currentTargetObject?.name}");
        // !! IMPLEMENT: Trigger dialogue via DialogueManager !!
        // Example:
        // if (currentTargetObject != null && DialogueManager.Instance != null) {
        //     int dialogueIdToStart = GetConfrontationDialogueID(); // Need logic to find correct dialogue ID
        //     DialogueManager.Instance.StartDialogue(dialogueIdToStart, gameObject, currentTargetObject);
        //     TransitionToState(CharacterAIState.InDialogue); // Switch to InDialogue state
        // } else {
        //     TransitionToState(CharacterAIState.Idle); // Cannot confront
        // }
        TransitionToState(CharacterAIState.Idle); // Placeholder
    }

    protected virtual void Update_Questioning()
    {
        // State entered after reaching NPC to question
        Debug.Log($"{vivCharacter.characterName}: Now questioning {currentTargetObject?.name}");
        // !! IMPLEMENT: Trigger dialogue via DialogueManager !!
        // Example:
        // if (currentTargetObject != null && DialogueManager.Instance != null) {
        //     int dialogueIdToStart = GetQuestioningDialogueID(); // Need logic to find correct dialogue ID
        //     DialogueManager.Instance.StartDialogue(dialogueIdToStart, gameObject, currentTargetObject);
        //     TransitionToState(CharacterAIState.InDialogue); // Switch to InDialogue state
        // } else {
        //     TransitionToState(CharacterAIState.Idle); // Cannot question
        // }
        TransitionToState(CharacterAIState.Idle); // Placeholder
    }

    protected virtual void Update_SettingTrap()
    {
        // State entered after reaching trap location marker
        Debug.Log($"{vivCharacter.characterName}: Now setting trap at {currentTargetObject?.name ?? "location"}");
        // !! IMPLEMENT: Trap setting animation and logic !!
        // Example:
        // StartActionAnimation("SetTrapAnim", CharacterAIState.Idle); // Use ExecutingAction state?
        // Or directly:
        // animator?.SetTrigger("SetTrap");
        // Instantiate(trapPrefab, currentMoveDestination, Quaternion.identity);
        // EventManager.Instance.TriggerInteraction(...); // Maybe trigger event "TrapSet"?
        TransitionToState(CharacterAIState.Idle); // Placeholder
    }

    #endregion

    #region Helper Methods
    // --- Helper Methods ---
    protected virtual void OnDestinationReached()
    {
        Debug.Log($"{vivCharacter?.characterName ?? gameObject.name}: Reached Destination {currentMoveDestination}");
        // Transition to the state intended after movement
        CharacterAIState nextState = stateAfterMoving; // Use the stored next state
        stateAfterMoving = CharacterAIState.Idle; // Reset for next move
        TransitionToState(nextState);
    }

    // Checks if the player is currently inside a trigger tagged with sensitiveAreaTag
    protected virtual bool CheckIfPlayerInSensitiveArea(GameObject playerObject)
    {
        if (playerObject == null) return false;
        // Uses OverlapSphere - ensure player and sensitive areas have colliders.
        Collider[] hits = Physics.OverlapSphere(playerObject.transform.position, 0.5f); // Small radius around player center
        foreach (Collider hit in hits)
        {
            // Check the tag directly
            if (hit.CompareTag(sensitiveAreaTag))
            {
                return true; // Found a sensitive area trigger the player is inside
            }
        }
        return false; // Not inside any sensitive area trigger
    }

    // Cooldown reset method
    private void ResetObservationTrigger()
    {
        canTriggerObservationEvent = true;
    }

    // Makes the character look towards a target
    protected virtual void FaceTarget(GameObject target)
    {
        if (target == null || navMeshAgent == null) return;

        // Temporarily disable NavMeshAgent rotation if you want smoother Slerp control
        bool originalUpdateRotation = navMeshAgent.updateRotation;
        navMeshAgent.updateRotation = false;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        if (direction != Vector3.zero) // Prevent LookRotation spitting errors for zero direction
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Look only on Y axis
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // Use defined rotationSpeed
        }

        // Restore NavMeshAgent rotation control if it was enabled
        navMeshAgent.updateRotation = originalUpdateRotation;
    }

    // Placeholder for checking animation completion
    protected virtual bool IsAnimationFinished(string animationName)
    {
        if (animator == null) return true; // Cannot check, assume finished
        // Check if the current animation state on base layer matches name and is done
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Base layer index is 0
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }

    // Placeholder - Called from Update_ExecutingAction when action is complete
    protected virtual void OnActionExecutionComplete()
    {
        Debug.Log($"{vivCharacter?.characterName ?? gameObject.name}: Action Execution Complete.");
        // Often transitions back to Idle or a follow-up state
    }


    // Utility for initiating movement and setting the state after arrival
    protected virtual void MoveToPosition(Vector3 destination, CharacterAIState stateAfterArrival = CharacterAIState.Idle)
    {
        if (navMeshAgent == null || !navMeshAgent.enabled || !navMeshAgent.isOnNavMesh)
        {
            Debug.LogWarning($"{vivCharacter?.characterName ?? gameObject.name}: Cannot move, NavMeshAgent not ready.");
            return;
        }
        currentMoveDestination = destination;
        currentTargetObject = null; // Clear object target when moving to position
        this.stateAfterMoving = stateAfterArrival; // Store the intended state
        TransitionToState(CharacterAIState.MovingToTarget);
        navMeshAgent.SetDestination(destination);
    }
    #endregion

    #region Public Action Methods
    // =======================================================
    // --- PUBLIC ACTION METHODS (Called by IABLAction classes) ---
    // These methods initiate the behavior by setting targets and transitioning state.
    // The actual work happens in the corresponding Update_State methods.
    // =======================================================

    public virtual void Action_ObserveTarget(GameObject target)
    {
        if (target == null) { Debug.LogWarning($"{vivCharacter.characterName}: ObserveTarget called with null target."); TransitionToState(CharacterAIState.Idle); return; }
        Debug.Log($"{vivCharacter.characterName}: ACTION - ObserveTarget ({target.name})");
        currentTargetObject = target;
        TransitionToState(CharacterAIState.Observing);
    }

    public virtual void Action_ConfrontTarget(GameObject target)
    {
        if (target == null) { Debug.LogWarning($"{vivCharacter.characterName}: ConfrontTarget called with null target."); return; }
        Debug.Log($"{vivCharacter.characterName}: ACTION - ConfrontTarget ({target.name})");
        currentTargetObject = target;
        MoveToPosition(target.transform.position, CharacterAIState.Confronting); // Move first, then confront state
    }

    public virtual void Action_QuestionNPC(GameObject npc)
    {
        if (npc == null) { Debug.LogWarning($"{vivCharacter.characterName}: QuestionNPC called with null npc."); return; }
        Debug.Log($"{vivCharacter.characterName}: ACTION - QuestionNPC ({npc.name})");
        currentTargetObject = npc;
        MoveToPosition(npc.transform.position, CharacterAIState.Questioning); // Move first, then questioning state
    }

    public virtual void Action_SetTrapAtLocation(GameObject locationMarker)
    {
        if (locationMarker == null) { Debug.LogWarning($"{vivCharacter.characterName}: SetTrapAtLocation called with null marker."); return; }
        Debug.Log($"{vivCharacter.characterName}: ACTION - SetTrapAtLocation ({locationMarker.name})");
        currentTargetObject = locationMarker; // Store marker mainly for position
        MoveToPosition(locationMarker.transform.position, CharacterAIState.SettingTrap); // Move first, then setting trap state
    }

    public virtual void Action_PatrolTo(Vector3 destination)
    {
        Debug.Log($"{vivCharacter.characterName}: ACTION - PatrolTo ({destination})");
        currentTargetObject = null;
        currentMoveDestination = destination;
        stateAfterMoving = CharacterAIState.Patrolling; // Set state after arrival to continue patrolling
        TransitionToState(CharacterAIState.Patrolling); // Use patrolling state for movement logic
        if (navMeshAgent != null) navMeshAgent.SetDestination(destination);
    }

    public virtual void Action_AttackTarget(GameObject target)
    {
        if (target == null) { Debug.LogWarning($"{vivCharacter.characterName}: AttackTarget called with null target."); return; }
        Debug.Log($"{vivCharacter.characterName}: ACTION - AttackTarget ({target.name})");
        currentTargetObject = target;
        TransitionToState(CharacterAIState.Attacking);
        // Combat system might take over from here
    }

    // --- Add other public action methods corresponding to your ABL acts ---
    public virtual void Action_ProtectTarget(GameObject targetToProtect) { Debug.LogWarning($"Action_ProtectTarget not implemented on {vivCharacter.characterName}"); }
    public virtual void Action_Quip() { Debug.LogWarning($"Action_Quip not implemented on {vivCharacter.characterName}"); /* Trigger text bubble? */ }
    public virtual void Action_GossipWith(List<GameObject> nearbyCharacters) { Debug.LogWarning($"Action_GossipWith not implemented on {vivCharacter.characterName}"); }
    public virtual void Action_RequestAssistance() { Debug.LogWarning($"Action_RequestAssistance not implemented on {vivCharacter.characterName}"); /* Trigger event/status? */ }
    // Add Action_Intimidate, Action_EliminateEvidence etc. as needed

    #endregion
}