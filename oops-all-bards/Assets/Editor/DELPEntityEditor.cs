using UnityEngine;
using UnityEditor; // Required for editor scripts

[CustomEditor(typeof(DELPEntity))]
public class DELPEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields (your lists of facts and rules)
        base.OnInspectorGUI();

        // Get a reference to the DELPEntity script we are inspecting
        DELPEntity delpEntity = (DELPEntity)target;

        // Add some space for our button
        EditorGUILayout.Space();

        // Add a button to the Inspector
        if (GUILayout.Button("Reset to Initial State"))
        {
            // When the button is clicked, call our new public method
            delpEntity.ClearRuntimeFacts();

            // Mark the object as "dirty" so Unity knows to save the changes
            EditorUtility.SetDirty(delpEntity);
        }
    }
}