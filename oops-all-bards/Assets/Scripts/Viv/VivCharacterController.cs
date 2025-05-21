// using UnityEngine;
// using UnityEngine.AI;
// using System.Collections.Generic;
// using Viv;

// [RequireComponent(typeof(Animator))]
// [RequireComponent(typeof(NavMeshAgent))]
// [RequireComponent(typeof(VivCharacter))]
// public class VivCharacterController : MonoBehaviour
// {
//     #region Enums and State
//     // --- Public Enums ---
//     public enum CharacterAIState
//     {
//         Idle,           // Doing nothing specific, waiting for commands or triggers
//         MovingToTarget, // Actively moving towards a position or GameObject
//         Observing,      // Watching a specific target
//         Interacting,    // Generic state for short interactions (e.g., picking up item)
//         ExecutingAction,// State for actions with duration (e.g., setting trap animation)
//         InDialogue,     // Engaged via DialogueManager
//         Patrolling,     // Moving between waypoints
//         Attacking,
//         Fleeing,
//         Searching,
//         Guarding,
//         Confronting,    // Added explicitly
//         Questioning,    // Added explicitly
//         SettingTrap     // Added explicitly
//     }

//     // --- Public Properties / State ---
//     [Header("State Info")]
//     [SerializeField]
//     private CharacterAIState currentState = CharacterAIState.Idle;
//     public CharacterAIState CurrentState => currentState;

//     private enum AttackSequencePhase
//     {
//         None,           // Default, or when Attacking state is not active
//         Yelling,        // Playing the yell animation
//         InAggroDialogue,// In the hostile dialogue
//         TransitionToCombat // Ready to load combat scene
//     }
//     private AttackSequencePhase currentAttackSequencePhase = AttackSequencePhase.None;
//     #endregion

//     #region Inspector Fields
//     // --- Perception Settings ---
//     [Header("Perception Settings")]
//     [Tooltip("Tag used on trigger colliders for sensitive areas.")]
//     [SerializeField] private string sensitiveAreaTag = "SensitiveArea";
//     [Tooltip("How often the 'Observe' event can trigger for sensitive area presence (seconds).")]
//     [SerializeField] private float observationTriggerCooldown = 5.0f; // Cooldown to prevent spam
//     [Header("Interaction Settings")]
//     [Tooltip("Standard interaction distance.")]
//     [SerializeField] private float interactionDistance = 2.0f;
//     [Tooltip("How fast the character turns to face targets.")]
//     [SerializeField] private float rotationSpeed = 5.0f;
//     // --- Component References ---
//     [Header("Component References (Auto-assigned)")]
//     [SerializeField] protected Animator animator;
//     [SerializeField] protected NavMeshAgent navMeshAgent;
//     [SerializeField] protected VivCharacter vivCharacter;
//     #endregion

//     #region Internal State
//     // --- Internal State ---
//     protected GameObject currentTargetObject;      // GameObject target for actions like Observe, Confront, Attack
//     protected Vector3 currentMoveDestination;   // Position target for movement
//     protected CharacterAIState stateAfterMoving = CharacterAIState.Idle; // What state to enter after reaching destination
//     protected float stateTimer;                 // Timer for states that might time out
//     private bool canTriggerObservationEvent = true; // Cooldown flag for observation trigger
//     #endregion

//     #region Interrupt Handling
//     private bool isInterruptRequested = false;
//     private CharacterAIState nextStateAfterInterrupt = CharacterAIState.Idle;
//     private GameObject targetForNextStateAfterInterrupt = null;
//     #endregion

//     #region Initialization
//     // --- Setup ---
//     protected virtual void Awake()
//     {
//         // Cache required components
//         animator = GetComponent<Animator>();
//         navMeshAgent = GetComponent<NavMeshAgent>();
//         vivCharacter = GetComponent<VivCharacter>();

//         // Basic validation
//         if (animator == null || navMeshAgent == null || vivCharacter == null)
//         {
//             Debug.LogError($"VivCharacterController on {gameObject.name} is missing required components!", this);
//             enabled = false; // Disable if components are missing
//         }
//     }

//     protected virtual void Start()
//     {
//         // Set initial state
//         TransitionToState(CharacterAIState.Idle);
//     }
//     #endregion

//     #region State Machine Core
//     // --- Core State Machine Logic ---
//     protected virtual void Update()
//     {
//         if (isInterruptRequested)
//         {
//             isInterruptRequested = false;
//             CharacterAIState stateToTransitionTo = nextStateAfterInterrupt;
//             GameObject newTarget = targetForNextStateAfterInterrupt;

//             targetForNextStateAfterInterrupt = null;

//             Debug.Log($"{vivCharacter.characterName}: Interrupt processed. Transitioning from {currentState} to {stateToTransitionTo}. New target: {newTarget?.name ?? "None"}");

//             // It's important that TransitionToState sets currentTargetObject if newTarget is not null
//             // before OnEnterState for the new state is called.
//             if (newTarget != null)
//             {
//                 currentTargetObject = newTarget;
//             }
//             TransitionToState(stateToTransitionTo);
//             // After transitioning due to interrupt, we might want to skip the rest of this frame's Update for the old state.
//             // However, the TransitionToState will set a new 'currentState', so the switch below will run for the *new* state.
//             // This is generally fine.
//         }

