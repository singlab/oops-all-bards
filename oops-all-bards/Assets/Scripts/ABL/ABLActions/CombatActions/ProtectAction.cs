using UnityEngine;

public class ProtectAction : IABLAction
{
    public void Execute(string jsonData)
    {
        ProtectData data = JsonUtility.FromJson<ProtectData>(jsonData);

        if (data == null)
        {
            Debug.LogError("ProtectAction: Failed to parse action data.");
            return;
        }

        BasePlayer actingCharacter = PartyManager.Instance.FindPartyMemberById(data.characterId);
        ITargetable target = PartyManager.Instance.FindPartyMemberById(data.targetId);

        if (actingCharacter == null || target == null)
        {
            Debug.LogError($"ProtectAction: Could not find party member for actor ID {data.characterId} or target ID {data.targetId}.");
            return;
        }

        AllyAction action = new AllyAction(actingCharacter, target, AllyAction.ActionTypes.PROTECT);
        CombatManager.Instance.combatQueue.PriorityPush(action);

        Debug.Log($"ProtectAction: Character {data.characterId} is protecting {data.targetId}.");
    }
}
