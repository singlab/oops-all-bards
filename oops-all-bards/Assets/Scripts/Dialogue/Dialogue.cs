using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [SerializeField] private int id;
    [SerializeField] private string speakerName;
    [SerializeField] private bool exhausted;
    [SerializeField] private List<DialogueNode> dialogueNodes = new List<DialogueNode>();

    public int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public string SpeakerName
    {
        get { return this.speakerName; }
        set { this.speakerName = value; }
    }
    
    public bool Exhausted
    {
        get { return this.exhausted; }
        set { this.exhausted = value; }
    }

    public List<DialogueNode> DialogueNodes
    {
        get { return this.dialogueNodes; }
        set { this.dialogueNodes = value; }
    }
}

[System.Serializable]
public class DialogueNode
{
    [SerializeField] private int id;
    [SerializeField] private string nodeText;
    [SerializeField] private List<NodeResponse> nodeResponses = new List<NodeResponse>();

    public int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public string NodeText
    {
        get { return this.nodeText; }
        set { this.nodeText = value; }
    }

    public List<NodeResponse> NodeResponses
    {
        get { return this.nodeResponses; }
        set { this.nodeResponses = value; }
    }
}

[System.Serializable]
public class NodeResponse
{
    [SerializeField] private string nodeResponseText;
    [SerializeField] private int nextNode;
    // TODO: Implement effects of choosing a response in a dialogue

    public string NodeResponseText
    {
        get { return this.nodeResponseText; }
        set { this.nodeResponseText = value; }
    }

    public int NextNode
    {
        get { return this.nextNode; }
        set { this.nextNode = value; }
    }
}

[System.Serializable]
public class Dialogues
{
	[SerializeField] public List<Dialogue> dialogues = new List<Dialogue>();

    public Dialogue GetDialogue(int id)
    {
        foreach (Dialogue d in dialogues)
        {
            if (d.ID == id)
            {
                return d;
            }
        }
        return null;
    }
}