//         stateTimer += Time.deltaTime;

//         // Execute logic based on the current state
//         switch (currentState)
//         {
//             case CharacterAIState.Idle: Update_Idle(); break;
//             case CharacterAIState.MovingToTarget: Update_MovingToTarget(); break;
//             case CharacterAIState.Observing: Update_Observing(); break;
//             case CharacterAIState.ExecutingAction: Update_ExecutingAction(); break;
//             case CharacterAIState.InDialogue: Update_InDialogue(); break;
//             case CharacterAIState.Patrolling: Update_Patrolling(); break;
//             case CharacterAIState.Attacking: Update_Attacking(); break;
//             case CharacterAIState.Confronting: Update_Confronting(); break;
//             case CharacterAIState.Questioning: Update_Questioning(); break;
//             case CharacterAIState.SettingTrap: Update_SettingTrap(); break;
//             // Add cases for other states
//             default: break;
//         }

//         UpdateAnimator(); // Update general animator parameters
//     }

//     protected virtual void UpdateAnimator()
//     {
//         // Update Animator speed based on NavMeshAgent velocity
//         if (animator != null && navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
//         {
//             // Use speed relative to agent's configured max speed for normalized value
//             float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
//             animator.SetFloat("Speed", speed); // Assumes a "Speed" float parameter exists
//         }
//         else if (animator != null)
//         {
//             animator.SetFloat("Speed", 0f); // Set speed to 0 if no agent or not moving
//         }
//     }

//     // --- State Transition ---
//     protected virtual void TransitionToState(CharacterAIState newState)
//     {
//         if (currentState == newState) return; // Already in this state

//         OnExitState(currentState); // Logic for exiting the old state
//         currentState = newState;   // Change state
//         stateTimer = 0f;           // Reset timer
//         OnEnterState(currentState); // Logic for entering the new state
//     }

//     protected virtual void OnEnterState(CharacterAIState state)
//     {
//         Debug.Log($"{vivCharacter?.characterName ?? gameObject.name}: Entering State - {state}");
//         // Setup for the new state (e.g., set animation bools)
//         animator?.SetBool("IsObserving", state == CharacterAIState.Observing);
//         // Reset NavMeshAgent path if entering Idle? Only if it was previously moving.
//         if (state == CharacterAIState.Idle && navMeshAgent.hasPath)
//         {
//             navMeshAgent?.ResetPath();
//         }
//         // Reset observation cooldown flag when entering observing state
//         if (state == CharacterAIState.Observing)
//         {
//             canTriggerObservationEvent = true;
//             CancelInvoke(nameof(ResetObservationTrigger)); // Cancel any pending reset
//         }
//         // Reset target rotation when moving
//         if (navMeshAgent != null) navMeshAgent.updateRotation = (state == CharacterAIState.MovingToTarget || state == CharacterAIState.Patrolling);

//         if (state == CharacterAIState.Attacking)
//         {
//             // Movement to target (if needed) is handled by Action_AttackTarget
//             // before this state is formally entered after arrival.
//             // So, when we enter CharacterAIState.Attacking, we assume we are at/near the target
//             // and ready to start the aggressive sequence.
//             Debug.Log($"{vivCharacter.characterName}: Entered Attacking state. Target: {currentTargetObject?.name}. Starting Yell phase.");
//             TransitionAttackSequencePhase(AttackSequencePhase.Yelling);
//         }
//         else
//         {
//             // If not entering Attacking state, ensure attack phase is reset
//             currentAttackSequencePhase = AttackSequencePhase.None;
//         }
//     }
//     protected virtual void OnExitState(CharacterAIState state)
//     {
//         Debug.Log($"{vivCharacter?.characterName ?? gameObject.name}: Exiting State - {state}");
//         // Cleanup from the old state (e.g., reset animation bools)
//         if (state == CharacterAIState.Observing) animator?.SetBool("IsObserving", false);
//         // Ensure agent stops if exiting movement abruptly
//         // if((state == CharacterAIState.MovingToTarget || state == CharacterAIState.Patrolling) && navMeshAgent.hasPath) {
//         //    navMeshAgent?.ResetPath();
//         // }

//         if (state == CharacterAIState.Attacking)
//         {
//             currentAttackSequencePhase = AttackSequencePhase.None;
//             // Optional: Ensure NavMeshAgent stops if it was somehow moving during a phase
//             // if (navMeshAgent != null && navMeshAgent.isOnNavMesh) navMeshAgent.ResetPath();
//             Debug.Log($"{vivCharacter.characterName}: Exited Attacking state, sequence phase reset.");
//         }
//     }
//     #endregion

//     #region State Implementation Methods
//     protected virtual void Update_Idle()
//     {
//         // Waiting for a command. Could play random idles.
//     }

//     protected virtual void Update_MovingToTarget()
//     {
//         // Check if destination is reached
//         if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh && !navMeshAgent.pathPending)
//         {
//             if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
//             {
//                 // Check if velocity is near zero (ensures agent has actually stopped)
//                 if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.1f)
//                 {
//                     OnDestinationReached(); // Handle arrival
//                 }
//             }
//         }
//         // Optional: Add path validation or timeout logic
//     }

