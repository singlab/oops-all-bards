using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    private bool triggering;
    public string exhaustedDialogueResponse;
    public DialogueQuestLinks[] dialogueQuestLinks;

    public void Execute()
    {
        Debug.Log("Executing dialogue.");

        // Find the relevant dialogue ID and quest stage ID
        int dialogueId = -1; // Initialize to an invalid value
        int questStageId = -1;
        int questId = -1;

        foreach (var link in dialogueQuestLinks)
        {
            if (QuestManager.Instance.activeQuests.Any(q => q.ID == link.questId)) //Check if the quest is active
            {
                questId = link.questId;
                dialogueId = link.dialogueId;
                questStageId = link.questStageId;
                break; // Stop searching once a match is found
            }
        }

        if (dialogueId == -1) // No matching quest/dialogue found
        {
            DialogueManager.Instance.SpawnTextBubble(gameObject, exhaustedDialogueResponse);
            return; // Exit early
        }

        Dialogue toStart = DialogueManager.Instance.jsonReader.dialogues.GetDialogue(dialogueId);

        if (toStart != null && !toStart.Exhausted)
        {
            DialogueManager.Instance.StartDialogue(dialogueId);
            if (DialogueManager.Instance.portrait.sprite == null)
            {
                Debug.Log("Generating Portrait");
                DialogueManager.Instance.dialogueModel(gameObject);
            }

            if (questStageId != -1)
            {
                Quest currentQuest = QuestManager.Instance.activeQuests.Find(q => q.ID == questId);
                if (currentQuest != null) {
                  currentQuest.CurrentStageIndex = questStageId; //Set current stage to the correct one.
                  QuestManager.Instance.MarkStageComplete(); // Mark the stage as complete using the QuestManager's function
                } else {
                  Debug.LogWarning("Quest not found in active quests list.");
                }
            }

        }
        else
        {
            DialogueManager.Instance.SpawnTextBubble(gameObject, exhaustedDialogueResponse);
        }
    }

    void Update()
    {
        if (triggering && Input.GetKeyDown(KeyCode.F))
        {
            Execute();
            triggering = false; // Prevent double triggering
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Use CompareTag for better performance
        {
            triggering = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggering = false;
        }
    }

    [System.Serializable]
    public class DialogueQuestLinks
    {
        public int questId;
        public int dialogueId;
        public int questStageId; // Use -1 if no quest stage is linked
    }
}
