using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    public List<DialogueEvent> dialogueEvents = new List<DialogueEvent>();
    private bool triggering;
    public string exhaustedDialogueResponse;

    public void Execute()
    {
        // Find the relevant dialogue event
        DialogueEvent selectedEvent = FindMatchingEvent();

        if (selectedEvent != null)
        {
            // Trigger the selected dialogue event
            DialogueManager.Instance.StartDialogue(selectedEvent.dialogueID);

            // Handle quest stage progression if applicable and questStageID is valid
            if (selectedEvent.eventType == DialogueEvent.DialogueEventType.QuestContinue && selectedEvent.questStageID != -1) 
            {
                QuestManager.Instance.MarkStageComplete(); 
            }

            // Handle quest acceptance if applicable
            if (selectedEvent.eventType == DialogueEvent.DialogueEventType.QuestStart)
            {
                QuestManager.Instance.AcceptQuest(selectedEvent.questID);
            }

            selectedEvent.exhausted = true;
        }
        else
        {
            DialogueManager.Instance.SpawnTextBubble(gameObject, exhaustedDialogueResponse); 
        }
    }

    private DialogueEvent FindMatchingEvent()
    {
        foreach (DialogueEvent dialogueEvent in dialogueEvents)
        {
            if (!dialogueEvent.exhausted && CheckConditions(dialogueEvent.checkCondition) && 
                (dialogueEvent.questID == QuestManager.Instance.GetCurrentQuestID() || dialogueEvent.questStageID == -99)) 
            {
                return dialogueEvent;
            }
        }
        return null;
    }

    private bool CheckConditions(DialogueEvent.Condition condition)
    {
        switch (condition.conditionType)
        {
            case DialogueEvent.Condition.ConditionType.QuestCompleted:
                return QuestManager.Instance.IsQuestCompleted(condition.relatedID);
            case DialogueEvent.Condition.ConditionType.PreviousStageCompleted:
                return QuestManager.Instance.IsPreviousStageCompleted(condition.relatedID);
            // case DialogueEvent.Condition.ConditionType.ItemInInventory:
            //     return InventoryManager.Instance.HasItem(condition.relatedID);
            // case DialogueEvent.Condition.ConditionType.FlagSet:
            //     return GameManager.Instance.IsFlagSet(condition.relatedID);
            default:
                return true; // No condition, always allow
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
}