//     protected virtual void Update_Observing()
//     {
//         if (currentTargetObject == null)
//         {
//             Debug.LogWarning($"{vivCharacter.characterName}: Observing state entered without a target.");
//             TransitionToState(CharacterAIState.Idle);
//             return;
//         }

//         FaceTarget(currentTargetObject); // Keep looking at the target

//         // Suspicious Action Check (Player in Sensitive Area)
//         if (canTriggerObservationEvent && currentTargetObject.CompareTag("Player"))
//         {
//             bool playerInSensitiveArea = CheckIfPlayerInSensitiveArea(currentTargetObject);

//             if (playerInSensitiveArea)
//             {
//                 Debug.Log($"{vivCharacter.characterName} observes Player ({currentTargetObject.name}) in sensitive area tagged '{sensitiveAreaTag}'!");
//                 EventManager.Instance.TriggerInteraction(
//                     gameObject,
//                     currentTargetObject,
//                     InteractionTypes.Observe,
//                     OutcomeStrings.Observe.Observe_SuspiciousAction_Sneaking
//                 );
//                 canTriggerObservationEvent = false;
//                 Invoke(nameof(ResetObservationTrigger), observationTriggerCooldown);

//                 // Decide what to do next? Continue observing? Confront?
//                 // TransitionToState(CharacterAIState.Idle); // Example
//             }
//         }
//     }

//     protected virtual void Update_ExecutingAction()
//     {
//         // Used for actions with duration (e.g., playing an animation)
//         // Needs specific logic based on the action being executed
//         Debug.LogWarning("Update_ExecutingAction needs implementation based on the specific action.");
//         // Example: Check if a 'SetTrap' animation is done
//         // if (IsAnimationFinished("SetTrapAnim")) {
//         //     InstantiateTrap(); // Perform the action effect
//         //     OnActionExecutionComplete();
//         //     TransitionToState(CharacterAIState.Idle);
//         // }
//     }

//     protected virtual void Update_InDialogue()
//     {
//         // Wait for DialogueManager to signal dialogue end
//         if (DialogueManager.Instance == null || !DialogueManager.Instance.isInDialogue)
//         {
//             TransitionToState(CharacterAIState.Idle);
//         }
//         // Optional: Face the target if dialogue involves another character
//         if (currentTargetObject != null) FaceTarget(currentTargetObject);
//     }

//     protected virtual void Update_Patrolling()
//     {
//         // Similar to MovingToTarget, but needs waypoint logic
//         if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh && !navMeshAgent.pathPending)
//         {
//             if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
//             {
//                 if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.1f)
//                 {
//                     // Reached Patrol Point
//                     // !! IMPLEMENT: Get next waypoint and call MoveToPosition(nextWaypoint, CharacterAIState.Patrolling)
//                     Debug.Log($"{vivCharacter.characterName} reached patrol point. Needs logic for next point.");
//                     TransitionToState(CharacterAIState.Idle); // Placeholder: Stop patrolling after one point
//                 }
//             }
//         }
//     }

//     protected virtual void Update_Attacking()
//     {
//         if (currentTargetObject == null)
//         {
//             Debug.LogWarning($"{vivCharacter.characterName}: Target lost during Attack sequence. Transitioning to Idle.");
//             TransitionToState(CharacterAIState.Idle);
//             return;
//         }

//         FaceTarget(currentTargetObject); // Continuously face target during all attack phases

//         switch (currentAttackSequencePhase)
//         {
//             case AttackSequencePhase.Yelling:
//                 animator?.SetFloat("Speed", 0f); // Ensure stationary while yelling
//                                                  // The "Yell" animation trigger is now handled in TransitionAttackSequencePhase.
//                                                  // Since we don't wait for yell duration, we immediately try to transition to dialogue.
//                                                  // This phase effectively just ensures the yell was triggered.
//                 Debug.Log($"{vivCharacter.characterName}: Yelling phase nominal, attempting to transition to Aggro Dialogue.");
//                 TransitionAttackSequencePhase(AttackSequencePhase.InAggroDialogue);
//                 break;

//             case AttackSequencePhase.InAggroDialogue:
//                 animator?.SetFloat("Speed", 0f);
//                 // Dialogue was started in TransitionAttackSequencePhase.
//                 // Wait for DialogueManager to signal dialogue end.
//                 if (DialogueManager.Instance != null && !DialogueManager.Instance.isInDialogue)
//                 {
//                     // Dialogue finished
//                     Debug.Log($"{vivCharacter.characterName}: Aggro Dialogue complete.");
//                     TransitionAttackSequencePhase(AttackSequencePhase.TransitionToCombat);
//                 }
//                 break;

//             case AttackSequencePhase.TransitionToCombat:
//                 Debug.Log($"{vivCharacter.characterName}: Aggressive dialogue with {currentTargetObject.name} finished. Preparing to load combat scene.");

//                 // !! IMPLEMENT: Load your combat scene here !!
//                 // Example:
//                 // GameManager.Instance.StartCombatEncounter(vivCharacter, currentTargetObject.GetComponent<VivCharacter>());
//                 // SceneManager.LoadScene("YourCombatSceneName");

