using UnityEngine;
using System.Collections.Generic;
using DELP;
using Viv;

public class KnowledgeUpdater : MonoBehaviour
{
    public List<KnowledgeUpdateRule> rules;
    private System.Action<object> handleInteractionLambda;
    private Viv.Viv vivInstance;

    void Start()
    {
        vivInstance = Viv.Viv.Instance;
        if (vivInstance == null)
        {
            Debug.LogError("Viv instance not found. KnowledgeUpdater will not function.");
        }

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
        if (vivInstance == null) return;

        Dictionary<string, object> data = eventData as Dictionary<string, object>;
        if (data == null) { /* Error handling */ return; }

        GameObject actor = data.TryGetValue("actor", out object actorObj) ? actorObj as GameObject : null;
        GameObject target = data.TryGetValue("target", out object targetObj) ? targetObj as GameObject : null;
        string interactionType = data.TryGetValue("interactionType", out object typeObj) ? typeObj as string : null;
        string receivedOutcome = data.TryGetValue("outcome", out object outcomeObj) ? outcomeObj as string : null; // <-- Get the string outcome

        if (actor == null || string.IsNullOrEmpty(interactionType) || string.IsNullOrEmpty(receivedOutcome)) // <-- Check string outcome
        {
            Debug.LogWarning("KnowledgeUpdater received incomplete interaction data after unpacking.");
            return;
        }

        foreach (KnowledgeUpdateRule rule in rules)
        {
            if (string.Equals(rule.interactionType, interactionType, System.StringComparison.OrdinalIgnoreCase) &&
                string.Equals(rule.outcome, receivedOutcome, System.StringComparison.Ordinal))
            {
                List<VivCharacter> affectedCharacters = GetAffectedCharacters(actor, target, rule);

                foreach (VivCharacter recipientCharacter in affectedCharacters)
                {
                    ApplyRuleToRecipient(actor, target, recipientCharacter, rule);
                }
            }
        }
    }

    // Determines which characters are affected by a rule based on the event and rule criteria
    private List<VivCharacter> GetAffectedCharacters(GameObject actor, GameObject target, KnowledgeUpdateRule rule)
    {
        List<VivCharacter> affected = new List<VivCharacter>();
        VivCharacter actorViv = actor?.GetComponent<VivCharacter>(); // Use ?. for null check
        VivCharacter targetViv = target?.GetComponent<VivCharacter>();

        switch (rule.applicability)
        {
            case RuleApplicability.DirectParticipantsOnly:
                if (rule.actorMatters && actorViv != null) affected.Add(actorViv);
                // Add target only if it's different from actor or if actor doesn't matter
                if (rule.targetMatters && targetViv != null && (!rule.actorMatters || actorViv != targetViv))
                {
                    if (!affected.Contains(targetViv)) affected.Add(targetViv); // Avoid adding twice
                }
                break;

            case RuleApplicability.SpecificCharacterByID:
                if (rule.specificCharacterID != -1)
                {
                    // We need Viv to find characters by ID now
                    VivCharacter specificChar = vivInstance.FindVivCharacter(rule.specificCharacterID); // Assumes Viv.FindVivCharacter(id) exists
                    if (specificChar != null) affected.Add(specificChar);
                    else { Debug.LogWarning($"Rule '{rule.name}' references specific Character ID {rule.specificCharacterID} which was not found."); }
                }
                break;

            // case RuleApplicability.CharactersWithFaction:
            //     if (!string.IsNullOrEmpty(rule.specificFaction))
            //     {
            //         // We need Viv or another manager to find characters by faction
            //         affected.AddRange(vivInstance.FindCharactersByFaction(rule.specificFaction)); // Assumes Viv.FindCharactersByFaction(name) exists
            //     }
            //     break;

            case RuleApplicability.CharactersNearActor:
                if (actor != null)
                {
                    affected.AddRange(FindNearbyVivCharacters(actor.transform.position, rule.proximityRadius));
                }
                break;

            case RuleApplicability.CharactersNearTarget:
                if (target != null)
                {
                    affected.AddRange(FindNearbyVivCharacters(target.transform.position, rule.proximityRadius));
                }
                break;

            case RuleApplicability.AllCharacters:
                affected.AddRange(vivInstance.GetAllRegisteredCharacters()); // Assumes Viv.GetAllRegisteredCharacters() exists
                break;
        }
        return affected;
    }

