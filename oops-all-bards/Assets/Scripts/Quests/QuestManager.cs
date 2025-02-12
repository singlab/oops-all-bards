using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    private static QuestManager _instance;
    public static QuestManager Instance => _instance;

    public JSONReader jsonReader;
    public GameObject questUI;
    public GameObject questPrefab;

    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();
    private GameObject currentQuestMarker;
    private int currentQuestIndex = 0; // Index in the activeQuests list
    
    // Dictionary to store the mapping between quest stages and their associated DialogueEvents
    private Dictionary<int, Dictionary<int, DialogueEvent>> questStageToDialogueEventMap = new Dictionary<int, Dictionary<int, DialogueEvent>>();

    // Dictionary to store instantiated quest start markers, key is quest ID, value is the GameObject
    private Dictionary<int, GameObject> questStartMarkers = new Dictionary<int, GameObject>();
    

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Important to prevent further execution in this case
        }
    }

    void Start()
    {
        InitializeQuestStageToDialogueEventMap();
        AssignQuestUIToManager(); // Call this here to ensure questUI is assigned early
        // AcceptQuest(0); // Accept the first quest
        UpdateQuestUI();
        UpdateQuestStartMarkers();
    }

    void Update()
    {
        if (questUI == null)
        {
            AssignQuestUIToManager();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleCurrentQuest();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
        DialogueManager.Instance.OnDialogueStateChanged += HandleDialogueStateChanged;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent memory leaks
        DialogueManager.Instance.OnDialogueStateChanged -= HandleDialogueStateChanged;
    }

    private void HandleDialogueStateChanged(bool isInDialogue)
    {
        questUI.SetActive(!isInDialogue); 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "GigDemo")
        {
            AssignQuestUIToManager(); // Re-assign questUI in case it's different in the new scene
            UpdateQuestUI(); // Update the UI
            InitializeQuestStageToDialogueEventMap(); // Re-initialize the map
            UpdateQuestStartMarkers(); // Re-instantiate quest start markers
            UpdateQuestMarker(activeQuests[currentQuestIndex]); // Re-instantiate the quest marker
        }
    }

    public void AcceptQuest(int questId)
    {
        Quest questToAccept = jsonReader.quests.GetQuest(questId);

        if (questToAccept != null)
        {
            activeQuests.Add(questToAccept);

            // Set current quest to the accepted quest
            currentQuestIndex = activeQuests.IndexOf(questToAccept);
            UpdateQuestUI();
        }
        else
        {
            Debug.LogWarning("No quest found with ID: " + questId);
        }
    }

    private void CycleCurrentQuest()
    {
        if (activeQuests.Count == 0)
        {
            return; // No quests to cycle through
        }

        currentQuestIndex++;
        if (currentQuestIndex >= activeQuests.Count)
        {
            currentQuestIndex = 0; // Wrap around to the first quest
        }
        UpdateQuestMarker(activeQuests[currentQuestIndex]);
        UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        ClearQuestUI();

        if (activeQuests.Count == 0) return;

        Quest currentQuest = activeQuests[currentQuestIndex];
        if (currentQuest == null) return;

        GameObject questInstance = Instantiate(questPrefab, questUI.transform, false);

        questInstance.transform.Find("QuestName").GetComponent<TMP_Text>().text = currentQuest.Name;
        TMP_Text questStageText = questInstance.transform.Find("QuestStageText").GetComponent<TMP_Text>();

        int currentStageIndex = currentQuest.CurrentStageIndex;

        if (currentQuest.Linear)
        {
            questStageText.text = currentQuest.Stages[currentStageIndex].DisplayText;
        }
        else
        {
            // Find the minimum parallel stage
            int minParallelStage = currentQuest.ParallelStages.Count > 0 ? currentQuest.ParallelStages.Min() : -1;

            if (currentStageIndex == minParallelStage) // Only display parallel stages if the current stage is the minimum
            {
                if (!currentQuest.Stages[currentStageIndex].Complete)
                {
                    questStageText.text = currentQuest.Stages[currentStageIndex].DisplayText;
                }

                foreach (int stageIndex in currentQuest.ParallelStages)
                {
                    if (stageIndex != currentStageIndex && !currentQuest.Stages[stageIndex].Complete)
                    {
                        questStageText.text += (questStageText.text != "" ? "\n\n" : "") + currentQuest.Stages[stageIndex].DisplayText;
                    }
                }
            } else {
                questStageText.text = currentQuest.Stages[currentStageIndex].DisplayText;
            }
        }

        UpdateQuestMarker(currentQuest);
    }

    private void UpdateQuestMarker(Quest quest)
    {
        // Destroy the old marker if it exists
        if (currentQuestMarker != null)
        {
            Destroy(currentQuestMarker);
            currentQuestMarker = null; // Important: Reset the reference
        }

        // Get the current DialogueEvent associated with the current quest stage
        DialogueEvent currentDialogueEvent = GetCurrentDialogueEventForQuestStage(quest); 

        if (currentDialogueEvent != null && currentDialogueEvent.dialogueBubblePrefab != null)
        {
            // Use the DialogueEvent's prefab for the marker
            currentQuestMarker = Instantiate(currentDialogueEvent.dialogueBubblePrefab, GetNPCMarkerPosition(quest) + new Vector3(0f,0.5f,0f), Quaternion.identity);                                 
        }
    }

    public void UpdateQuestStartMarkers()
    {
        // Iterate through all DialogueInteractable objects in the scene
        DialogueInteractable[] interactables = FindObjectsOfType<DialogueInteractable>();

        foreach (DialogueInteractable interactable in interactables)
        {
            foreach (DialogueEvent dialogueEvent in interactable.dialogueEvents)
            {
                if (dialogueEvent.eventType == DialogueEvent.DialogueEventType.QuestStart)
                {
                    if (!dialogueEvent.exhausted && interactable.CheckConditions(dialogueEvent.checkCondition)) // Check the condition
                    {
                        GameObject toInstantiate = Instantiate(dialogueEvent.dialogueBubblePrefab, (GetNPCMarkerPosition(jsonReader.quests.GetQuest(dialogueEvent.questID)) + new Vector3(0f,0.5f,0f)), Quaternion.identity);
                        questStartMarkers[dialogueEvent.questID] = toInstantiate;
                    }
                }
            }
        }
    }

    public void DestroyQuestStartMarker(int questID)
    {
        Debug.Log("Destroying quest start marker for quest ID: " + questID);
        if (questStartMarkers.ContainsKey(questID))
        {
            Debug.Log("Entry found in questStartMarkers dictionary.");
            Destroy(questStartMarkers[questID]);
            questStartMarkers.Remove(questID);
        }
    }

    private DialogueEvent GetCurrentDialogueEventForQuestStage(Quest quest)
    {
        if (questStageToDialogueEventMap.ContainsKey(quest.ID)) 
        {
            Dictionary<int, DialogueEvent> stageToEventMap = questStageToDialogueEventMap[quest.ID];
            if (stageToEventMap.ContainsKey(quest.CurrentStageIndex)) 
            {
                return stageToEventMap[quest.CurrentStageIndex];
            }
        }

        return null; 
    }

    private Vector3 GetNPCMarkerPosition(Quest quest)
    {
        string npcTargetName = quest.Stages[quest.CurrentStageIndex].NPCTargetName;
        if (npcTargetName == "None")
        {
            return Vector3.zero; // Or handle this case appropriately
        }

        GameObject model = GameObject.Find(npcTargetName);
        if (model == null)
        {
            Debug.LogWarning($"NPC with name '{npcTargetName}' not found!");
            return Vector3.zero; 
        }

        Transform target = model.transform.Find("CameraTarget");
        if (target == null)
        {
            Debug.LogWarning($"CameraTarget not found on NPC '{npcTargetName}'!");
            return Vector3.zero; 
        }

        return target.position;
    }

    private void InitializeQuestStageToDialogueEventMap()
    {
        // Iterate through all DialogueInteractable objects in the scene
        DialogueInteractable[] interactables = FindObjectsOfType<DialogueInteractable>();

        foreach (DialogueInteractable interactable in interactables)
        {
            foreach (DialogueEvent dialogueEvent in interactable.dialogueEvents)
            {
                if (dialogueEvent.questID != -1 && dialogueEvent.questStageID != -1)
                {
                    if (!questStageToDialogueEventMap.ContainsKey(dialogueEvent.questID))
                    {
                        questStageToDialogueEventMap[dialogueEvent.questID] = new Dictionary<int, DialogueEvent>();
                    }

                    questStageToDialogueEventMap[dialogueEvent.questID][dialogueEvent.questStageID] = dialogueEvent;
                }
            }
        }
    }

    public void MarkStageComplete()
    {
        Quest currentQuest = activeQuests[currentQuestIndex];
        int currentStageIndex = currentQuest.CurrentStageIndex;

        currentQuest.Stages[currentStageIndex].Complete = true;


        if (!currentQuest.Linear && currentQuest.ParallelStages.Contains(currentStageIndex))
        {
            bool allParallelComplete = true;
            foreach (int stageIndex in currentQuest.ParallelStages)
            {
                if (!currentQuest.Stages[stageIndex].Complete)
                {
                    allParallelComplete = false;
                    break;
                }
            }

            if (!allParallelComplete)
            {
                UpdateQuestUI();
                return; // Don't advance if not all parallel stages are done
            }
        }

        // Destroy the marker when the stage is complete
        if (currentQuestMarker != null)
        {
            Destroy(currentQuestMarker);
            currentQuestMarker = null;
        }

        currentQuest.CurrentStageIndex++;

        if (currentQuest.CurrentStageIndex >= currentQuest.Stages.Count)
        {
            MarkQuestComplete();
            UpdateQuestStartMarkers();
        }
        else
        {
            UpdateQuestUI();
        }
    }

    public void MarkQuestComplete()
    {
        Quest currentQuest = activeQuests[currentQuestIndex];
        currentQuest.Complete = true;
        activeQuests.RemoveAt(currentQuestIndex); // Remove the completed quest
        completedQuests.Add(currentQuest); // Add it to the completed quests list

        if (activeQuests.Count > 0) {
            currentQuestIndex = 0; // Reset to the first active quest (or 0 if none left)
            UpdateQuestUI();
        } else {
          ClearQuestUI();
        }
    }

    private void ClearQuestUI()
    {
        if (questUI == null) return;
        foreach (Transform child in questUI.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void AssignQuestUIToManager()
    {
        questUI = GameObject.Find("QuestUI");
        if (questUI == null)
        {
            // Debug.LogWarning("QuestUI not found in the scene!");
        }
    }

    public int GetCurrentQuestID()
    {
        if (activeQuests.Count > 0)
        {
            return activeQuests[currentQuestIndex].ID; 
        }
        else
        {
            return -1; // Or another appropriate value to indicate no active quest
        }
    }

    public bool IsQuestCompleted(int questID)
    {
        foreach (Quest quest in completedQuests)
        {
            if (quest.ID == questID && quest.Complete)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsPreviousStageCompleted(int questID)
    {
        Quest currentQuest = activeQuests[currentQuestIndex];
        if (currentQuest == null || currentQuest.CurrentStageIndex == 0)
        {
            return false; // Quest not found or first stage
        }

        return currentQuest.Stages[currentQuest.CurrentStageIndex - 1].Complete; 
    }
}

[System.Serializable]
public class Quests
{
    [SerializeField] public List<Quest> quests = new List<Quest>();

    public Quest GetQuest(int id)
    {
        foreach (Quest q in quests)
        {
            if (q.ID == id)
            {
                return q;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Quest
{
    [SerializeField] private string name;
    [SerializeField] private int id;
    [SerializeField] private List<QuestStage> stages;
    [SerializeField] private bool complete;
    [SerializeField] private bool linear;
    [SerializeField] private List<int> parallelStages;
    [SerializeField] private int currentStageIndex = 0;

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public List<QuestStage> Stages
    {
        get { return this.stages; }
        set { this.stages = value; }
    }

    public bool Complete
    {
        get { return this.complete; }
        set { this.complete = value; }
    }

    public bool Linear
    {
        get { return this.linear; }
        set { this.linear = value; }
    }

    public List<int> ParallelStages
    {
        get { return this.parallelStages;}
        set { this.parallelStages = value;}
    }

    public int CurrentStageIndex
    {
        get { return this.currentStageIndex; }
        set { this.currentStageIndex = value; }
    }
}

[System.Serializable]
public class QuestStage
{
    // [SerializeField] private bool hasNPCTarget;
    [SerializeField] private int id;
    [SerializeField] private string npcTargetName;
    [SerializeField] private bool complete;
    [SerializeField] private string displayText;

    public int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public string NPCTargetName
    {
        get { return this.npcTargetName; }
        set { this.npcTargetName = value; }
    }

    public bool Complete
    {
        get { return this.complete; }
        set { this.complete = value; }
    }

    public string DisplayText
    {
        get { return this.displayText; }
        set { this.displayText = value; }
    }
}
