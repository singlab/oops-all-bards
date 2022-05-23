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
            toInstantiate.transform.parent = nodeContentOrganizer.transform;
            toInstantiate.GetComponentInChildren<TMP_Text>().text = response.NodeResponseText;
            toInstantiate.GetComponent<Button>().onClick.AddListener(delegate { NextNode(response.NextNode); } );
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
        if (index != -1 && index != -999)
        {
            nodeIndex = index;
            DialogueNode currentNode = jsonReader.dialogues.GetDialogue(dialogueIndex).DialogueNodes[nodeIndex];
            RenderCurrentNode(currentNode);
        } else if (index == -1)
        {
            CloseDialogue();
        } else
        {
            CloseDialogue();
            DemoManager.Instance.PrepareForGig();
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

    public void AssignDialogueUI()
    {
        dialogueUI = GameObject.Find("DialogueUI");
    }
}
