using UnityEngine;
using Viv;

public class ObserveTargetAction : IABLAction
{
    public void Execute(string jsonData)
    {
        ObserveTargetData data = JsonUtility.FromJson<ObserveTargetData>(jsonData);

        if (data == null)
        {
            Debug.LogError("ObserveTargetAction: Received null data.");
            return;
        }

        Debug.Log($"ObserveTargetAction: Executing action for character ID {data.characterId} to observe target ID {data.targetId} for {data.duration} seconds.");

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
        }

        if (controller != null && targetGO != null)
        {
            controller.Action_ObserveTarget(targetGO, data.duration);
        }
        else
        {
            Debug.LogError($"ObserveTargetAction: Could not find VivCharacterController for character ({data.characterId}) or target ({data.targetId}).");
        }
    }
}