using UnityEngine;

/// <summary>
/// Represents a Working Memory Element (WME) that signals the status 
/// of a completed or terminated ICharacterBehavior in Unity.
/// This is intended to be created in Unity and then packaged into an ABLMessage
/// to be sent to and processed by the ABL system.
/// </summary>
[System.Serializable]
public class BehaviorStatusWME
{
    [SerializeField] private int characterID;
    [SerializeField] private string behaviorName; // e.g., "Move", "Observe", "CalmConfrontation"
    [SerializeField] private string status;       // e.g., "Success", "Failure", "Interrupted"

    private const int MESSAGE_CODE = 3;
    private const string MESSAGE_TYPE = "BehaviorStatusWME";

    /// <summary>
    /// Constructor for BehaviorStatusWME.
    /// </summary>
    /// <param name="charID">The ID of the character whose behavior status is being reported.</param>
    /// <param name="behName">The name of the behavior (typically from ICharacterBehavior.GetBehaviorName()).</param>
    /// <param name="stat">The completion status: "Success", "Failure", or "Interrupted".</param>
    public BehaviorStatusWME(int charID, string behName, string stat)
    {
        this.characterID = charID;
        this.behaviorName = behName;
        this.status = stat;
    }

    // Public properties to access the fields
    public int CharacterID
    {
        get { return this.characterID; }
    }

    public string BehaviorName
    {
        get { return this.behaviorName; }
    }

    public string Status
    {
        get { return this.status; }
    }

    /// <summary>
    /// Packages this WME into an ABLMessage for sending to the ABL server.
    /// The server uses the message code (3) to identify how to parse the JSON data.
    /// </summary>
    /// <returns>An ABLMessage containing this WME's data.</returns>
    public ABLMessage ToABLMessage()
    {
        string jsonData = JsonUtility.ToJson(this);
        ABLMessage message = new ABLMessage(MESSAGE_CODE, MESSAGE_TYPE, jsonData);
        return message;
    }

    /// <summary>
    /// Utility method to convert this WME directly to a JSON string for logging or other purposes.
    /// </summary>
    /// <returns>A JSON string representation of this object.</returns>
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
