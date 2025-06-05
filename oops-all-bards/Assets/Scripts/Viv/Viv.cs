using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DELP;

namespace Viv
{
    public class Viv : MonoBehaviour
    {
        // A reference to the bindings between supertasks, behaviors, and assumptions.
        [SerializeField] private CustomDictionary bindings;
        private static Viv _instance;
        public static Viv Instance => Viv._instance;
        // A registry that maps the integer ID of a character to the VivCharacter object associated with that ID.
        [SerializeField] private static Dictionary<int, VivCharacter> characterRegistry = new Dictionary<int, VivCharacter>();

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != null)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        // A character calls this method to get a new instance of a supertask by name.
        public Supertask CreateSupertaskForCharacter(string supertaskName, VivCharacter character)
        {
            if (bindings.SupertaskDict.ContainsKey(supertaskName))
            {
                // For now, we assume the target is the player (ID 0) as a default.
                int targetCharacterId = 0;

                Supertask newTask = new Supertask(supertaskName, character.characterID, targetCharacterId, bindings);
                return newTask;
            }
            else
            {
                Debug.LogError($"Supertask name '{supertaskName}' not found in Viv bindings! Cannot create task for {character.characterName}.");
                return null;
            }
        }

        public static void RegisterCharacter(VivCharacter character)
        {
            // Register the character with the Viv system.
            if (character.characterID == -1)
            {
                Debug.LogError("VivCharacter ID is not set. Please assign a valid ID.");
                return;
            }

            if (!characterRegistry.ContainsKey(character.characterID))
            {
                characterRegistry.Add(character.characterID, character);
                Debug.Log($"Registered VivCharacter '{character.characterName}' with ID {character.characterID}.");
            }
            else
            {
                Debug.LogWarning($"VivCharacter with ID {character.characterID} is already registered. Please use a unique ID.");
            }
        }

        public static void UnregisterCharacter(int characterId)
        {
            if (Instance != null && characterRegistry.ContainsKey(characterId))
            {
                characterRegistry.Remove(characterId);
                Debug.Log($"Unregistered character ID: {characterId}");
            }
        }

        public VivCharacter FindVivCharacter(int characterId)
        {
            characterRegistry.TryGetValue(characterId, out VivCharacter character);
            return character;
        }

        public IEnumerable<VivCharacter> GetAllRegisteredCharacters()
        {
            return new List<VivCharacter>(characterRegistry.Values);
        }

        private void DebugSupertask(Supertask st)
        {
            Debug.Log(st.Name);
            foreach (Behavior b in st.Behaviors)
            {
                Debug.Log(b.Name);
                foreach (Assumption a in b.Assumptions)
                {
                    Debug.Log(a.ToString());
                }
            }
        }