//                 // After initiating combat scene load/setup, Wurguth in *this* scene might go idle.
//                 // The VivCharacter in the combat scene would take over.
//                 TransitionToState(CharacterAIState.Idle); // Fallback
//                                                           // currentAttackSequencePhase will be reset to None by OnExitState(Attacking)
//                 break;

//             case AttackSequencePhase.None:
//                 // This case should ideally not be reached if OnEnterState(Attacking)
//                 // correctly sets an initial phase. But as a fallback:
//                 Debug.LogWarning($"{vivCharacter.characterName}: In Attacking state but AttackSequencePhase is None. Resetting to Idle.");
//                 TransitionToState(CharacterAIState.Idle);
//                 break;
//         }
//     }

//     protected virtual void Update_Confronting()
//     {
//         if (currentTargetObject == null)
//         {
//             TransitionToState(CharacterAIState.Idle);
//             return;
//         }

//         FaceTarget(currentTargetObject);

//         // Assuming dialogue initiation happens here
//         if (stateTimer > 0.1f && DialogueManager.Instance != null && !DialogueManager.Instance.isInDialogue)
//         {
//             Debug.Log($"{vivCharacter.characterName} initiating confrontation dialogue with {currentTargetObject.name}");
//             animator?.SetTrigger("Talk"); // <<--- TRIGGER TALK ANIMATION

//             int confrontationDialogueID = GetConfrontationDialogueID(currentTargetObject);

//             if (confrontationDialogueID != -1)
//             {
//                 DialogueManager.Instance.StartDialogue(confrontationDialogueID);
//                 TransitionToState(CharacterAIState.InDialogue);
//             }
//             else
//             {
//                 Debug.LogWarning($"{vivCharacter.characterName}: Could not find confrontation dialogue ID for {currentTargetObject.name}.");
//                 TransitionToState(CharacterAIState.Idle);
//             }
//         }
//         else if (DialogueManager.Instance != null && DialogueManager.Instance.isInDialogue && currentState != CharacterAIState.InDialogue)
//         {
//             TransitionToState(CharacterAIState.InDialogue);
//         }
//     }

//     protected virtual void Update_Questioning()
//     {
//         // State entered after reaching NPC to question
//         Debug.Log($"{vivCharacter.characterName}: Now questioning {currentTargetObject?.name}");
//         // !! IMPLEMENT: Trigger dialogue via DialogueManager !!
//         // Example:
//         // if (currentTargetObject != null && DialogueManager.Instance != null) {
//         //     int dialogueIdToStart = GetQuestioningDialogueID(); // Need logic to find correct dialogue ID
//         //     DialogueManager.Instance.StartDialogue(dialogueIdToStart, gameObject, currentTargetObject);
//         //     TransitionToState(CharacterAIState.InDialogue); // Switch to InDialogue state
//         // } else {
//         //     TransitionToState(CharacterAIState.Idle); // Cannot question
//         // }
//         TransitionToState(CharacterAIState.Idle); // Placeholder
//     }

//     protected virtual void Update_SettingTrap()
//     {
//         // State entered after reaching trap location marker
//         Debug.Log($"{vivCharacter.characterName}: Now setting trap at {currentTargetObject?.name ?? "location"}");
//         // !! IMPLEMENT: Trap setting animation and logic !!
//         // Example:
//         // StartActionAnimation("SetTrapAnim", CharacterAIState.Idle); // Use ExecutingAction state?
//         // Or directly:
//         // animator?.SetTrigger("SetTrap");
//         // Instantiate(trapPrefab, currentMoveDestination, Quaternion.identity);
//         // EventManager.Instance.TriggerInteraction(...); // Maybe trigger event "TrapSet"?
//         TransitionToState(CharacterAIState.Idle); // Placeholder
//     }

//     #endregion

//     #region Helper Methods
//     // --- Helper Methods ---
//     protected virtual void OnDestinationReached()
//     {
//         Debug.Log($"{vivCharacter?.characterName ?? gameObject.name}: Reached Destination {currentMoveDestination}");
//         // Transition to the state intended after movement
//         CharacterAIState nextState = stateAfterMoving; // Use the stored next state
//         stateAfterMoving = CharacterAIState.Idle; // Reset for next move
//         TransitionToState(nextState);
//     }

//     // Checks if the player is currently inside a trigger tagged with sensitiveAreaTag
//     protected virtual bool CheckIfPlayerInSensitiveArea(GameObject playerObject)
//     {
//         if (playerObject == null) return false;
//         // Uses OverlapSphere - ensure player and sensitive areas have colliders.
//         Collider[] hits = Physics.OverlapSphere(playerObject.transform.position, 0.5f); // Small radius around player center
//         foreach (Collider hit in hits)
//         {
//             // Check the tag directly
//             if (hit.CompareTag(sensitiveAreaTag))
//             {
//                 return true; // Found a sensitive area trigger the player is inside
//             }
//         }
//         return false; // Not inside any sensitive area trigger
//     }

//     // Cooldown reset method
//     private void ResetObservationTrigger()
//     {
//         canTriggerObservationEvent = true;
//     }

//     // Makes the character look towards a target
//     protected virtual void FaceTarget(GameObject target)
//     {
//         if (target == null || navMeshAgent == null) return;

