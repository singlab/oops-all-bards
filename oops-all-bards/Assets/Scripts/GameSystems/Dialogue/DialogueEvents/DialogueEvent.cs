using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueEvent", menuName = "RPG/Dialogue Event")]
public class DialogueEvent : ScriptableObject
{
    public enum DialogueEventType
    {
        QuestStart,
        QuestContinue,
        QuestComplete,
        Generic
    }

    public DialogueEventType eventType;

    // Prefab to display over the character's head
    public GameObject dialogueBubblePrefab;

    // Dialogue ID for easy referencing
    public int dialogueID;

    // Quest ID if this event is related to a quest
    public int questID;

    // Stage ID if completion of this dialogue event should complete a quest stage
    public int questStageID;

    // Optional: Condition to check before triggering this event
    public Condition checkCondition;
    // Whether or not this event has been exhausted
    public bool exhausted = false;

    [System.Serializable]
    public class Condition
    {
        public enum ConditionType
        {
            QuestCompleted,
            ItemInInventory,
            FlagSet,
            PreviousStageCompleted,
            None
        }

        public ConditionType conditionType;

        // Data for the specific condition (e.g., Quest ID, Item ID, Flag Name)
        public int relatedID; 
    }
}
