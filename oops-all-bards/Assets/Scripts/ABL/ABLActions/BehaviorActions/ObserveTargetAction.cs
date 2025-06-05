using UnityEngine;
using Viv;

public class ObserveTargetAction : IABLAction
{
    public void Execute(string jsonData)
    {
        ObserveTargetData data = JsonUtility.FromJson<ObserveTargetData>(jsonData);

        VivCharacter vivCharacter = Viv.Viv.Instance.FindVivCharacter(data.characterId);
        VivCharacterController controller = vivCharacter?.Controller;

        // TODO: Need a more robust way to find the target, possibly using PartyManager or GameManager
        // This assumes that the target is a party member and that the GameObject's name matches the returned BasePlayer's name.
        BasePlayer target = PartyManager.Instance.FindPartyMemberById(data.targetId);
        GameObject targetGO = GameObject.Find(target.Name);
        if (target == null || targetGO == null)
        {
            Debug.LogError($"ObserveTargetAction: Could not find target with ID {data.targetId} or name {target.Name}.");
            return;
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