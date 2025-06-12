using UnityEngine;
using Viv;

public class VivSceneTransitionTrigger : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnPointTag;

    private void OnTriggerEnter(Collider other)
    {
        // Ensure it's the player entering the trigger
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("Player entered scene transition trigger. Checking for followers...");

        // Find Wurguth in the current scene (ID 2)
        VivCharacter wurguth = Viv.Viv.Instance.FindVivCharacter(2);

        if (wurguth != null)
        {
            // Check Wurguth's current intent
            if (wurguth.IsRunningBehavior("investigateSuspiciousActivity"))
            {
                // If he's investigating, carry him over with a 5-second delay so he can be covert
                SceneTransitionManager.Instance.CarryCharacterToNextScene(wurguth, SpawnMethod.Delayed, 5.0f);
            }
            else if (wurguth.IsRunningBehavior("neutralizeThreat"))
            {
                // If he's in aggressive pursuit, carry him over instantly
                SceneTransitionManager.Instance.CarryCharacterToNextScene(wurguth, SpawnMethod.Instant);
            }
            else
            {
                // If he's not in a specific behavior, carry him over with a default method
                SceneTransitionManager.Instance.CarryCharacterToNextScene(wurguth, SpawnMethod.Instant);
            }
        }

        // TODO: Handle other characters similarly if needed

        // Trigger the scene transition
        Debug.Log($"Triggering scene transition to {targetSceneName} at spawn point {targetSpawnPointTag}.");
        SceneTransitionManager.Instance.TransitionToScene(targetSceneName, targetSpawnPointTag);
    }
}