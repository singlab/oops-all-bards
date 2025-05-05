using UnityEngine;

[CreateAssetMenu(fileName = "KnowledgeUpdateRule", menuName = "AI/Knowledge Update Rule")]
public class KnowledgeUpdateRule : ScriptableObject
{
    public string interactionType;
    public string outcome;
    public string factToAdd;
    public string factToRemove;
    public bool actorMatters;
    public bool targetMatters;
    [TextArea(3, 10)]
    public string description;
}