//         // Temporarily disable NavMeshAgent rotation if you want smoother Slerp control
//         bool originalUpdateRotation = navMeshAgent.updateRotation;
//         navMeshAgent.updateRotation = false;

//         Vector3 direction = (target.transform.position - transform.position).normalized;
//         if (direction != Vector3.zero) // Prevent LookRotation spitting errors for zero direction
//         {
//             Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Look only on Y axis
//             transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // Use defined rotationSpeed
//         }

//         // Restore NavMeshAgent rotation control if it was enabled
//         navMeshAgent.updateRotation = originalUpdateRotation;
//     }

//     // Placeholder for checking animation completion
//     protected virtual bool IsAnimationFinished(string animationName)
//     {
//         if (animator == null) return true; // Cannot check, assume finished
//         // Check if the current animation state on base layer matches name and is done
//         AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Base layer index is 0
//         return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
//     }

//     // Placeholder - Called from Update_ExecutingAction when action is complete
//     protected virtual void OnActionExecutionComplete()
//     {
//         Debug.Log($"{vivCharacter?.characterName ?? gameObject.name}: Action Execution Complete.");
//         // Often transitions back to Idle or a follow-up state
//     }


//     // Utility for initiating movement and setting the state after arrival
//     protected virtual void MoveToPosition(Vector3 destination, CharacterAIState stateAfterArrival = CharacterAIState.Idle)
//     {
//         if (navMeshAgent == null || !navMeshAgent.enabled || !navMeshAgent.isOnNavMesh)
//         {
//             Debug.LogWarning($"{vivCharacter?.characterName ?? gameObject.name}: Cannot move, NavMeshAgent not ready.");
//             return;
//         }
//         currentMoveDestination = destination;
//         currentTargetObject = null; // Clear object target when moving to position
//         this.stateAfterMoving = stateAfterArrival; // Store the intended state
//         TransitionToState(CharacterAIState.MovingToTarget);
//         navMeshAgent.SetDestination(destination);
//     }

//     private void TransitionAttackSequencePhase(AttackSequencePhase newPhase)
//     {
//         if (currentAttackSequencePhase == newPhase && newPhase != AttackSequencePhase.Yelling) // Allow re-triggering Yell if necessary
//             return;

//         Debug.Log($"{vivCharacter.characterName}: Attack Sequence Phase: {currentAttackSequencePhase} -> {newPhase}");
//         currentAttackSequencePhase = newPhase;
//         // stateTimer for the main CharacterAIState.Attacking is NOT reset here,
//         // as the sub-phases don't rely on it in this simplified version.

//         switch (newPhase)
//         {
//             case AttackSequencePhase.Yelling:
//                 if (navMeshAgent != null && navMeshAgent.isOnNavMesh) navMeshAgent.ResetPath(); // Ensure stopped
//                 if (navMeshAgent != null) navMeshAgent.updateRotation = false; // For FaceTarget control
//                 animator?.SetTrigger("Yell"); // Trigger yell animation
//                 Debug.Log($"{vivCharacter.characterName}: Yell animation triggered.");
//                 break;
//             case AttackSequencePhase.InAggroDialogue:
//                 if (navMeshAgent != null) navMeshAgent.updateRotation = false; // For FaceTarget control
//                 if (currentTargetObject != null && DialogueManager.Instance != null)
//                 {
//                     int hostileDialogueID = GetAggressiveConfrontationDialogueID(currentTargetObject);
//                     if (hostileDialogueID != -1)
//                     {
//                         Debug.Log($"{vivCharacter.characterName}: Starting aggressive dialogue (ID: {hostileDialogueID}) with {currentTargetObject.name}.");
//                         DialogueManager.Instance.StartDialogue(hostileDialogueID);
//                     }
//                     else
//                     {
//                         Debug.LogWarning($"{vivCharacter.characterName}: No hostile dialogue ID found for {currentTargetObject.name}. Skipping dialogue, proceeding to combat transition.");
//                         TransitionAttackSequencePhase(AttackSequencePhase.TransitionToCombat);
//                     }
//                 }
//                 else
//                 {
//                     Debug.LogError($"{vivCharacter.characterName}: Cannot start aggressive dialogue (target or DialogueManager null). Transitioning to Idle.");
//                     TransitionToState(CharacterAIState.Idle); // Critical failure in sequence
//                 }
//                 break;
//             case AttackSequencePhase.TransitionToCombat:
//                 // The actual scene load/combat start is now handled in Update_Attacking's case for this phase.
//                 Debug.Log($"{vivCharacter.characterName}: Ready to transition to combat scene.");
//                 break;
//         }
//     }

//     protected virtual int GetAggressiveConfrontationDialogueID(GameObject target)
//     {
//         // !! IMPLEMENT THIS LOGIC !!
//         // This method needs to determine the correct dialogue ID for Wurguth's
//         // aggressive confrontation with the specific 'target' (likely the player).
//         // It could be based on Wurguth's VivCharacter data, game state, or target properties.
//         // Example:
//         if (vivCharacter.characterName == "Wurguth" && target.CompareTag("Player"))
//         {
//             // You might have a ScriptableObject or config file mapping characters to dialogue IDs
//             // For now, a placeholder:
//             Debug.LogWarning("GetAggressiveConfrontationDialogueID: Returning placeholder ID 101 for Wurguth vs Player.");
//             return 101; // Replace with your actual lookup
//         }
//         Debug.LogError($"GetAggressiveConfrontationDialogueID: No specific ID found for {vivCharacter.characterName} vs {target.name}.");
//         return -1; // Indicates no specific dialogue found
//     }

