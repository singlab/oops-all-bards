using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        ToggleDialogueUI();
    }

    private void ToggleDialogueUI()
    {
        dialogueUI.SetActive(!dialogueUI.activeSelf);
    }

    public void StartDialogue(int dialogueID)
    {
        Debug.Log("Starting dialogue.");
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
        nodeText.text = node.NodeText;
        for (int i = 0; i < node.NodeResponses.Count; i++)
        {
            NodeResponse response = node.NodeResponses[i];
            GameObject toInstantiate = Instantiate(nodeResponsePrefab, nodeContentOrganizer.transform.position, Quaternion.identity);
            toInstantiate.transform.parent = nodeContentOrganizer.transform;
            toInstantiate.GetComponentInChildren<TMP_Text>().text = response.NodeResponseText;
            toInstantiate.GetComponent<Button>().onClick.AddListener(delegate{NextNode(response.NextNode);});
        }
    }

    public void NextNode(int index)
    {
        nodeIndex = index;
        DialogueNode currentNode = jsonReader.dialogues.GetDialogue(dialogueIndex).DialogueNodes[nodeIndex];
        RenderCurrentNode(currentNode);
    }
}
