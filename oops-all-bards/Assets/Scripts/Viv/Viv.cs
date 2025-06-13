using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (CustomDictionary.Instance.SupertaskDict.ContainsKey(supertaskName))
            {
                int targetCharacterId = 0; // Assuming Player target for now

                // Create a new instance of the Supertask, passing the full 'character' object as the owner.
                Supertask newTask = new Supertask(supertaskName, character, targetCharacterId, CustomDictionary.Instance);
                return newTask;
            }
            else
            {
                Debug.LogError($"Supertask name '{supertaskName}' not found in Viv bindings!");
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
        // A reference to the VivCharacter that owns this supertask.
        [System.NonSerialized] private VivCharacter owner;
        // The integer ID of the character engaged in the supertask.
        [SerializeField] private int actingCharacter;
        // The integer ID of the character targeted by the supertask.
        [SerializeField] private int targetCharacter;
        // A boolean indicating whether or not the supertask is currently being carried out.
        [SerializeField] private bool inProgress = false;
        // The evaluation of the supertask in terms of its behaviors, containing the TFUs associated with each behavior.
        [SerializeField] private TFU[] evaluation;

        // -- STATE MANAGEMENT --
        public enum EvaluationState { Idle, Evaluating, ReadyToDispatch }
        private EvaluationState currentState = EvaluationState.Idle;
        private int pendingResponses = 0;

        public Supertask(string name, VivCharacter owner, int targetCharacter, CustomDictionary bindings)
        {
            this.name = name;
            this.owner = owner;
            this.actingCharacter = owner.characterID;
            this.targetCharacter = targetCharacter;
            this.behaviors = this.FormBehaviors(name, CustomDictionary.Instance);
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

            // Get the list of behavior names associated with this supertask
            List<string> behaviorNames = CustomDictionary.Instance.SupertaskDict[name];

            foreach (string bname in behaviorNames)
            {
                BehaviorData data = CustomDictionary.Instance.BehaviorDict[bname];
                Behavior toAdd = new Behavior(
                    bname,
                    this.actingCharacter,
                    this.targetCharacter,
                    this,         
                    data.incompatibleWith,
                    CustomDictionary.Instance
                );
                behaviors.Add(toAdd);
            }
            return behaviors;
        }

        // A function that dispatches all suitable behaviors for the VivCharacter to the ABL agent.
        public void SelectAndDispatchBehaviors()
        {
            if (currentState != EvaluationState.ReadyToDispatch) { return; }

            ScoreAllBehaviors();

            // Filter for all logically possible behaviors -- no falsities
            var possibleBehaviors = new List<Behavior>();
            for (int i = 0; i < behaviors.Count; i++)
            {
                if (evaluation[i].Falsities == 0)
                {
                    possibleBehaviors.Add(behaviors[i]);
                }
            }

            if (possibleBehaviors.Count == 0)
            {
                Debug.Log("No valid behaviors (all had false assumptions).");
                currentState = EvaluationState.Idle;
                return;
            }

            // Prioritize the list based on certainty and personality -- first, by highest number of truths (most certain), then by priority (lowest number)
            var prioritizedList = possibleBehaviors
                .OrderByDescending(b => ScoreBehavior(b).Truths)
                .ThenBy(b => owner.persona.GetPriorityFor(b.Name))
                .ToList();

            // Filter for compatibility to build the final dispatch list -- some things cannot be done at the same time
            var dispatchList = new List<Behavior>();
            foreach (Behavior candidate in prioritizedList)
            {
                bool isCompatible = true;
                foreach (Behavior selected in dispatchList)
                {
                    if (selected.incompatibleWith.Contains(candidate.Name) ||
                        candidate.incompatibleWith.Contains(selected.Name))
                    {
                        isCompatible = false;
                        break;
                    }
                }

                if (isCompatible)
                {
                    dispatchList.Add(candidate);
                }
            }

            // Dispatch the final set of behaviors
            if (dispatchList.Count > 0)
            {
                Debug.Log($"<color=magenta>Viv has chosen {dispatchList.Count} compatible behavior(s) to dispatch.</color>");
                foreach (var b in dispatchList) Debug.Log($" - '{b.Name}' (Priority: {owner.persona.GetPriorityFor(b.Name)}, Truths: {ScoreBehavior(b).Truths})");

                var behaviorNames = dispatchList.Select(b => b.Name).ToList();
                this.owner.UpdateActiveBehaviors(behaviorNames);

                VivWME wme = new VivWME(this.actingCharacter);
                List<SpawnGoalData> goalsToSpawn = new List<SpawnGoalData>();
                foreach (Behavior validBehavior in dispatchList)
                {
                    goalsToSpawn.Add(new SpawnGoalData
                    {
                        name = validBehavior.Name,
                        actingCharacter = this.actingCharacter,
                        targetCharacter = this.targetCharacter
                    });
                }
                wme.ToSpawn = goalsToSpawn.ToArray();

                ABLMessage msg = wme.ToABLMessage();
                TCPTestClient.Instance.SendMessage<ABLMessage>(msg);
            }
            else
            {
                // If no behaviors are dispatched, we can clear the list.
                this.owner.ActiveBehaviorNames.Clear();
                Debug.Log("<color=magenta>Viv has determined that there are no valid behaviors to dispatch at this time.");
            }

            currentState = EvaluationState.Idle;
        }

        // A function that evaluates the given supertask with respect to its component behaviors.
        public void BeginEvaluation()
        {
            // Don't start a new evaluation if one is already in progress.
            if (currentState == EvaluationState.Evaluating) return;

            Debug.Log($"<color=yellow>Beginning evaluation for Supertask '{this.Name}'...</color>");
            currentState = EvaluationState.Evaluating;

            // Count how many assumptions we need answers for.
            pendingResponses = 0;
            foreach (var behavior in behaviors)
            {
                pendingResponses += behavior.Assumptions.Count;
            }

            if (pendingResponses == 0)
            {
                currentState = EvaluationState.ReadyToDispatch;
                return;
            }

            // Tell all assumptions to validate themselves.
            foreach (var behavior in behaviors)
            {
                foreach (var assumption in behavior.Assumptions)
                {
                    assumption.Validate();
                }
            }
        }

        // A utility function used to count the number of truths (YES), falsities (NO), and uncertainties (UNDECIDED) belonging to each behavior.
        private TFU ScoreBehavior(Behavior behavior)
        {
            TFU score = new TFU();

            // Loop directly through the assumptions
            foreach (Assumption assumption in behavior.Assumptions)
            {
                switch (assumption.IsValid)
                {
                    case Assumption.Validity.YES:
                        score.Truths++;
                        break;
                    case Assumption.Validity.NO:
                        score.Falsities++;
                        break;

                    // Both UNDECIDED and the DEFAULT state count as uncertainties.
                    case Assumption.Validity.UNDECIDED:
                    case Assumption.Validity.DEFAULT:
                        score.Uncertainties++;
                        break;
                }
            }

            return score;
        }

        // A utility function to score all behaviors in the supertask and return a TFU array representing the evaluation.
        public void ScoreAllBehaviors()
        {
            if (this.evaluation == null)
            {
                this.evaluation = new TFU[this.behaviors.Count];
            }

            for (int i = 0; i < behaviors.Count; i++)
            {
                evaluation[i] = ScoreBehavior(this.behaviors[i]);
            }
        }

        public void OnAssumptionValidated()
        {
            pendingResponses--;
            Debug.Log($"Response received. {pendingResponses} responses still pending.");

            // If all responses have been received, we are ready to make a decision.
            if (pendingResponses <= 0)
            {
                Debug.Log($"<color=yellow>All responses received. Supertask '{this.Name}' is ready to dispatch.</color>");
                currentState = EvaluationState.ReadyToDispatch;

                // Now that we are ready, we can immediately try to dispatch.
                SelectAndDispatchBehaviors();
            }
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
        // The parent supertask of the behavior.
        [SerializeField] private Supertask parentSupertask;
        [Tooltip("A list of other Behavior names that this behavior cannot run at the same time as.")]
        public List<string> incompatibleWith;

        public Behavior(string name, int actingCharacter, int targetCharacter, Supertask parent, List<string> incompatibleWith, CustomDictionary bindings)
        {
            this.name = name;
            this.actingCharacter = actingCharacter;
            this.parentSupertask = parent;
            this.incompatibleWith = incompatibleWith ?? new List<string>();
            this.assumptions = this.FormAssumptions(name, actingCharacter, targetCharacter, CustomDictionary.Instance);
        }

        private List<Assumption> FormAssumptions(string name, int actingCharacter, int targetCharacter, CustomDictionary bindings)
        {
            List<Assumption> assumptions = new List<Assumption>();

            // Get the assumption templates from the bindings
            List<string> assumptionTemplates = CustomDictionary.Instance.BehaviorDict[name].assumptionTemplates;

            // Create the role bindings dict
            var roleBindings = new Dictionary<string, string>();

            // Get the acting character's name for the {self} role
            VivCharacter selfChar = Viv.Instance.FindVivCharacter(actingCharacter);
            if (selfChar != null)
            {
                // e.g., "{self}" -> "Wurguth"
                roleBindings.Add("{self}", selfChar.characterName.ToLower());
            }

            // Get the target character's name for the {target} role
            // TODO: This lookup should probably use a universal registry
            string targetCharacterName = "player"; // Default placeholder
            VivCharacter targetChar = Viv.Instance.FindVivCharacter(targetCharacter);
            if (targetChar != null)
            {
                targetCharacterName = targetChar.characterName.ToLower();
            }
            // e.g., "{target}" -> "player"
            roleBindings.Add("{target}", targetCharacterName);

            // TODO: Add other roles here in the future, like {eventActor}, {location}, etc.

            // Create an Assumption for each template, passing the bindings to resolve it
            foreach (string template in assumptionTemplates)
            {
                Assumption toAdd = new Assumption(template, actingCharacter, this.parentSupertask, roleBindings);
                assumptions.Add(toAdd);
            }

            return assumptions;
        }

        public string Name
        {
            get { return this.name; }
        }

        public List<Assumption> Assumptions
        {
            get { return this.assumptions; }
        }
    }

    [System.Serializable]
    public class Assumption
    {
        // The parent supertask of the assumption.
        public Supertask parentSupertask { get; set; }
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
        private Action<object> onDelpResponse;

        public Assumption(string template, int actingCharacter, Supertask parent, Dictionary<string, string> roleBindings)
        {
            this.template = template;
            this.actingCharacter = actingCharacter;
            this.isValid = Validity.DEFAULT;
            this.parentSupertask = parent;

            // Resolve the template immediately upon creation
            this.resolvedQuery = ResolveTemplate(template, roleBindings);

            this.onDelpResponse = (eventData) => { AssignDELPResponse(eventData); };
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
            EventManager.Instance.SubscribeToEvent(EventType.DelpResponse, onDelpResponse);

            DELPQuery query = new DELPQuery(this.ToString());
            DELPMessage msg = query.PrepareQuery();
            TCPTestClient.Instance.SendMessage<DELPMessage>(msg);
        }

        private void AssignDELPResponse(object eventData)
        {
            // Cast the generic event data to the specific type we expect.
            DELPResponse response = eventData as DELPResponse;
            if (response == null) return;

            // Check if this response is the one we are waiting for.
            if (response.msg == this.resolvedQuery)
            {
                Debug.Log($"<color=cyan>Assumption '{this.resolvedQuery}' received a matching response.</color>");

                if (response.data.answer.Contains("YES")) this.isValid = Validity.YES;
                else if (response.data.answer.Contains("NO")) this.isValid = Validity.NO;
                else if (response.data.answer.Contains("UNDECIDED")) this.isValid = Validity.UNDECIDED;
                else this.isValid = Validity.DEFAULT;

                Debug.Log($"Assumption validated: {this.isValid}");

                EventManager.Instance.UnsubscribeToEvent(EventType.DelpResponse, onDelpResponse);

                parentSupertask.OnAssumptionValidated();
            }
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
        public BehaviorData val;
    }

    [System.Serializable]
    public class BehaviorData
    {
        [Tooltip("A list of other Behavior names that this behavior cannot run at the same time as.")]
        public List<string> incompatibleWith;

        [Tooltip("The list of assumption templates required for this behavior to be valid.")]
        public List<string> assumptionTemplates;
    }

    [System.Serializable]
    // A class that represents a binding between the integer ID of a character, and the name of that character.
    public class CharacterBindings
    {
        public int key;
        public string val;
    }
}