//     protected virtual int GetConfrontationDialogueID(GameObject target) // For normal confrontation
//     {
//         // !! IMPLEMENT THIS LOGIC for non-aggressive confrontation !!
//         if (vivCharacter.characterName == "Wurguth" && target.CompareTag("Player"))
//         {
//             Debug.LogWarning("GetConfrontationDialogueID: Returning placeholder ID 102 for Wurguth vs Player (non-aggressive).");
//             return 102; // Replace
//         }
//         return -1;
//     }
//     #endregion

//     #region Public Action Methods
//     // =======================================================
//     // --- PUBLIC ACTION METHODS (Called by IABLAction classes) ---
//     // These methods initiate the behavior by setting targets and transitioning state.
//     // The actual work happens in the corresponding Update_State methods.
//     // =======================================================

//     public virtual void Action_ObserveTarget(GameObject target)
//     {
//         if (target == null) { Debug.LogWarning($"{vivCharacter.characterName}: ObserveTarget called with null target."); TransitionToState(CharacterAIState.Idle); return; }
//         Debug.Log($"{vivCharacter.characterName}: ACTION - ObserveTarget ({target.name})");
//         currentTargetObject = target;
//         TransitionToState(CharacterAIState.Observing);
//     }

//     public virtual void Action_ConfrontTarget(GameObject target)
//     {
//         if (target == null) { Debug.LogWarning($"{vivCharacter.characterName}: ConfrontTarget called with null target."); return; }
//         Debug.Log($"{vivCharacter.characterName}: ACTION - ConfrontTarget ({target.name})");
//         currentTargetObject = target;
//         MoveToPosition(target.transform.position, CharacterAIState.Confronting); // Move first, then confront state
//     }

//     public virtual void Action_QuestionNPC(GameObject npc)
//     {
//         if (npc == null) { Debug.LogWarning($"{vivCharacter.characterName}: QuestionNPC called with null npc."); return; }
//         Debug.Log($"{vivCharacter.characterName}: ACTION - QuestionNPC ({npc.name})");
//         currentTargetObject = npc;
//         MoveToPosition(npc.transform.position, CharacterAIState.Questioning); // Move first, then questioning state
//     }

//     public virtual void Action_SetTrapAtLocation(GameObject locationMarker)
//     {
//         if (locationMarker == null) { Debug.LogWarning($"{vivCharacter.characterName}: SetTrapAtLocation called with null marker."); return; }
//         Debug.Log($"{vivCharacter.characterName}: ACTION - SetTrapAtLocation ({locationMarker.name})");
//         currentTargetObject = locationMarker; // Store marker mainly for position
//         MoveToPosition(locationMarker.transform.position, CharacterAIState.SettingTrap); // Move first, then setting trap state
//     }

//     public virtual void Action_PatrolTo(Vector3 destination)
//     {
//         Debug.Log($"{vivCharacter.characterName}: ACTION - PatrolTo ({destination})");
//         currentTargetObject = null;
//         currentMoveDestination = destination;
//         stateAfterMoving = CharacterAIState.Patrolling; // Set state after arrival to continue patrolling
//         TransitionToState(CharacterAIState.Patrolling); // Use patrolling state for movement logic
//         if (navMeshAgent != null) navMeshAgent.SetDestination(destination);
//     }

//     public virtual void Action_AttackTarget(GameObject target)
//     {
//         if (target == null)
//         {
//             Debug.LogWarning($"{vivCharacter.characterName}: Action_AttackTarget called with null target.");
//             TransitionToState(CharacterAIState.Idle);
//             return;
//         }
//         Debug.Log($"{vivCharacter.characterName}: ACTION - Initiating AGGRESSIVE approach for target ({target.name})");
//         currentTargetObject = target;

//         float distance = Vector3.Distance(transform.position, target.transform.position);
//         // Use interactionDistance or a specific engageRange to decide if movement is needed
//         if (distance <= interactionDistance)
//         {
//             // Already in range, directly transition to Attacking state.
//             // OnEnterState(Attacking) will then start the Yelling phase.
//             TransitionToState(CharacterAIState.Attacking);
//         }
//         else
//         {
//             // Need to move closer first.
//             // OnDestinationReached will transition to CharacterAIState.Attacking,
//             // which will then trigger the Yelling phase via its OnEnterState.
//             MoveToPosition(target.transform.position, CharacterAIState.Attacking);
//         }
//     }

