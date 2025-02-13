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
        Cursor.lockState = CursorLockMode.Confined;
        nodeIndex = 0;
        GameManager.Instance.TogglePlayerControls();
        dialogueIndex = dialogueID;

        Dialogue dialogue = jsonReader.dialogues.GetDialogue(dialogueID);
        RenderDialogueUI(dialogue);
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

        foreach (NodeResponse response in node.NodeResponses) 
        {
            GameObject responseObj = Instantiate(nodeResponsePrefab, nodeContentOrganizer.transform);
            responseObj.GetComponentInChildren<TMP_Text>().text = response.NodeResponseText;
            
            if (response.SkillCheck != null)
            {
                Debug.Log("Skill check detected: " + response.SkillCheck + " with target " + response.SkillCheckTarget);
                Debug.Log("Player's skill value: " + PartyManager.Instance.FindPartyMemberById(0).PlayerClass.GetBaseStatByName(response.SkillCheck).ModifiedValue);

                if (response.SkillCheckTarget <= PartyManager.Instance.FindPartyMemberById(0).PlayerClass.GetBaseStatByName(response.SkillCheck).ModifiedValue)
                {
                    responseObj.AddComponent<DialogueHighlight>().highlightType = DialogueHighlight.DialogueHighlightType.PassedSkillCheck;
                } else
                {
                    responseObj.AddComponent<DialogueHighlight>().highlightType = DialogueHighlight.DialogueHighlightType.FailedSkillCheck;
                    return;
                }
            } else
            {
                responseObj.AddComponent<DialogueHighlight>();
            }

            
            responseObj.GetComponent<Button>().onClick.AddListener(() => 
            {
                NextNode(response.NextNode); 
                if (response.Then != null) 
                {
                    Invoke(response.Then, 0); 
                }
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