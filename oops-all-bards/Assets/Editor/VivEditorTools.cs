using UnityEngine;
using UnityEditor;
using Viv;

public class VivEditorTools
{
    [MenuItem("GameObject/Viv/Create VivCharacter", false, 10)]
    static void CreateVivCharacterGameObject(MenuCommand menuCommand)
    {
        GameObject vivGO = new GameObject("Viv Character");
        vivGO.AddComponent<VivCharacter>();

        GameObjectUtility.SetParentAndAlign(vivGO, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(vivGO, "Create " + vivGO.name);

        Selection.activeObject = vivGO;

        Debug.Log("Created VivCharacter GameObject with required components.");
    }
}