//     // --- Add other public action methods corresponding to your ABL acts ---
//     public virtual void Action_ProtectTarget(GameObject targetToProtect) { Debug.LogWarning($"Action_ProtectTarget not implemented on {vivCharacter.characterName}"); }
//     public virtual void Action_Quip() { Debug.LogWarning($"Action_Quip not implemented on {vivCharacter.characterName}"); /* Trigger text bubble? */ }
//     public virtual void Action_GossipWith(List<GameObject> nearbyCharacters) { Debug.LogWarning($"Action_GossipWith not implemented on {vivCharacter.characterName}"); }
//     public virtual void Action_RequestAssistance() { Debug.LogWarning($"Action_RequestAssistance not implemented on {vivCharacter.characterName}"); /* Trigger event/status? */ }
//     // Add Action_Intimidate, Action_EliminateEvidence etc. as needed

//     #endregion
// }

using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Viv;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(VivCharacter))]
public class VivCharacterController : MonoBehaviour
{
    // --- Component References ---
    [Header("Core Components")]
    public Animator animator;         // Made public for behavior components
    public NavMeshAgent navMeshAgent;   // Made public
    public VivCharacter vivCharacter;   // Made public

    // --- Current Active Behavior ---
    private ICharacterBehavior currentActiveBehavior;
    private GameObject currentBehaviorTarget;

    [Header("Interaction Settings")]
    [SerializeField] public float interactionDistance = 2.0f;
    [SerializeField] public float rotationSpeed = 10.0f;

    // Perception settings might move to an ObserveBehavior component later
    [Header("Perception Settings (for internal use or ObserveBehavior)")]
    [SerializeField] private string sensitiveAreaTag = "SensitiveArea";
    [SerializeField] private float observationTriggerCooldown = 5.0f;
    private bool canTriggerObservationEvent = true;


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        vivCharacter = GetComponent<VivCharacter>();