    private void ApplyRuleToRecipient(GameObject eventActor, GameObject eventTarget, VivCharacter recipient, KnowledgeUpdateRule rule)
    {
        if (recipient == null) return;

        DELPEntity delp = recipient.delpEntity; // Get DELP entity directly
        if (delp != null)
        {
            // Apply fact changes, passing the recipient context
            AddFact(eventActor, eventTarget, recipient, rule.factToAdd, delp);
            RemoveFact(eventActor, eventTarget, recipient, rule.factToRemove, delp);
        }
        else
        {
            Debug.LogWarning($"VivCharacter '{recipient.characterName}' has no DELPEntity assigned when trying to apply rule '{rule.name}'.");
        }
    }

    // Add fact helper function.
    private void AddFact(GameObject eventActor, GameObject eventTarget, VivCharacter recipient, string factTemplate, DELPEntity delp)
    {
        if (delp != null && !string.IsNullOrEmpty(factTemplate))
        {
            string processedFact = ProcessFactString(eventActor, eventTarget, recipient, factTemplate);
            if (!string.IsNullOrEmpty(processedFact))
            {
                delp.AddFact(processedFact);
                Debug.Log($"Firing DELP_KnowledgeBaseUpdated event for character ID: {recipient.characterID}");
                var eventData = new KnowledgeUpdateEventData { characterID = recipient.characterID };
                EventManager.Instance.InvokeEvent(EventType.DELP_KnowledgeBaseUpdated, eventData);
            }
        }
    }

    private void RemoveFact(GameObject eventActor, GameObject eventTarget, VivCharacter recipient, string factTemplate, DELPEntity delp)
    {
        if (delp != null && !string.IsNullOrEmpty(factTemplate))
        {
            string processedFact = ProcessFactString(eventActor, eventTarget, recipient, factTemplate);
            if (!string.IsNullOrEmpty(processedFact))
            {
                delp.RemoveFact(processedFact);
                Debug.Log($"Firing DELP_KnowledgeBaseUpdated event for character ID: {recipient.characterID}");
                var eventData = new KnowledgeUpdateEventData { characterID = recipient.characterID };
                EventManager.Instance.InvokeEvent(EventType.DELP_KnowledgeBaseUpdated, eventData);
            }
        }
    }

    // Process fact string.
    private string ProcessFactString(GameObject eventActor, GameObject eventTarget, VivCharacter recipient, string factTemplate)
    {
        if (string.IsNullOrEmpty(factTemplate) || recipient == null)
        {
            Debug.LogWarning("ProcessFactString: Null factTemplate or recipient.");
            return string.Empty;
        }

        string processedFact = factTemplate;

        // 1. Replace "{self}" with the recipient's identifier
        processedFact = processedFact.Replace("{self}", recipient.characterName);

        // 2. Replace "{eventActor}"
        if (eventActor != null)
        {
            VivCharacter evActorVivChar = eventActor.GetComponent<VivCharacter>();
            processedFact = processedFact.Replace("{eventActor}", evActorVivChar?.characterName ?? eventActor.name);
        }
        else
        {
            processedFact = processedFact.Replace("{eventActor}", "unknownActor"); // Or handle as error
        }

        // 3. Replace "{eventTarget}"
        if (eventTarget != null)
        {
            VivCharacter evTargetVivChar = eventTarget.GetComponent<VivCharacter>();
            processedFact = processedFact.Replace("{eventTarget}", evTargetVivChar?.characterName ?? eventTarget.name);
        }
        else
        {
            processedFact = processedFact.Replace("{eventTarget}", "unknownTarget"); // Or handle as error
        }

        return processedFact;
    }

    private IEnumerable<VivCharacter> FindNearbyVivCharacters(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        List<VivCharacter> nearby = new List<VivCharacter>();
        foreach (Collider hit in colliders)
        {
            VivCharacter vivChar = hit.GetComponent<VivCharacter>();
            if (vivChar != null)
            {
                nearby.Add(vivChar);
            }
        }
        return nearby;
        // Alternative: Iterate through Viv.Instance.GetAllRegisteredCharacters() and check distance.
    }
}