        public DELPEntity FindCharacterDELPEntity(int characterId)
        {
            if (characterRegistry.TryGetValue(characterId, out VivCharacter character))
            {
                return character.delpEntity;
            }

            Debug.LogWarning($"DELPEntity not found for character ID: {characterId}");
            return null;
        }
    }

    [System.Serializable]
    public class Supertask
    {
        // The name of the given supertask.
        [SerializeField] private string name;
        // The list of behaviors associated with the supertask.
        [SerializeField] private List<Behavior> behaviors = new List<Behavior>();
        // The integer ID of the character engaged in the supertask.
        [SerializeField] private int actingCharacter;
        // The integer ID of the character targeted by the supertask.
        [SerializeField] private int targetCharacter;
        // A boolean indicating whether or not the supertask is currently being carried out.
        [SerializeField] private bool inProgress = false;
        // The evaluation of the supertask in terms of its behaviors, containing the TFUs associated with each behavior.
        [SerializeField] private TFU[] evaluation;

        public Supertask(string name, int actingCharacter, int targetCharacter, CustomDictionary bindings)
        {
            this.name = name;
            this.actingCharacter = actingCharacter;
            this.targetCharacter = targetCharacter;
            this.behaviors = this.FormBehaviors(name, bindings);
        }

        private VivWME ToVivWME()
        {
            VivWME wme = new VivWME(actingCharacter);
            return wme;
        }

        // A utility function that assigns the list of behaviors associated with the supertask.
        private List<Behavior> FormBehaviors(string name, CustomDictionary bindings)
        {
            List<Behavior> behaviors = new List<Behavior>();
            List<string> behaviorNames = bindings.SupertaskDict[name];

            foreach (string bname in behaviorNames)
            {
                Behavior toAdd = new Behavior(bname, actingCharacter, targetCharacter, bindings);
                behaviors.Add(toAdd);
            }
            return behaviors;
        }

        // A function that dispatches all behaviors that are not built on false assumptions to the ABL agent.
        public void SelectAndDispatchBehaviors()
        {
            // Ensure the evaluation is current.
            this.Evaluate();

            // Create a list to hold only the behaviors that pass criteria.
            var validBehaviors = new List<Behavior>();

            for (int i = 0; i < behaviors.Count; i++)
            {
                // Core decision-making rule:
                if (evaluation[i].Falsities == 0)
                {
                    validBehaviors.Add(behaviors[i]);
                    Debug.Log($"Behavior '{behaviors[i].Name}' is valid for dispatch (T/F/U: {evaluation[i].Truths}/{evaluation[i].Falsities}/{evaluation[i].Uncertainties}).");
                }
                else
                {
                    Debug.Log($"Behavior '{behaviors[i].Name}' is INVALID for dispatch (T/F/U: {evaluation[i].Truths}/{evaluation[i].Falsities}/{evaluation[i].Uncertainties}).");
                }
            }

            // If there are any valid behaviors, dispatch them.
            if (validBehaviors.Count > 0)
            {
                Debug.Log($"Dispatching {validBehaviors.Count} valid behavior(s) to ABL...");

                // Create the WME with the necessary parameters.
                VivWME wme = new VivWME(this.actingCharacter);

                // Create the list of goals to spawn with parameters.
                List<SpawnGoalData> goalsToSpawn = new List<SpawnGoalData>();
                foreach (Behavior validBehavior in validBehaviors)
                {
                    goalsToSpawn.Add(new SpawnGoalData
                    {
                        name = validBehavior.Name,
                        actingCharacter = this.actingCharacter,
                        targetCharacter = this.targetCharacter
                    });
                }
                wme.ToSpawn = goalsToSpawn.ToArray();

                // Send the message to the server.
                ABLMessage msg = wme.ToABLMessage();
                TCPTestClient.Instance.SendMessage<ABLMessage>(msg);
            }
            else
            {
                Debug.Log("No valid behaviors to dispatch for this supertask at this time.");
            }
        }

        // A function that evaluates the given supertask with respect to its component behaviors.
        public void Evaluate()
        {
            if (this.evaluation == null)
            {
                // Create a new evaluation matrix, m x n, where m = the number of the behaviors belonging to the supertask, and n = 3 (storing truths, falsities, and uncertainties of the behavior).
                this.evaluation = new TFU[behaviors.Count];
            }

            // Evaluate each behavior.
            for (int i = 0; i < behaviors.Count; i++)
            {
                Behavior currentBehavior = behaviors[i];
                currentBehavior.Evaluate();
                evaluation[i] = ScoreBehavior(currentBehavior);
            }
        }

        // A utility function used to count the number of truths (YES), falsities (NO), and uncertainties (UNDECIDED) belonging to each behavior.
        private TFU ScoreBehavior(Behavior behavior)
        {
            TFU score = new TFU();
            string[] eval = behavior.Evaluation;

            for (int i = 0; i < eval.Length; i++)
            {
                string current = eval[i];
                if (current == "YES")
                {
                    score.Truths += 1;
                }
                else if (current == "NO")
                {
                    score.Falsities += 1;
                }
                else
                {
                    score.Uncertainties += 1;
                }
            }

            return score;
        }

        public string Name
        {
            get { return this.name; }
        }

        public List<Behavior> Behaviors
        {
            get { return this.behaviors; }
        }

        public bool InProgress
        {
            get { return this.inProgress; }
            set { this.inProgress = value; }
        }

        public TFU[] Evaluation
        {
            get { return this.evaluation; }
        }
    }

    [System.Serializable]
    public class TFU
    {
        [SerializeField] private int truths;
        [SerializeField] private int falsities;
        [SerializeField] private int uncertainties;

        public TFU()
        {
            this.truths = 0;
            this.falsities = 0;
            this.uncertainties = 0;
        }

        public TFU(int truths, int falsities, int uncertainties)
        {
            this.truths = truths;
            this.falsities = falsities;
            this.uncertainties = uncertainties;
        }

        public int[] ToArray()
        {
            int[] array = { this.truths, this.falsities, this.uncertainties };
            return array;
        }

        public int Truths
        {
            get { return this.truths; }
            set { this.truths = value; }
        }

        public int Falsities
        {
            get { return this.falsities; }
            set { this.falsities = value; }
        }

        public int Uncertainties
        {
            get { return this.uncertainties; }
            set { this.uncertainties = value; }
        }
    }

    [System.Serializable]
    public class Behavior
    {
        // The name of the behavior (should match the name of the corresponding ABL behavior).
        [SerializeField] private string name;
        // The integer ID of the acting character.
        [SerializeField] private int actingCharacter;
        // A list of the assumptions on which each behavior is built.
        [SerializeField] private List<Assumption> assumptions = new List<Assumption>();
        // The evaluation of the behavior in terms of its assumptions, represented as an array of strings, i.e. ["YES","NO","UNDECIDED"].
        [SerializeField] private string[] evaluation;


        public Behavior(string name, int actingCharacter, int targetCharacter, CustomDictionary bindings)
        {
            this.name = name;
            this.actingCharacter = actingCharacter;
            this.assumptions = this.FormAssumptions(name, actingCharacter, targetCharacter, bindings);
        }

        private List<Assumption> FormAssumptions(string name, int actingCharacter, int targetCharacter, CustomDictionary bindings)
        {
            List<Assumption> assumptions = new List<Assumption>();

            // Get the assumption templates from the bindings
            List<string> assumptionTemplates = bindings.BehaviorDict[name];

            // Create the role bindings dict
            var roleBindings = new Dictionary<string, string>();

            // Get the acting character's name for the {self} role
            VivCharacter selfChar = Viv.Instance.FindVivCharacter(actingCharacter);
            if (selfChar != null)
            {
                // e.g., "{self}" -> "Wurguth"
                roleBindings.Add("{self}", selfChar.characterName);
            }

            // Get the target character's name for the {target} role
            // TODO: This lookup should probably use a universal registry
            string targetCharacterName = "Player"; // Default placeholder
            VivCharacter targetChar = Viv.Instance.FindVivCharacter(targetCharacter);
            if (targetChar != null)
            {
                targetCharacterName = targetChar.characterName;
            }
            // e.g., "{target}" -> "Player"
            roleBindings.Add("{target}", targetCharacterName);

            // TODO: Add other roles here in the future, like {eventActor}, {location}, etc.

            // Create an Assumption for each template, passing the bindings to resolve it
            foreach (string template in assumptionTemplates)
            {
                Assumption toAdd = new Assumption(template, actingCharacter, roleBindings);
                assumptions.Add(toAdd);
            }

            return assumptions;
        }

        // A function that evaluates the given behavior by validating/failing to validate the assumptions on which it is built.
        public void Evaluate()
        {
            if (this.evaluation == null)
            {
                this.evaluation = new string[assumptions.Count];
            }

            for (int i = 0; i < assumptions.Count; i++)
            {
                assumptions[i].Validate();
                evaluation[i] = assumptions[i].IsValid.ToString();
            }
        }

        public string Name
        {
            get { return this.name; }
        }

        public List<Assumption> Assumptions
        {
            get { return this.assumptions; }
        }

        public string[] Evaluation
        {
            get { return this.evaluation; }
        }
    }

    [System.Serializable]
    public class Assumption
    {
        // An int ID representing the character making this assumption, so that the correct knowledgebase can be queried.
        [SerializeField] private int actingCharacter;
        [SerializeField] private string template; // e.g., "isHostile({target})"
        [SerializeField] private string resolvedQuery; // e.g., "isHostile(Player)"
        // A temporary DELPResponse to store a returned message from the server.
        [SerializeField] private DELPResponse tmpResponse;
        // Whether or not the assumption holds true for the given owner, predicate, and subject.
        public enum Validity { DEFAULT, YES, NO, UNDECIDED };
        [SerializeField] private Validity isValid;

        // private fields for event management
        private Action<object> assignDelpResponseLambda;

        public Assumption(string template, int actingCharacter, Dictionary<string, string> roleBindings)
        {
            this.template = template;
            this.actingCharacter = actingCharacter;
            this.isValid = Validity.DEFAULT;

            // Resolve the template immediately upon creation
            this.resolvedQuery = ResolveTemplate(template, roleBindings);
        }

        public override string ToString()
        {
            return this.resolvedQuery;
        }

        private string ResolveTemplate(string template, Dictionary<string, string> roleBindings)
        {
            string result = template;
            foreach (var binding in roleBindings)
            {
                result = result.Replace(binding.Key, binding.Value);
            }
            return result;
        }

        // A utility function to query a DELP knowledgebase with the given assumption and validate it.
        public void Validate()
        {
            // Create lambda for event subscription
            assignDelpResponseLambda = (eventData) => AssignDELPResponse();
            EventManager.Instance.SubscribeToEvent(EventType.DelpResponse, assignDelpResponseLambda);

            DELPQuery query = new DELPQuery(this.ToString());
            DELPMessage msg = query.PrepareQuery();
            TCPTestClient.Instance.SendMessage<DELPMessage>(msg);
        }

        private void AssignDELPResponse()
        {
            Debug.Log("Received DELP response; assigning to assumption.");
            this.tmpResponse = (DELPResponse)EventManager.Instance.EventData;

            if (this.tmpResponse.msg == this.ToString())
            {
                if (this.tmpResponse.data.answer.Contains("YES"))
                {
                    this.isValid = Validity.YES;
                }
                else if (this.tmpResponse.data.answer.Contains("NO"))
                {
                    this.isValid = Validity.NO;
                }
                else if (this.tmpResponse.data.answer.Contains("UNDECIDED"))
                {
                    this.isValid = Validity.UNDECIDED;
                }
                else
                {
                    this.isValid = Validity.DEFAULT;
                }
                Debug.Log("Assumption validated: " + this.isValid.ToString());
            }
            else
            {
                Debug.Log("Assumption could not be validated; mismatching query.");
            }

            EventManager.Instance.UnsubscribeToEvent(EventType.DelpResponse, null);
        }

        public Validity IsValid
        {
            get { return this.isValid; }
        }
    }

    [System.Serializable]
    // A class that represents a binding between the name of a given supertask, and a list of names of the ABL behaviors associated with that supertask.
    public class SupertaskBindings
    {
        public string key;
        public List<string> val;
    }

    [System.Serializable]
    // A class that represents a binding between the name of an ABL behavior, and a list of assumptions.
    public class BehaviorBindings
    {
        public string key;
        public List<string> val;
    }

    [System.Serializable]
    // A class that represents a binding between the integer ID of a character, and the name of that character.
    public class CharacterBindings
    {
        public int key;
        public string val;
    }
}