        if (animator == null || navMeshAgent == null || vivCharacter == null)
        {
            Debug.LogError($"VivCharacterController on {gameObject.name} is missing required components!", this);
            enabled = false;
        }
    }

    protected virtual void Start()
    {
        // Initially, no behavior is active
        SetIdleStateVisuals();
    }

    protected virtual void Update()
    {
        // Update the active behavior
        if (currentActiveBehavior != null)
        {
            bool behaviorCompleted = currentActiveBehavior.UpdateBehavior(this);
            if (behaviorCompleted)
            {
                currentActiveBehavior.ExitBehavior(this);
                Debug.Log($"{vivCharacter.characterName}: Behavior {currentActiveBehavior.GetType().Name} completed.");
                currentActiveBehavior = null; // Ready for a new behavior
                SetIdleStateVisuals(); // Revert to idle visuals unless a new behavior is immediately set
                // Inform ABL that the action corresponding to this behavior is done (via WME update)
                SignalActionCompletionToABL(currentActiveBehavior.GetType().Name, true);
            }
        }
        else
        {
            // If no active behavior, ensure character is visually idle
            UpdateAnimatorSpeed();
        }
    }

    protected virtual void UpdateAnimatorSpeed()
    {
        if (animator != null && navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
            animator.SetFloat("Speed", speed);
        }
        else if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    /// <summary>
    /// Sets the character's visual state to idle (e.g., animator parameters).
    /// </summary>
    protected virtual void SetIdleStateVisuals()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            // Reset any other animation bools/triggers that should be off during idle
            // e.g., animator.SetBool("IsObserving", false);
            // e.g., animator.SetBool("IsTalking", false);
        }
        if (navMeshAgent != null && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
        {
            if (navMeshAgent.hasPath) navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
        }
        Debug.Log($"{vivCharacter?.characterName ?? gameObject.name}: Set to Idle Visuals.", this);
    }

    /// <summary>
    /// Sets or changes the character's active behavior.
    /// If a behavior is already running, it will be exited and signaled as interrupted.
    /// </summary>
    /// <param name="newBehavior">The ICharacterBehavior component to activate.</param>
    /// <param name="target">The target GameObject for the new behavior (can be null).</param>
    /// <param name="optionalData">Optional data for the new behavior's EnterBehavior method (e.g., a Vector3 for MoveBehavior).</param>
    public void SetActiveBehavior(ICharacterBehavior newBehavior, GameObject target = null, object optionalData = null)
    {
        // Check if we are trying to set the same behavior with the same target and it's already running.
        // Depending on desired logic, you might want to prevent re-entry or allow it.
        // For now, allowing re-entry (which means Exit then Enter) if it's the exact same component instance.
        // If newBehavior is a different instance of the same type, it will be treated as a new behavior.
        if (currentActiveBehavior != null && currentActiveBehavior == newBehavior && currentBehaviorTarget == target)
        {
            Debug.LogWarning($"{vivCharacter.characterName}: Attempting to re-activate the exact same behavior instance ({newBehavior.GetBehaviorName()}) with the same target. Re-entering.", this);
            // Allowing re-entry will call ExitBehavior then EnterBehavior.
            // If you want to prevent this, you could return here:
            // Debug.Log($"{vivCharacter.characterName}: Behavior {newBehavior.GetBehaviorName()} is already active with the same target. No change.", this);
            // return;
        }

        // If there's an existing behavior, exit it properly and signal interruption
        if (currentActiveBehavior != null)
        {
            Debug.Log($"{vivCharacter.characterName}: Interrupting behavior '{currentActiveBehavior.GetBehaviorName()}' for new behavior '{newBehavior.GetBehaviorName()}'.", this);
            currentActiveBehavior.ExitBehavior(this);
            // Signal that the previous action was interrupted (unless it completed on its own just before this call)
            SignalActionCompletionToABL(currentActiveBehavior.GetBehaviorName(), false, true);
        }

        currentActiveBehavior = newBehavior;
        currentBehaviorTarget = target; // Store the target for the new behavior

        if (currentActiveBehavior != null)
        {
            // currentBehaviorNameForWME = currentActiveBehavior.GetBehaviorName(); // Store for WME signal
            Debug.Log($"{vivCharacter.characterName}: Starting behavior '{currentActiveBehavior.GetBehaviorName()}' with target '{(target ? target.name : "None")}'. OptionalData present: {optionalData != null}", this);
            currentActiveBehavior.EnterBehavior(this, currentBehaviorTarget, optionalData);
        }
        else
        {
            // currentBehaviorNameForWME = "None";
            Debug.LogWarning($"{vivCharacter.characterName}: SetActiveBehavior called with a null newBehavior. Character will become idle.", this);
            SetIdleStateVisuals(); // Ensure character appears idle if no behavior is set
        }
    }

    /// <summary>
    /// Signals to ABL that an action/behavior has completed.
    /// </summary>
    /// <param name="behaviorName">The name of the behavior that completed/ended.</param>
    /// <param name="success">True if the behavior completed successfully, false otherwise.</param>
    /// <param name="interrupted">True if the behavior was interrupted before natural completion.</param>
    protected virtual void SignalActionCompletionToABL(string behaviorName, bool success, bool interrupted = false)
    {
        string status = success ? "Success" : (interrupted ? "Interrupted" : "Failure");
        Debug.Log($"SignalToABL: {vivCharacter.characterName} behavior '{behaviorName}' ended with status: {status}", this);

        // --- YOUR ABL WME UPDATE LOGIC GOES HERE ---
        // Example (pseudo-code, depends on your ABL Java Action / WME update mechanism):
        // ActionStatusWME wme = new ActionStatusWME(vivCharacter.characterID, behaviorName, status);
        // YourAblBridge.Instance.UpdateWME(wme); // Or however you send WMEs to ABL
        // This is crucial for ABL's success_test_wait conditions.
    }


    // --- Public Action Methods ---
    // These will be simpler. They find the right ICharacterBehavior component
    // on this GameObject and call SetActiveBehavior.

    public virtual void Action_ObserveTarget(GameObject target)
    {
        ObserveBehavior observeComp = GetComponent<ObserveBehavior>(); // Or manage a list/dictionary of behaviors
        if (observeComp != null)
        {
            SetActiveBehavior(observeComp, target);
        }
        else Debug.LogError("ObserveBehavior component not found!", this);
    }

    public virtual void Action_AggressiveConfrontation(GameObject target)
    {
        AggressiveConfrontationBehavior aggroConfrontComp = GetComponent<AggressiveConfrontationBehavior>();
        if (aggroConfrontComp != null)
        {
            SetActiveBehavior(aggroConfrontComp, target);
        }
        else Debug.LogError("AggressiveConfrontationBehavior component not found!", this);
    }

    public virtual void Action_CalmConfrontation(GameObject target)
    {
        CalmConfrontationBehavior calmConfrontComp = GetComponent<CalmConfrontationBehavior>();
        if (calmConfrontComp != null)
        {
            SetActiveBehavior(calmConfrontComp, target);
        }
        else Debug.LogError("CalmConfrontationBehavior component not found!", this);
    }

    public virtual void Action_MoveToPosition(Vector3 destination)
    {
        MoveBehavior moveBehavior = GetComponent<MoveBehavior>();
        if (moveBehavior != null)
        {
            Debug.Log($"{vivCharacter.characterName}: VCC - Activating MoveBehavior for position {destination}");
            // Pass the destination as optionalData
            SetActiveBehavior(moveBehavior, null, destination);
        }
        else
        {
            Debug.LogError("MoveBehavior component not found!", this);
        }
    }

    // TODO: Add other Action_ methods for QuestionNPC, SetTrap etc.

    // --- Utility methods that might be used by Behavior Components ---
    public void FaceTarget(GameObject target)
    {
        if (target == null || navMeshAgent == null || !navMeshAgent.enabled) return;
        bool originalUpdateRotation = navMeshAgent.updateRotation;
        navMeshAgent.updateRotation = false;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        navMeshAgent.updateRotation = originalUpdateRotation;
    }

    public bool IsPlayerInSensitiveArea(GameObject playerObject) // Example helper
    {
        if (playerObject == null) return false;
        Collider[] hits = Physics.OverlapSphere(playerObject.transform.position, 0.5f);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(sensitiveAreaTag)) return true;
        }
        return false;
    }
    public void ResetObservationCooldown() => canTriggerObservationEvent = true;
    public bool CanTriggerObservation() => canTriggerObservationEvent;
    public void StartObservationCooldown()
    {
        canTriggerObservationEvent = false;
        Invoke(nameof(ResetObservationCooldown), observationTriggerCooldown);
    }
    public int GetCharacterID() => vivCharacter.characterID;
    public string GetCharacterName() => vivCharacter.characterName;
}