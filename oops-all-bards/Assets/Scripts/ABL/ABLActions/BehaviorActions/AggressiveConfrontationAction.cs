using UnityEngine;
using Viv;

public class AggressiveConfrontationAction : IABLAction
{
    public void Execute(string jsonData)
    {
        ConfrontationData data = JsonUtility.FromJson<ConfrontationData>(jsonData);

        if (data == null)
        {
            Debug.LogError("AggressiveConfrontationAction: Received null data.");
            return;
        }

        Debug.Log($"AggressiveConfrontationAction: Executing action for character ID {data.characterId} to confront target ID.");

        VivCharacter vivCharacter = Viv.Viv.Instance.FindVivCharacter(data.characterId);
        VivCharacterController controller = vivCharacter?.Controller;

        // TODO: Need a more robust way to find the target, possibly using PartyManager or GameManager
        // This assumes that the target is a party member and that the GameObject's name matches the returned BasePlayer's name.
        BasePlayer target = PartyManager.Instance.FindPartyMemberById(data.targetId);
        GameObject targetGO = GameObject.Find(target.Name);
        if (target == null || targetGO == null)
        {
            Debug.Log($"ObserveTargetAction: Could not find target with ID {data.targetId} or name {target.Name}. Falling back to Player tag");
            targetGO = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        if (controller != null && targetGO != null)
        {
            controller.Action_AggressiveConfrontation(targetGO);
        }
        else
        {
            Debug.LogError($"AggressiveConfrontationAction: Could not find VivCharacterController for character ({data.characterId}) or target ({data.targetId}).");
        }
    }
}