using UnityEngine;
using Viv;

/// <summary>
/// Manages the player's Fame score, checks for threshold crossings,
/// and triggers relevant interaction events for the AI system.
/// </summary>
public class FameManager : MonoBehaviour
{
    public static FameManager Instance { get; private set; }

    [Header("Fame Settings")]
    [Tooltip("The Fame score required to trigger the 'High Combat Fame' event.")]
    [SerializeField] private int fameThresholdHighCombat = 60;
    // Add other relevant Fame thresholds here if needed later

    // Runtime fame value - consider saving/loading this data
    private int currentPlayerFame = 0;

    void Awake()
    {
        // Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // TODO: Load initial currentPlayerFame from save data if applicable
        // LoadFameData();
    }

    /// <summary>
    /// Gets the player's current Fame score.
    /// </summary>
    public int GetCurrentFame()
    {
        return currentPlayerFame;
    }

    /// <summary>
    /// Adds (or removes, if negative) Fame to the player's score and checks thresholds.
    /// Call this method from any system that grants/removes Fame (Combat rewards, Quests, etc.).
    /// </summary>
    /// <param name="amount">The amount of Fame to add (can be negative).</param>
    public void AddFame(int amount, BasePlayer player)
    {
        int oldFame = player.Fame;
        int currentPlayerFame = player.Fame += amount;
        currentPlayerFame = Mathf.Max(0, currentPlayerFame); // Ensure non-negative

        Debug.Log($"Player Fame changed from {oldFame} to {currentPlayerFame}. Added: {amount}");

        // Check if thresholds were crossed AFTER updating
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        CheckAndTriggerFameThresholds(playerObject, oldFame, currentPlayerFame);

        // TODO: Save currentPlayerFame to save data if applicable
        // SaveFameData();
    }

    /// <summary>
    /// Checks if any defined Fame thresholds were crossed and triggers events.
    /// </summary>
    private void CheckAndTriggerFameThresholds(GameObject playerObject, int oldFame, int newFame)
    {
        // Check for crossing HIGH COMBAT FAME threshold (going up)
        if (oldFame < fameThresholdHighCombat && newFame >= fameThresholdHighCombat)
        {
            Debug.Log($"FameManager: High Combat Fame threshold ({fameThresholdHighCombat}) crossed by Player ({playerObject.name})!");
            if (EventManager.Instance != null)
            {
                EventManager.Instance.TriggerInteraction(
                    playerObject, // Player is the actor whose reputation changed
                    null,         // Target usually null for reputation change
                    InteractionTypes.ReputationChange, // Use the defined constant
                    OutcomeStrings.Reputation.Reputation_Fame_High_Combat // Use the defined constant
                );
            }
            else
            {
                Debug.LogError("FameManager: EventManager Instance is null. Cannot trigger interaction.");
            }
        }

        // --- Add checks for other relevant thresholds ---
        // Example: Dropping below a certain threshold
        // int fameThresholdLow = 100;
        // if (oldFame >= fameThresholdLow && newFame < fameThresholdLow)
        // {
        //     EventManager.Instance?.TriggerInteraction(playerObject, null, InteractionTypes.ReputationChange, OutcomeStrings.Reputation.Fame_Dropped_Low);
        // }
    }

    // --- Optional Save/Load Stubs ---
    // private void SaveFameData() { /* Implement saving currentPlayerFame */ }
    // private void LoadFameData() { /* Implement loading currentPlayerFame */ }
}