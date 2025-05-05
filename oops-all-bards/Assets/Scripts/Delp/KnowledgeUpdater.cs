using UnityEngine;
using System.Collections.Generic;
using DELP;
using Viv;

public class KnowledgeUpdater : MonoBehaviour
{
    public List<KnowledgeUpdateRule> rules;
    private System.Action<object> handleInteractionLambda;

    void Start()
    {
        handleInteractionLambda = (eventData) => HandleInteraction(eventData);

        EventManager.Instance.SubscribeToEvent(EventType.OnInteraction, handleInteractionLambda);
    }

    void OnDestroy()
    {
        if (EventManager.Instance != null && handleInteractionLambda != null)
        {
            EventManager.Instance.UnsubscribeToEvent(EventType.OnInteraction, handleInteractionLambda);
        }
    }

    private void HandleInteraction(object eventData)
    {
        Dictionary<string, object> data = eventData as Dictionary<string, object>;
        if (data == null) { Debug.LogError("HandleInteraction received invalid eventData format."); return; }

        GameObject actor = data.TryGetValue("actor", out object actorObj) ? actorObj as GameObject : null;
        GameObject target = data.TryGetValue("target", out object targetObj) ? targetObj as GameObject : null;
        string interactionType = data.TryGetValue("interactionType", out object typeObj) ? typeObj as string : null;
        string receivedOutcome = data.TryGetValue("outcome", out object outcomeObj) ? outcomeObj as string : null; // <-- Get the string

        if (actor == null || string.IsNullOrEmpty(interactionType) || string.IsNullOrEmpty(receivedOutcome)) // <-- Check string
        {
            Debug.LogError("KnowledgeUpdater received incomplete interaction data after unpacking.");
            return;
        }

        foreach (KnowledgeUpdateRule rule in rules)
        {
            // Check if the rule matches the event.
            // Compare interactionType string and the outcome string
            if (string.Equals(rule.interactionType, interactionType, System.StringComparison.OrdinalIgnoreCase) &&
                string.Equals(rule.outcome, receivedOutcome, System.StringComparison.Ordinal)) // <-- String comparison
            {
                ApplyRule(actor, target, rule);
            }
        }
    }

    private void ApplyRule(GameObject actor, GameObject target, KnowledgeUpdateRule rule)
    {
        // If the actor matters for this rule, update the delp entity for the actor.
        if (rule.actorMatters)
        {
            VivCharacter actorVivChar = actor.GetComponent<VivCharacter>();
            if (actorVivChar != null && actorVivChar.characterID != -1) // Check if component exists and ID is valid
            {
                int id = actorVivChar.characterID;
                DELPEntity delp = Viv.Viv.Instance.FindCharacterDELPEntity(id);
                if (delp != null)
                {
                    AddFact(actor, target, rule.factToAdd, delp);
                    RemoveFact(actor, target, rule.factToRemove, delp);
                }
                else { Debug.LogWarning($"Could not find DELPEntity for actor ID: {id}"); }
            }
            else { Debug.LogWarning($"Actor '{actor.name}' does not have a valid VivCharacter component or ID."); }
        }

        // If the target matters, update the delp entity for the target.
        // Make sure target is not null before proceeding
        if (rule.targetMatters && target != null)
        {
            VivCharacter targetVivChar = target.GetComponent<VivCharacter>();
            if (targetVivChar != null && targetVivChar.characterID != -1)
            {
                int id = targetVivChar.characterID;
                DELPEntity delp = Viv.Viv.Instance.FindCharacterDELPEntity(id);
                if (delp != null)
                {
                    AddFact(actor, target, rule.factToAdd, delp);
                    RemoveFact(actor, target, rule.factToRemove, delp);
                }
                else { Debug.LogWarning($"Could not find DELPEntity for target ID: {id}"); }
            }
            else { Debug.LogWarning($"Target '{target.name}' does not have a valid VivCharacter component or ID."); }
        }
    }

    // Add fact helper function.
    private void AddFact(GameObject actor, GameObject target, string fact, DELPEntity delp)
    {
        if (delp != null && !string.IsNullOrEmpty(fact)) // Check if fact string is valid
        {
            string processedFact = ProcessFactString(actor, target, fact);
            if (!string.IsNullOrEmpty(processedFact)) // Ensure processing didn't result in empty string
            {
                delp.AddFact(processedFact);
            }
        }
    }

    // Remove fact helper function.
    private void RemoveFact(GameObject actor, GameObject target, string fact, DELPEntity delp)
    {
        if (delp != null && !string.IsNullOrEmpty(fact)) // Check if fact string is valid
        {
            string processedFact = ProcessFactString(actor, target, fact);
            if (!string.IsNullOrEmpty(processedFact)) // Ensure processing didn't result in empty string
            {
                delp.RemoveFact(processedFact);
            }
        }
    }

    // Process fact string.
    private string ProcessFactString(GameObject actor, GameObject target, string fact)
    {
        if (string.IsNullOrEmpty(fact)) return string.Empty;

        // Replace placeholders with actual values. This makes the rules MUCH more flexible.
        string processedFact = fact;

        // Use actor name (can be changed to ID if DELP handles integers better)
        if (actor != null)
        {
            processedFact = processedFact.Replace("actor", actor.GetComponent<VivCharacter>()?.characterName ?? actor.name); // Use VivCharacter name if available
        }

        // Check if the target is valid before getting its name.
        if (target != null)
        {
            processedFact = processedFact.Replace("target", target.GetComponent<VivCharacter>()?.characterName ?? target.name); // Use VivCharacter name if available
        }

        // Add more placeholder replacements if needed (e.g., "location", "item")

        return processedFact;
    }
}