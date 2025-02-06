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
    public GameObject questMarkerPrefab;

    public List<Quest> activeQuests = new List<Quest>();
    private GameObject currentQuestMarker; // Store the current marker
    private int currentQuestIndex = 0; // Index in the activeQuests list

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
        AssignQuestUIToManager(); // Call this here to ensure questUI is assigned early
        AcceptQuest(jsonReader.quests.quests[0]); // Accept the first quest
        AcceptQuest(jsonReader.quests.quests[1]);
        UpdateQuestUI();
    }

    void Update()
    {
        if (questUI == null)
        {
            AssignQuestUIToManager();
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
        }
    }

    public void AcceptQuest(Quest quest)
    {
        activeQuests.Add(quest);
        if (activeQuests.Count == 1) { // Only update if this is the first quest
          UpdateQuestUI();
        }
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

        string npcTargetName = quest.Stages[quest.CurrentStageIndex].NPCTargetName;
        if (npcTargetName == "None")
        {
            return;
        }

        GameObject model = GameObject.Find(npcTargetName);
        if (model == null)
        {
            Debug.LogWarning($"NPC with name '{npcTargetName}' not found!");
            return;
        }

        Transform target = model.transform.Find("CameraTarget");
        if (target == null)
        {
            Debug.LogWarning($"CameraTarget not found on NPC '{npcTargetName}'!");
            return;
        }

        currentQuestMarker = Instantiate(questMarkerPrefab, target.position, Quaternion.identity, target); // Store the new marker
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

        if (activeQuests.Count > 0) {
            currentQuestIndex = 0; // Reset to the first active quest (or 0 if none left)
            UpdateQuestUI();
        } else {
          ClearQuestUI();
        }
    }

    private void ClearQuestUI()
    {
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
            Debug.LogWarning("QuestUI not found in the scene!");
        }
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
