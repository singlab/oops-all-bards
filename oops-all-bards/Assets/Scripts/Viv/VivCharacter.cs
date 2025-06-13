using UnityEngine;
using DELP;
using System.Collections.Generic;

namespace Viv
{
    [RequireComponent(typeof(VivCharacterController))]
    public class VivCharacter : MonoBehaviour
    {
        [Header("Core Info")]
        public string characterName = "DefaultName";
        [Tooltip("Unique ID for this character used by AI systems.")]
        public int characterID = -1;
        [Tooltip("Path to the prefab for this character in Resources, used for instantiation in the Viv system.")]
        public string prefabPath;
        [Tooltip("The current supertask assigned to this character.")]
        [SerializeField] private Supertask currentSupertask;
        public Supertask CurrentSupertask => currentSupertask;
        [Tooltip("List of active behavior names for this character. Used to track behaviors in the Viv system.")]
        public List<string> ActiveBehaviorNames { get; private set; } = new List<string>();

        [Header("Viv Persona")]
        public VivPersona persona;

        [Header("AI Components")]
        [Tooltip("The DELP knowledge base asset for this character.")]
        public DELPEntity delpEntity;


        [Header("Component References")]
        [SerializeField]
        private VivCharacterController characterController;
        public VivCharacterController Controller => characterController;


        protected virtual void Awake()
        {
            if (characterController == null)
            {
                characterController = GetComponent<VivCharacterController>();
            }
        }


        protected virtual void Start()
        {
            // Validate required data
            if (characterID == -1)
            {
                Debug.LogWarning($"VivCharacter ID for '{characterName}' is not set. Please assign a valid ID.", this);
            }
            if (delpEntity == null)
            {
                Debug.LogError($"VivCharacter '{characterName}' is missing its required DELPEntity ScriptableObject!", this);
                enabled = false; // Disable this component if critical data is missing
                return;
            }

            // Register with the main Viv system
            if (Viv.Instance != null)
            {
                Viv.RegisterCharacter(this);
            }
            else
            {
                Debug.LogError($"Viv instance not found when trying to register {characterName}. Make sure Viv initializes first.");
            }

            // Initialize the DELP knowledge base for this character
            if (delpEntity != null)
            {
                delpEntity.PrepareAndUpdate();
                Debug.Log($"VivCharacter: DELPEntity initialized for {characterName} with ID {characterID}.");
            }
            else
            {
                Debug.LogError($"VivCharacter: DELPEntity is null for {characterName}. Cannot initialize knowledge base.");
            }

            // After registration, this character determines and acquires its own task.
            string desiredSupertaskName = null;

            // Logic to determine which supertask this specific character should perform.
            if (this.characterID == 1) // Quinton
            {
                desiredSupertaskName = "SabotagePlayer";
            }
            else if (this.characterID == 2) // Wurguth
            {
                desiredSupertaskName = "ProtectGuildOfShadows";
            }

            // If this character has a task defined...
            if (!string.IsNullOrEmpty(desiredSupertaskName))
            {
                // ...ask the central Viv library to create an instance of that task.
                Supertask myTask = Viv.Instance.CreateSupertaskForCharacter(desiredSupertaskName, this);

                // If the task was created successfully, assign it.
                if (myTask != null)
                {
                    this.AssignSupertask(myTask);
                }
            }
        }

        public void AssignSupertask(Supertask task)
        {
            this.currentSupertask = task;
            Debug.Log($"VivCharacter: Supertask '{task.Name}' assigned to {this.name}.");

            EvaluateTask();
        }

        public void EvaluateTask()
        {
            if (CurrentSupertask != null)
            {
                CurrentSupertask.BeginEvaluation();
            }
        }

        public void UpdateActiveBehaviors(List<string> newBehaviorNames)
        {
            ActiveBehaviorNames.Clear();
            ActiveBehaviorNames.AddRange(newBehaviorNames);
        }

        public bool IsRunningBehavior(string behaviorName)
        {
            return ActiveBehaviorNames.Contains(behaviorName);
        }

        void OnEnable()
        {
            EventManager.Instance.SubscribeToEvent(EventType.DELP_KnowledgeBaseUpdated, OnKnowledgeBaseUpdated);
        }

        void OnDisable()
        {
            EventManager.Instance.UnsubscribeToEvent(EventType.DELP_KnowledgeBaseUpdated, OnKnowledgeBaseUpdated);
        }

        protected virtual void OnDestroy()
        {
            // Unregister when the GameObject is destroyed
            if (Viv.Instance != null && characterID != -1)
            {
                Viv.UnregisterCharacter(characterID);
            }
        }

        private void OnKnowledgeBaseUpdated(object eventData)
        {
            KnowledgeUpdateEventData knowledgeUpdate = eventData as KnowledgeUpdateEventData;
            if (knowledgeUpdate == null) return;

            if (knowledgeUpdate.characterID == this.characterID)
            {
                Debug.Log($"Character '{this.characterName}' detected an update to its own knowledge base. Re-evaluating supertask.");

                EvaluateTask();
            }
        }

        public void InitializeFromState(PersistentCharacterState state)
        {
            Debug.Log($"Re-initializing {this.characterName} from persistent state.");

            if (this.delpEntity != null)
            {
                this.delpEntity.ClearRuntimeFacts();
            }

            // Re-apply runtime facts to the DELPEntity ScriptableObject instance
            foreach (var fact in state.runtimeFacts)
            {
                this.delpEntity.AddFact(fact);
            }

            // // Re-assign the supertask
            // if (!string.IsNullOrEmpty(state.activeSupertaskName))
            // {
            //     Supertask myTask = Viv.Instance.CreateSupertaskForCharacter(state.activeSupertaskName, this);
            //     this.AssignSupertask(myTask);
            // }
        }
    }
}