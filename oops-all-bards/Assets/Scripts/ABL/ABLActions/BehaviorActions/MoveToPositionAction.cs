using UnityEngine;
using Viv;

public class MoveToPositionAction : IABLAction
{
    public void Execute(string jsonData)
    {
        MoveToPositionData data = JsonUtility.FromJson<MoveToPositionData>(jsonData);

        if (data == null)
        {
            Debug.LogError("MoveToPositionAction: Received null data.");
            return;
        }

        Debug.Log($"MoveToPositionAction: Executing action for character ID {data.characterId} to move to position ({data.x}, {data.y}, {data.z}).");

        VivCharacter vivCharacter = Viv.Viv.Instance.FindVivCharacter(data.characterId);
        VivCharacterController controller = vivCharacter?.Controller;

        if (controller != null)
        {
            Vector3 destination = new Vector3(data.x, data.y, data.z);
            controller.Action_MoveToPosition(destination);
        }
        else
        {
            Debug.LogError($"MoveToPositionAction: Could not find VivCharacterController for character ({data.characterId}).");
        }
    }
}