// MoveToTargetAction.cs (Updated)
using UnityEngine;
using Viv;

public class MoveToTargetAction : IABLAction
{
    public void Execute(string jsonData)
    {
        MoveToTargetData data = JsonUtility.FromJson<MoveToTargetData>(jsonData);

        if (data == null)
        {
            Debug.LogError("MoveToTargetAction: Received null data or failed to parse.");
            return;
        }

        VivCharacterController controller = Viv.Viv.Instance.FindVivCharacter(data.characterId)?.Controller;

        if (controller == null)
        {
            Debug.LogError($"MoveToTargetAction: Could not find VivCharacterController for character ({data.characterId}).");
            return;
        }

        GameObject targetGO = null;
        if (data.targetId == 0) // Special case for the Player
        {
            targetGO = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            BasePlayer targetData = PartyManager.Instance.FindPartyMemberById(data.targetId);
            if (targetData != null)
            {
                targetGO = GameObject.Find(targetData.Name);
            }
        }

        if (targetGO != null)
        {
            Debug.Log($"MoveToTargetAction: Executing for character {data.characterId} to move to target {targetGO.name}.");
            controller.Action_MoveToTarget(targetGO);
        }
        else
        {
            Debug.LogError($"MoveToTargetAction: Failed to find a valid target GameObject for ID {data.targetId}. Action will not be queued.");
        }
    }
}