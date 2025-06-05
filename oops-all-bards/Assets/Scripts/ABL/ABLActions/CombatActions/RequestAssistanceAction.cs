using UnityEngine;

public class RequestAssistanceAction : IABLAction
{
    public void Execute(string jsonData)
    {
        RequestAssistanceData data = JsonUtility.FromJson<RequestAssistanceData>(jsonData);

        if (data == null)
        {
            Debug.LogError("RequestAssistanceAction: Failed to parse action data.");
            return;
        }

        BasePlayer actingCharacter = PartyManager.Instance.FindPartyMemberById(data.characterId);

        if (actingCharacter == null)
        {
            Debug.LogError($"RequestAssistanceAction: Could not find party member for actor ID {data.characterId}.");
            return;
        }

        actingCharacter.CiFData.AddStatus(new Status(Status.StatusTypes.REQUIRES_ASSISTANCE));
        Debug.Log($"RequestAssistanceAction: Character {data.characterId} is requesting assistance.");
    }
}
