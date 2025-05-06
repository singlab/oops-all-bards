using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(KnowledgeUpdateRule))]
public class KnowledgeUpdateRuleEditor : Editor
{
    // Lists for dropdowns
    private List<string> outcomeChoices;
    private List<string> outcomeDisplayNames;
    private List<string> interactionTypeChoices;
    private List<string> interactionTypeDisplayNames;
    private SerializedProperty interactionTypeProp;
    private SerializedProperty outcomeProp;
    private SerializedProperty factToAddProp;
    private SerializedProperty factToRemoveProp;
    private SerializedProperty actorMattersProp;
    private SerializedProperty targetMattersProp;
    private SerializedProperty descriptionProp;
    private SerializedProperty applicabilityProp;
    private SerializedProperty specificCharacterIDProp;
    private SerializedProperty specificFactionProp;
    private SerializedProperty proximityRadiusProp;

    void OnEnable()
    {
        // Find the properties
        interactionTypeProp = serializedObject.FindProperty("interactionType");
        outcomeProp = serializedObject.FindProperty("outcome");
        factToAddProp = serializedObject.FindProperty("factToAdd");
        factToRemoveProp = serializedObject.FindProperty("factToRemove");
        actorMattersProp = serializedObject.FindProperty("actorMatters");
        targetMattersProp = serializedObject.FindProperty("targetMatters");
        descriptionProp = serializedObject.FindProperty("description");
        applicabilityProp = serializedObject.FindProperty("applicability");
        specificCharacterIDProp = serializedObject.FindProperty("specificCharacterID");
        specificFactionProp = serializedObject.FindProperty("specificFaction");
        proximityRadiusProp = serializedObject.FindProperty("proximityRadius");

        // Populate outcome choices
        outcomeChoices = new List<string>();
        outcomeDisplayNames = new List<string>();
        outcomeChoices.Add("");
        outcomeDisplayNames.Add("None (Select Outcome)");
        CollectConstantStrings(typeof(OutcomeStrings), "", outcomeChoices, outcomeDisplayNames);

        // Populate InteractionType choices
        interactionTypeChoices = new List<string>();
        interactionTypeDisplayNames = new List<string>();
        interactionTypeChoices.Add("");
        interactionTypeDisplayNames.Add("None (Select Type)");
        CollectConstantStrings(typeof(InteractionTypes), "", interactionTypeChoices, interactionTypeDisplayNames);
    }

    void CollectConstantStrings(System.Type type, string prefix, List<string> valueList, List<string> displayList)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        foreach (FieldInfo field in fields.OrderBy(f => f.Name))
        {
            if (field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
            {
                string value = (string)field.GetValue(null);
                valueList.Add(value);
                displayList.Add(prefix + field.Name);
            }
        }
        System.Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static);
        foreach (System.Type nestedType in nestedTypes.OrderBy(t => t.Name))
        {
            string nestedPrefix = prefix + type.Name + "/";
            CollectConstantStrings(nestedType, nestedPrefix, valueList, displayList);
        }
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Event Matching", EditorStyles.boldLabel);
        // --- InteractionType Dropdown ---
        int currentInteractionTypeIndex = interactionTypeChoices.IndexOf(interactionTypeProp.stringValue);
        if (currentInteractionTypeIndex < 0) currentInteractionTypeIndex = 0;
        int selectedInteractionTypeIndex = EditorGUILayout.Popup("Interaction Type", currentInteractionTypeIndex, interactionTypeDisplayNames.ToArray());
        if (selectedInteractionTypeIndex != currentInteractionTypeIndex)
        {
            interactionTypeProp.stringValue = interactionTypeChoices[selectedInteractionTypeIndex];
        }
        // --- End InteractionType Dropdown ---

        // --- Outcome Dropdown ---
        int currentOutcomeIndex = outcomeChoices.IndexOf(outcomeProp.stringValue);
        if (currentOutcomeIndex < 0) currentOutcomeIndex = 0;
        int selectedOutcomeIndex = EditorGUILayout.Popup("Outcome", currentOutcomeIndex, outcomeDisplayNames.ToArray());
        if (selectedOutcomeIndex != currentOutcomeIndex)
        {
            outcomeProp.stringValue = outcomeChoices[selectedOutcomeIndex];
        }
        // --- End Outcome Dropdown ---

        EditorGUILayout.Space(); // Add spacing
        EditorGUILayout.PropertyField(factToAddProp);
        EditorGUILayout.PropertyField(factToRemoveProp);

        EditorGUILayout.Space(); // Add spacing
        // --- Draw Applicability Enum Dropdown ---
        EditorGUILayout.PropertyField(applicabilityProp);

        // --- Conditionally draw fields based on applicability ---
        RuleApplicability currentApplicability = (RuleApplicability)applicabilityProp.enumValueIndex;

        switch (currentApplicability)
        {
            case RuleApplicability.DirectParticipantsOnly:
                // Only show actor/target matters if applicability is DirectParticipantsOnly
                EditorGUILayout.PropertyField(actorMattersProp);
                EditorGUILayout.PropertyField(targetMattersProp);
                break;

            case RuleApplicability.SpecificCharacterByID:
                EditorGUILayout.PropertyField(specificCharacterIDProp);
                break;

            case RuleApplicability.CharactersWithFaction:
                EditorGUILayout.PropertyField(specificFactionProp);
                break;

            case RuleApplicability.CharactersNearActor:
            case RuleApplicability.CharactersNearTarget:
                EditorGUILayout.PropertyField(proximityRadiusProp);
                break;

            case RuleApplicability.AllCharacters:
                break;
        }
        // --- End Conditional Fields ---

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Description", EditorStyles.boldLabel);
        descriptionProp.stringValue = EditorGUILayout.TextArea(descriptionProp.stringValue, GUILayout.Height(60));

        serializedObject.ApplyModifiedProperties();
    }
}