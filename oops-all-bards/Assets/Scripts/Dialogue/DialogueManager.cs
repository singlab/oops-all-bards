using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DialogueManager : MonoBehaviour
{

    private static DialogueManager _instance;
    public static DialogueManager Instance => DialogueManager._instance;
    private int nodeIndex = 0;
    private int dialogueIndex = 0;
    public JSONReader jsonReader;
    public GameObject dialogueUI;
    public GameObject nodeContentOrganizer;
    public TMP_Text nodeText;
    public Image portrait;
    public TMP_Text speakerName;
    public GameObject nodeResponsePrefab;
    public GameObject textBubblePrefab;

    // Singleton pattern
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //jsonReader.quips.DebugQuips();
    }

    public void ToggleDialogueUI()
    {
        dialogueUI.SetActive(!dialogueUI.activeSelf);
    }

    public void StartDialogue(int dialogueID)
    {
        nodeIndex = 0;
        Debug.Log("Starting dialogue.");
        DemoManager.Instance.TogglePlayerControls();
        dialogueIndex = dialogueID;
        Dialogue dialogue = jsonReader.dialogues.GetDialogue(dialogueID);
        RenderDialogueUI(dialogue);
    }

    private void RenderDialogueUI(Dialogue dialogue)
    {
        speakerName.text = dialogue.SpeakerName;
        portrait.sprite = Resources.Load<Sprite>($"Portraits/{dialogue.SpeakerName}");
        DialogueNode currentNode = dialogue.DialogueNodes[nodeIndex];
        RenderCurrentNode(currentNode);
        ToggleDialogueUI();
    }

    private void RenderCurrentNode(DialogueNode node)
    {
        ClearNodeResponses();
        nodeText.text = node.NodeText;
        for (int i = 0; i < node.NodeResponses.Count; i++)
        {
            NodeResponse response = node.NodeResponses[i];
            GameObject toInstantiate = Instantiate(nodeResponsePrefab, nodeContentOrganizer.transform.position, Quaternion.identity);
            toInstantiate.transform.SetParent(nodeContentOrganizer.transform);
            toInstantiate.GetComponentInChildren<TMP_Text>().text = response.NodeResponseText;
            toInstantiate.GetComponent<Button>().onClick.AddListener(delegate { NextNode(response.NextNode); } );
            if (response.Then != null)
            {
                Debug.Log($"{response.Then}");
                toInstantiate.GetComponent<Button>().onClick.AddListener(delegate { Invoke(response.Then, 0); } );
            }
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
        if (index != -1 && index != -2 && index != -999)
        {
            nodeIndex = index;
            DialogueNode currentNode = jsonReader.dialogues.GetDialogue(dialogueIndex).DialogueNodes[nodeIndex];
            RenderCurrentNode(currentNode);
        } else if (index == -1)
        {
            CloseDialogue();
        } else if (index == -2)
        {
            CloseDialogue();
            DemoManager.Instance.RecruitQuinton();
            GameObject.Find("Quinton").GetComponent<NPCMovement>().SendQuintonToBackroom();
        } else
        {
            CloseDialogue();
            DemoManager.Instance.LoadScene("GigDemo");
        }
    }

    private void CloseDialogue()
    {
        Dialogue dialogue = jsonReader.dialogues.GetDialogue(dialogueIndex);
        dialogue.Exhausted = true;
        ToggleDialogueUI();
        DemoManager.Instance.TogglePlayerControls();
    }

    // Spawn a text bubble prefab above the given character's gameobject with the given text.
    public void SpawnTextBubble(GameObject character, string text)
    {
        GameObject target = character.transform.Find("CameraTarget").gameObject;
        GameObject toInstantiate = Instantiate(textBubblePrefab, (target.transform.position + new Vector3(0, 0.5f, 0)), Quaternion.identity, target.transform);
        toInstantiate.GetComponentInChildren<TMP_Text>().text = text;
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
        } else if (!inCombat && (inCombat == PartyManager.Instance.inCombat))
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
        } else
        {
            return;
        }
        
        Debug.Log(relevantQuips.Count);
        chosenQuip = relevantQuips[Random.Range(0, relevantQuips.Count - 1)];
        SpawnTextBubble(model, chosenQuip.Text);
    }
}
