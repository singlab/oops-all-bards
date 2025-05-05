using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(KnowledgeUpdateRule))]
public class KnowledgeUpdateRuleEditor : Editor
{
    // Lists for Outcome dropdown
    private List<string> outcomeChoices;
    private List<string> outcomeDisplayNames;

    // --- Add Lists for InteractionType dropdown ---
    private List<string> interactionTypeChoices;
    private List<string> interactionTypeDisplayNames;
    // --------------------------------------------

    // Serialized Properties
    private SerializedProperty interactionTypeProp;
    private SerializedProperty outcomeProp;
    private SerializedProperty factToAddProp;
    private SerializedProperty factToRemoveProp;
    private SerializedProperty actorMattersProp;
    private SerializedProperty targetMattersProp;
    private SerializedProperty descriptionProp;

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

        // Populate outcome choices (as before)
        outcomeChoices = new List<string>();
        outcomeDisplayNames = new List<string>();
        outcomeChoices.Add("");
        outcomeDisplayNames.Add("None (Select Outcome)");
        CollectConstantStrings(typeof(OutcomeStrings), "", outcomeChoices, outcomeDisplayNames); // Pass lists

        // --- Populate InteractionType choices ---
        interactionTypeChoices = new List<string>();
        interactionTypeDisplayNames = new List<string>();
        interactionTypeChoices.Add("");
        interactionTypeDisplayNames.Add("None (Select Type)");
        CollectConstantStrings(typeof(InteractionTypes), "", interactionTypeChoices, interactionTypeDisplayNames); // Use InteractionTypes class
        // ---------------------------------------
    }

    // Modified to accept lists as parameters
    void CollectConstantStrings(System.Type type, string prefix, List<string> valueList, List<string> displayList)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        foreach (FieldInfo field in fields.OrderBy(f => f.Name)) // Optional: Sort fields alphabetically
        {
            if (field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
            {
                string value = (string)field.GetValue(null);
                valueList.Add(value);
                displayList.Add(prefix + field.Name);
            }
        }

        System.Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static);
        foreach (System.Type nestedType in nestedTypes.OrderBy(t => t.Name)) // Optional: Sort nested types
        {
            string nestedPrefix = prefix + type.Name + "/";
            CollectConstantStrings(nestedType, nestedPrefix, valueList, displayList);
        }
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // --- InteractionType Dropdown ---
        int currentInteractionTypeIndex = interactionTypeChoices.IndexOf(interactionTypeProp.stringValue);
        if (currentInteractionTypeIndex < 0) currentInteractionTypeIndex = 0;

        int selectedInteractionTypeIndex = EditorGUILayout.Popup("Interaction Type", currentInteractionTypeIndex, interactionTypeDisplayNames.ToArray());

        if (selectedInteractionTypeIndex != currentInteractionTypeIndex)
        {
            interactionTypeProp.stringValue = interactionTypeChoices[selectedInteractionTypeIndex];
        }
        // --- End InteractionType Dropdown ---


        // --- Outcome Dropdown (remains the same) ---
        int currentOutcomeIndex = outcomeChoices.IndexOf(outcomeProp.stringValue);
        if (currentOutcomeIndex < 0) currentOutcomeIndex = 0;

        int selectedOutcomeIndex = EditorGUILayout.Popup("Outcome", currentOutcomeIndex, outcomeDisplayNames.ToArray());

        if (selectedOutcomeIndex != currentOutcomeIndex)
        {
            outcomeProp.stringValue = outcomeChoices[selectedOutcomeIndex];
        }
        // --- End Outcome Dropdown ---

        EditorGUILayout.PropertyField(factToAddProp);
        EditorGUILayout.PropertyField(factToRemoveProp);
        EditorGUILayout.PropertyField(actorMattersProp);
        EditorGUILayout.PropertyField(targetMattersProp);

        EditorGUILayout.LabelField("Description");
        descriptionProp.stringValue = EditorGUILayout.TextArea(descriptionProp.stringValue, GUILayout.Height(60));

        serializedObject.ApplyModifiedProperties();
    }
}