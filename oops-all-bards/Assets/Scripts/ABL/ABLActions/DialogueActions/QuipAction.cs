using UnityEngine;

public class QuipAction : IABLAction
{
    public void Execute(string jsonData)
    {
        QuipData data = JsonUtility.FromJson<QuipData>(jsonData);

        if (data == null)
        {
            Debug.LogError("QuipAction: Failed to parse action data.");
            return;
        }

        Debug.Log($"QuipAction executed for character ID: {data.characterId}. In combat: {data.inCombat}");

        return;
    }
}