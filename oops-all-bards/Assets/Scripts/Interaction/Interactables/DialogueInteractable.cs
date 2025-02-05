using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    private bool triggering;
    // public int dialogueIndex = 0;  
    // public int[] dialogueIds;
    public string exhaustedDialogueResponse;
    public DialogueQuestLinks[] dialogueQuestLinks; 

    // Assigned DialogueTrigger starts dialogue from manager if the dialogue
    // has not already been exhausted, or falls back to exhausted dialogue response if it has.
    public void Execute()
    {
        Debug.Log("Executing dialogue.");
        // TODO: Have characters in dialogue actually look towards the player when they speak.
        // transform.LookAt(Camera.main.transform);
        int dialogueId = 0;
        int savedIndex = 0;
        for (int i = 0; i < dialogueQuestLinks.Length; i++)
        {
            if (QuestManager.Instance.CurrentQuest == dialogueQuestLinks[i].dialogueQuestLink[0])
            {
                dialogueId = dialogueQuestLinks[i].dialogueQuestLink[2];
                savedIndex = i;
            }
        }

        Dialogue toStart = null;
        if (dialogueId != null)
        {
            toStart = DialogueManager.Instance.jsonReader.dialogues.GetDialogue(dialogueId);
        }

        if (toStart != null && !toStart.Exhausted)
        {
            DialogueManager.Instance.StartDialogue(dialogueId);
            if (DialogueManager.Instance.portrait.sprite == null)
            {
                Debug.Log("Generating Portrait");
                DialogueManager.Instance.dialogueModel(gameObject);
            }
            
            if (dialogueQuestLinks != null && dialogueQuestLinks.Length > 0)
            {
                if (dialogueQuestLinks[savedIndex].dialogueQuestLink[1] != -1)
                {
                    QuestManager.Instance.MarkStageComplete(dialogueQuestLinks[savedIndex].dialogueQuestLink[1]);
                }
            }
            
        } else
        {
            DialogueManager.Instance.SpawnTextBubble(gameObject, exhaustedDialogueResponse);
        }  
    }

    void Update()
    {
        if (triggering)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Execute();

                //Sarah's edit added to prevent triggering dialogue twice error
                triggering = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            triggering = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            triggering = false;
        }
    }

    [System.Serializable]
    public class DialogueQuestLinks
    {
        public int[] dialogueQuestLink;
    }
}
