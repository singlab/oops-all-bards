using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour, IInteractable
{
    public int questId;
    public int questStageId;
    public int dialogueId;
    private bool triggering;
    private bool hasExecuted = false;

    public void Execute()
    {
        if (dialogueId != -1) { // Only start dialogue if an ID is provided
          DialogueManager.Instance.StartDialogue(dialogueId);
        }

        if (questId != -1 && questStageId != -1) { // Only mark quest stage complete if IDs are provided
            Quest currentQuest = QuestManager.Instance.activeQuests.Find(q => q.ID == questId);
            if (currentQuest != null)
            {
                if (currentQuest.CurrentStageIndex == questStageId) { //Only mark complete if this is the current stage
                  QuestManager.Instance.MarkStageComplete();
                } else if (currentQuest.Linear) {
                  Debug.LogWarning("Trying to complete a non-current stage in a linear quest.  This will not work.");
                } else {
                  currentQuest.Stages[questStageId].Complete = true; //Mark complete without advancing if nonlinear.
                  QuestManager.Instance.UpdateQuestUI(); //Refresh UI to show progress.
                }
            }
            else
            {
                Debug.LogWarning("Quest not found in active quests list.");
            }
        }


    }

    void Update()
    {
        if (triggering && !hasExecuted)
        {
            Execute();
            hasExecuted = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
