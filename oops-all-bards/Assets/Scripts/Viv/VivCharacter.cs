using UnityEngine;
using DELP;

namespace Viv
{
    [RequireComponent(typeof(VivCharacterController))]
    public class VivCharacter : MonoBehaviour
    {
        [Header("Core Info")]
        public string characterName = "DefaultName";
        [Tooltip("Unique ID for this character used by AI systems.")]
        public int characterID = -1;

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
        }

        protected virtual void OnDestroy()
        {
            // Unregister when the GameObject is destroyed
            if (Viv.Instance != null && characterID != -1)
            {
                Viv.UnregisterCharacter(characterID);
            }
        }
    }
}