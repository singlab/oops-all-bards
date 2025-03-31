using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viv 
{
    public class VivCharacter : MonoBehaviour
    {
        public string characterName = "DefaultName";
        public int characterID = -1;
        public DELPEntity delpEntity;

        void Start()
        {
            // Initialization logic specific to VivCharacter can go here
            Debug.Log($"VivCharacter '{characterName}' initialized.");
            
            if (characterID == -1)
            {
                Debug.LogWarning($"VivCharacter ID for character '{characterName}' is not set. Please assign a valid ID.");
            }

            if (delpEntity == null)
            {
                Debug.LogError($"VivCharacter '{gameObject.name}' is missing its required DELPEntity ScriptableObject!", this);
                // this.enabled = false;
                return;
            }

            Viv.RegisterCharacter(this);
        }
    }
}
