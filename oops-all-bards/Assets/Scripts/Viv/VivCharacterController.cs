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
    // --- Behavior Management ---
    [Header("Available Behaviors")]
    [Tooltip("Drag ICharacterBehavior components (e.g., MoveBehavior, ObserveBehavior scripts attached to this GameObject) here.")]
    public List<MonoBehaviour> availableBehaviorComponents = new List<MonoBehaviour>(); // Use MonoBehaviour to allow dragging in Inspector
    private readonly Queue<ActionCommand> commandQueue = new Queue<ActionCommand>();
    private ActionCommand currentCommand;
    private bool isIdle = false;

    private Dictionary<System.Type, ICharacterBehavior> behaviorMap = new Dictionary<System.Type, ICharacterBehavior>();
    private ICharacterBehavior currentActiveBehavior;
    private GameObject currentBehaviorTarget;

    [Header("Interaction Settings")]
    [SerializeField] public float interactionDistance = 2.0f;
    [SerializeField] public float rotationSpeed = 10.0f;

    // Perception settings might move to an ObserveBehavior component later
    [Header("Perception Settings (for internal use or ObserveBehavior)")]
    [SerializeField] private string sensitiveAreaTag = "LocationTrigger_SensitiveArea";
    [SerializeField] private float observationTriggerCooldown = 5.0f;
    private bool canTriggerObservationEvent = true;


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        vivCharacter = GetComponent<VivCharacter>();

        // Populate the behaviorMap from the Inspector list
        behaviorMap.Clear();
        foreach (var monoBehaviour in availableBehaviorComponents)
        {
            if (monoBehaviour is ICharacterBehavior characterBehavior)
            {
                if (!behaviorMap.ContainsKey(characterBehavior.GetType()))
                {
                    behaviorMap.Add(characterBehavior.GetType(), characterBehavior);
                }
                else
                {
                    Debug.LogWarning($"Duplicate behavior type {characterBehavior.GetType()} found in availableBehaviorComponents for {gameObject.name}. Using the first one encountered.", this);
                }
            }
            else if (monoBehaviour != null)
            {
                Debug.LogWarning($"Component {monoBehaviour.GetType().Name} in availableBehaviorComponents for {gameObject.name} does not implement ICharacterBehavior.", this);
            }
        }

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
        // First, check if there's no active command.
        if (currentCommand.Behavior == null)
        {
            // If there's no command, check if there's one waiting in the queue.
            if (commandQueue.Count > 0)
            {
                currentCommand = commandQueue.Dequeue();
                isIdle = false;

                Debug.Log($"<color=cyan>Dequeued and starting command: '{currentCommand.Behavior.GetBehaviorName()}'</color>");
                currentCommand.Behavior.EnterBehavior(this, currentCommand.Target, currentCommand.OptionalData);
            }
            else
            {
                // The queue is empty and no command is running.
                if (!isIdle)
                {
                    SetIdleStateVisuals();
                    isIdle = true; // Mark that we are now idle.
                }
            }
        }
        else // A command is currently being executed.
        {
            isIdle = false;
            bool behaviorCompleted = currentCommand.Behavior.UpdateBehavior(this);

            if (behaviorCompleted)
            {
                // The behavior has finished. Clean it up.
                Debug.Log($"Behavior '{currentCommand.Behavior.GetBehaviorName()}' completed.");
                currentCommand.Behavior.ExitBehavior(this);
                SignalActionCompletionToABL(currentCommand.Behavior.GetBehaviorName(), true);

                // Clear the current command slot.
                currentCommand = default;
            }
        }

        // This can always run, as it just syncs animation to the NavMeshAgent's velocity.
        UpdateAnimatorSpeed();
    }

    // Helper method to get a behavior from the map
    private T GetBehavior<T>() where T : class, ICharacterBehavior
    {
        if (behaviorMap.TryGetValue(typeof(T), out ICharacterBehavior behavior))
        {
            return behavior as T;
        }
        Debug.LogError($"{typeof(T).Name} not found in the behavior map for {vivCharacter.characterName}. Ensure it's added to the 'Available Behaviors' list in the Inspector and implements ICharacterBehavior.", this);
        return null;
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
    protected virtual void SignalActionCompletionToABL(string behName, bool success, bool interrupted = false)
    {
        string stat = success ? "Success" : (interrupted ? "Interrupted" : "Failure");
        Debug.Log($"SignalToABL: {vivCharacter.characterName} behavior '{behName}' ended with status: {stat}");

        BehaviorStatusWME statusWME = new BehaviorStatusWME(vivCharacter.characterID, behName, stat);
        ABLMessage messageToSend = statusWME.ToABLMessage();
        TCPTestClient.Instance.SendMessage<ABLMessage>(messageToSend);
    }

    // --- Public Action Methods ---
    public virtual void Action_ObserveTarget(GameObject target, float duration = 10f)
    {
        ObserveBehavior observeComp = GetBehavior<ObserveBehavior>();
        if (observeComp != null)
        {
            commandQueue.Enqueue(new ActionCommand
            {
                Behavior = observeComp,
                Target = target,
                OptionalData = duration
            });
            Debug.Log("Command 'Observe Target' added to queue.");
        }
    }

    public virtual void Action_AggressiveConfrontation(GameObject target)
    {
        AggressiveConfrontationBehavior aggroConfrontComp = GetBehavior<AggressiveConfrontationBehavior>();
        if (aggroConfrontComp != null)
        {
            commandQueue.Enqueue(new ActionCommand
            {
                Behavior = aggroConfrontComp,
                Target = target,
                OptionalData = null
            });
            Debug.Log("Command 'AggressiveConfrontation' added to queue.");
        }
    }

    public virtual void Action_CalmConfrontation(GameObject target)
    {
        CalmConfrontationBehavior calmConfrontComp = GetBehavior<CalmConfrontationBehavior>();
        if (calmConfrontComp != null)
        {
            commandQueue.Enqueue(new ActionCommand
            {
                Behavior = calmConfrontComp,
                Target = target,
                OptionalData = null
            });
            Debug.Log("Command 'MoveToPosition' added to queue.");
        }
    }

    public virtual void Action_MoveToPosition(Vector3 destination)
    {
        MoveBehavior moveComp = GetBehavior<MoveBehavior>();
        if (moveComp != null)
        {
            commandQueue.Enqueue(new ActionCommand
            {
                Behavior = moveComp,
                Target = null,
                OptionalData = destination
            });
            Debug.Log("Command 'MoveToPosition' added to queue.");
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

    // A private struct to hold a behavior and its startup data
    private struct ActionCommand
    {
        public ICharacterBehavior Behavior;
        public GameObject Target;
        public object OptionalData;
    }
}