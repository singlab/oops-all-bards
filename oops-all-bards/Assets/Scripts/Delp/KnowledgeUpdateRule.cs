using UnityEngine;
using System.Collections.Generic;

public enum RuleApplicability
{
    DirectParticipantsOnly, // Only Actor/Target (based on actor/targetMatters)
    SpecificCharacterByID,
    CharactersWithFaction,
    CharactersNearActor,
    CharactersNearTarget,
    AllCharacters
}

[CreateAssetMenu(fileName = "KnowledgeUpdateRule", menuName = "AI/Knowledge Update Rule", order = 0)]
public class KnowledgeUpdateRule : ScriptableObject
{
    [Header("Event Matching")]
    [Tooltip("Type of interaction (e.g., Dialogue, Attack).")]
    public string interactionType;
    [Tooltip("Specific outcome of the interaction.")]
    public string outcome;

    [Header("Knowledge Change")]
    [Tooltip("Fact to add (use 'actor', 'target', 'self' placeholders).")]
    public string factToAdd;
    [Tooltip("Optional: Fact to remove (use 'actor', 'target', 'self' placeholders).")]
    public string factToRemove;

    [Header("Applicability - Who gets updated?")]
    [Tooltip("How to determine which character(s) this rule applies to.")]
    public RuleApplicability applicability = RuleApplicability.DirectParticipantsOnly;

    [Tooltip("Character ID if Applicability is SpecificCharacterByID.")]
    public int specificCharacterID = -1;

    [Tooltip("Faction Name if Applicability is CharactersWithFaction.")]
    public string specificFaction = "";

    [Tooltip("Radius if Applicability is CharactersNearActor or CharactersNearTarget.")]
    public float proximityRadius = 10.0f;

    [Tooltip("If DirectParticipantsOnly, should the actor's KB be updated?")]
    public bool actorMatters = true; // Default to true if direct

    [Tooltip("If DirectParticipantsOnly, should the target's KB be updated?")]
    public bool targetMatters = true; // Default to true if direct

    [Tooltip("Designer description of what this rule does.")]
    [TextArea(3, 10)]
    public string description;
}
