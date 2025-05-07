using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    public static DialogueManager Instance => _instance;

    [SerializeField] public JSONReader jsonReader;
    [SerializeField] public GameObject dialogueUI;
    [SerializeField] public GameObject nodeContentOrganizer;
    [SerializeField] public TMP_Text nodeText;
    [SerializeField] public Image portrait;
    [SerializeField] public TMP_Text speakerName;
    [SerializeField] public GameObject nodeResponsePrefab;
    [SerializeField] public GameObject textBubblePrefab;
    [SerializeField] public GameObject questUI;
    public bool isInDialogue = false;
    public delegate void DialogueStateChangedEventHandler(bool isInDialogue);
    public event DialogueStateChangedEventHandler OnDialogueStateChanged;

    private int nodeIndex;
    private int dialogueIndex;
    private GameObject currentSpeaker_eventActor; // often player
    private GameObject currentListener_eventTarget; // the one being interacted with

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetDialogueEvents();
    }

    void ResetDialogueEvents()
    {
        Debug.Log("Resetting dialogue events.");
        DialogueInteractable[] interactables = FindObjectsOfType<DialogueInteractable>();

        foreach (DialogueInteractable interactable in interactables)
        {
            foreach (DialogueEvent dialogueEvent in interactable.dialogueEvents)
            {
                dialogueEvent.exhausted = false;
            }
        }
    }

    public void StartDialogue(int dialogueID)
    {
        if (jsonReader == null || jsonReader.dialogues == null)
        {
            Debug.LogError("DialogueManager: JSONReader or its dialogues data is not initialized!");
            CloseDialogue(); // Attempt to clean up
            return;
        }

        Dialogue dialogue = jsonReader.dialogues.GetDialogue(dialogueID);
        if (dialogue == null)
        {
            Debug.LogError($"DialogueManager: Dialogue with ID {dialogueID} not found.");
            CloseDialogue();
            return;
        }

        currentListener_eventTarget = GameObject.FindGameObjectWithTag("Player");
        if (currentListener_eventTarget == null)
        {
            Debug.LogError("DialogueManager: Player GameObject (tagged 'Player') not found. Cannot set as listener for event triggering.");
        }

        string npcNameFromDialogue = dialogue.SpeakerName;
        currentSpeaker_eventActor = GameObject.Find(npcNameFromDialogue);
        if (currentSpeaker_eventActor == null)
        {
            Debug.LogError($"DialogueManager: NPC GameObject named '{npcNameFromDialogue}' (from dialogue.SpeakerName) not found. Cannot set as speaker for event triggering.");
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        GameObject npcObject = GameObject.Find(dialogue.SpeakerName); // This is the NPC whose dialogue it is

        if (playerObject != null && npcObject != null)
        {
            // Standard scenario: Player (actor) interacts with NPC (target)
            currentSpeaker_eventActor = playerObject;
            currentListener_eventTarget = npcObject;
        }
        else
        {
            Debug.LogWarning("DialogueManager: Could not reliably set event actor/target. Player or NPC not found.");
            currentSpeaker_eventActor = null; // Ensure they are null if setup failed
            currentListener_eventTarget = null;
        }

        Cursor.lockState = CursorLockMode.Confined;
        nodeIndex = 0;
        if (GameManager.Instance != null) GameManager.Instance.TogglePlayerControls();
        dialogueIndex = dialogueID;

        RenderDialogueUI(dialogue); // This sets the UI speaker name from dialogue.SpeakerName
        isInDialogue = true;
        OnDialogueStateChanged?.Invoke(isInDialogue);
    }

    private void RenderDialogueUI(Dialogue dialogue)
    {
        speakerName.text = dialogue.SpeakerName;
        portrait.sprite = Resources.Load<Sprite>($"Portraits/{dialogue.SpeakerName}");
        // Handle missing portrait scenario here (e.g., use a default portrait)

        DialogueNode currentNode = dialogue.DialogueNodes[nodeIndex];
        RenderCurrentNode(currentNode);
        dialogueUI.SetActive(true);
        dialogueUI.GetComponent<Animator>().Play("dialogueBox");
    }

    public void ToggleDialogueUI()
    {
        dialogueUI.SetActive(!dialogueUI.activeSelf);
        dialogueUI.GetComponent<Animator>().Play("dialogueBox");
    }

    private void RenderCurrentNode(DialogueNode node)
    {
        ClearNodeResponses();
        nodeText.text = node.NodeText;

        if (node.NodeResponses == null)
        {
            Debug.LogWarning($"Dialogue Node {node.ID} (Text: \"{node.NodeText}\") has no responses.");
            return;
        }

        foreach (NodeResponse response in node.NodeResponses)
        {
            GameObject responseObj = Instantiate(nodeResponsePrefab, nodeContentOrganizer.transform);
            Button responseButton = responseObj.GetComponent<Button>();
            TMP_Text responseText = responseObj.GetComponentInChildren<TMP_Text>();
            DialogueHighlight highlight = responseObj.GetComponent<DialogueHighlight>() ?? responseObj.AddComponent<DialogueHighlight>(); // Get or Add

            NodeResponse currentResponseData = response;

            string displayText = currentResponseData.NodeResponseText;
            bool skillCheckDefined = !string.IsNullOrEmpty(currentResponseData.SkillCheck) && currentResponseData.SkillCheckTarget > 0;
            DialogueHighlight.DialogueHighlightType currentHighlightType = DialogueHighlight.DialogueHighlightType.Default;

            if (skillCheckDefined)
            {
                displayText += $" ({currentResponseData.SkillCheck} {currentResponseData.SkillCheckTarget})";
                bool skillCheckPassed = true; // Assume pass until checked
                BasePlayer player = PartyManager.Instance?.FindPartyMemberById(0);
                if (player != null && player.PlayerClass != null)
                {
                    var stat = player.PlayerClass.GetBaseStatByName(currentResponseData.SkillCheck);
                    if (stat != null)
                    {
                        skillCheckPassed = stat.ModifiedValue >= currentResponseData.SkillCheckTarget;
                    }
                    else
                    {
                        Debug.LogWarning($"Skill '{currentResponseData.SkillCheck}' not found for player.");
                        skillCheckPassed = false;
                    }
                }
                else
                {
                    Debug.LogError("Player data for skill check not found!");
                    skillCheckPassed = false;
                }

                currentHighlightType = skillCheckPassed ?
                    DialogueHighlight.DialogueHighlightType.PassedSkillCheck :
                    DialogueHighlight.DialogueHighlightType.FailedSkillCheck;
            }
            // Else, currentHighlightType remains DialogueHighlightType.Default

            responseText.text = displayText;
            highlight.InitializeHighlight(currentHighlightType);

            // --- onClick Listener ---
            responseButton.onClick.AddListener(() =>
            {
                // Optional: Check if the button is actually interactable before processing
                // This check is somewhat redundant if DialogueHighlight correctly disables it,
                // but can be a failsafe.
                if (!responseButton.interactable)
                {
                    Debug.Log($"Clicked a non-interactable (failed skill check) response: '{currentResponseData.NodeResponseText}'");
                    return; // Don't process click for non-interactable buttons
                }

                // Re-evaluate skill check pass/fail status for outcome determination
                // (or use the 'currentHighlightType' if confident it won't change)
                bool finalSkillCheckResult = true;
                if (skillCheckDefined)
                {
                    BasePlayer player = PartyManager.Instance?.FindPartyMemberById(0);
                    if (player != null && player.PlayerClass != null)
                    {
                        var stat = player.PlayerClass.GetBaseStatByName(currentResponseData.SkillCheck);
                        if (stat != null) finalSkillCheckResult = stat.ModifiedValue >= currentResponseData.SkillCheckTarget;
                        else finalSkillCheckResult = false;
                    }
                    else finalSkillCheckResult = false;
                }

                // Trigger Interaction Event if configured
                if (currentResponseData.triggersInteraction && !string.IsNullOrEmpty(currentResponseData.interactionType))
                {
                    string outcomeToTrigger = finalSkillCheckResult ? currentResponseData.outcomeOnPass : currentResponseData.outcomeOnFail;
                    if (!string.IsNullOrEmpty(outcomeToTrigger))
                    {
                        if (currentSpeaker_eventActor != null && currentListener_eventTarget != null)
                        {
                            EventManager.Instance.TriggerInteraction(
                                currentSpeaker_eventActor,
                                currentListener_eventTarget,
                                currentResponseData.interactionType,
                                outcomeToTrigger
                            );
                        }
                        else { /* Error Log */ }
                    }
                }

                NextNode(currentResponseData.NextNode);
                if (!string.IsNullOrEmpty(currentResponseData.Then)) Invoke(currentResponseData.Then, 0);
            });
        }
    }

    private void ClearNodeResponses()
    {
        foreach (Transform child in nodeContentOrganizer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void NextNode(int index)
    {
        if (index == -1)
        {
            CloseDialogue();
        }
        else if (index == -2)
        {
            CloseDialogue();
            DemoManager.Instance.RecruitQuinton();
            GameObject.Find("Quinton").GetComponent<NPCMovement>().SendQuintonToBackroom();
        }
        else if (index == -999)
        {
            CloseDialogue();
            DemoManager.Instance.LoadScene("GigDemo");
        }
        else
        {
            nodeIndex = index;
            DialogueNode currentNode = jsonReader.dialogues.GetDialogue(dialogueIndex).DialogueNodes[nodeIndex];
            RenderCurrentNode(currentNode);
        }
    }

    private void CloseDialogue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        dialogueUI.GetComponent<Animator>().Play("dialogueBoxClose");
        GameManager.Instance.TogglePlayerControls();
        isInDialogue = false;
        OnDialogueStateChanged?.Invoke(isInDialogue);
    }

    public void SpawnTextBubble(GameObject character, string text)
    {
        GameObject target = character.transform.Find("CameraTarget").gameObject;
        GameObject textBubble = Instantiate(textBubblePrefab, target.transform.position + Vector3.up * 0.5f, Quaternion.identity, target.transform);

        // Make the text bubble face the camera
        if (Camera.main != null)
        {
            textBubble.transform.LookAt(Camera.main.transform);
            textBubble.transform.Rotate(Vector3.up * 180f);

            // Optionally, if you want the text bubble to be perfectly upright
            // even if the camera is looking from above or below, uncomment this:
            textBubble.transform.rotation = Quaternion.Euler(0, textBubble.transform.eulerAngles.y, 0);
        }
        else
        {
            Debug.LogWarning("Main Camera not found. Text bubble may not face the screen correctly.");
        }

        textBubble.GetComponentInChildren<TMP_Text>().text = text;
    }

    public void QuintonDialogueTrigger()
    {
        DemoManager.Instance.CreateSignpostMessage(DemoManager.help4);
        DemoManager.Instance.CreateSignpostMessage(DemoManager.help3);
    }

    public void TriggerAssistanceQuip(int actingCharacter)
    {
        if (!PartyManager.Instance.inCombat)
        {
            return;
        }

        Quip assistanceQuip = jsonReader.quips.quips[4];
        GameObject model = CombatManager.Instance.GetModelByID(actingCharacter);
        SpawnTextBubble(model, assistanceQuip.Text);
    }

    public void FinalHelpTrigger()
    {
        DemoManager.Instance.CreateSignpostMessage(DemoManager.help9);
        DemoManager.Instance.completedDemo = true;
    }

    public void ChooseAppropriateQuip(int actingCharacter, bool inCombat)
    {
        Quip chosenQuip;
        List<Quip> relevantQuips = new List<Quip>();
        GameObject model;
        BasePlayer ac = PartyManager.Instance.FindPartyMemberById(actingCharacter);

        List<Quip> specificQuips = new List<Quip>();
        List<Quip> genericQuips = new List<Quip>();
        IEnumerable<Quip> result;

        if (inCombat && (inCombat == PartyManager.Instance.inCombat))
        {
            Debug.Log("We're in combat.");
            foreach (CombatStatus cs in ac.CombatStatuses)
            {
                result = specificQuips.Concat(jsonReader.quips.GetQuipsByCombatStatus(cs.Type));
                foreach (Quip q in result)
                {
                    relevantQuips.Add(q);
                }
            }
            if (relevantQuips.Count == 0)
            {
                result = genericQuips.Concat(jsonReader.quips.GetGenericCombatQuips());
                foreach (Quip q in result)
                {
                    relevantQuips.Add(q);
                }
            }
            model = CombatManager.Instance.GetModelByID(actingCharacter);
        }
        else if (!inCombat && (inCombat == PartyManager.Instance.inCombat))
        {
            Debug.Log("We're not in combat.");
            foreach (Status s in ac.CiFData.Statuses)
            {
                result = specificQuips.Concat(jsonReader.quips.GetQuipsByCiFStatus(s.Type));
                foreach (Quip q in result)
                {
                    relevantQuips.Add(q);
                }
            }
            if (relevantQuips.Count == 0)
            {
                result = genericQuips.Concat(jsonReader.quips.GetGenericNoncombatQuips());
                foreach (Quip q in result)
                {
                    relevantQuips.Add(q);
                }
            }
            model = PartyManager.Instance.GetModelByID(actingCharacter);
        }
        else
        {
            return;
        }

        Debug.Log(relevantQuips.Count);
        chosenQuip = relevantQuips[Random.Range(0, relevantQuips.Count - 1)];
        SpawnTextBubble(model, chosenQuip.Text);
    }
}