using UnityEngine;
using UnityEngine.SceneManagement;
using Viv;
using System.Collections;

public class CharacterSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the persistent manager exists and has characters to spawn
        if (SceneTransitionManager.Instance == null || SceneTransitionManager.Instance.charactersToCarryOver.Count == 0)
        {
            return;
        }

        Debug.Log("Character Spawner detected characters to carry over.");

        // Find the spawn point for this transition
        Transform spawnPoint = GameObject.FindWithTag(SceneTransitionManager.Instance.targetSpawnPointTag)?.transform;
        if (spawnPoint == null)
        {
            Debug.LogError($"Cannot find spawn point with tag: {SceneTransitionManager.Instance.targetSpawnPointTag}");
            return;
        }

        // Spawn and re-initialize each character
        foreach (var charState in SceneTransitionManager.Instance.charactersToCarryOver)
        {
            switch (charState.spawnMethod)
            {
                case SpawnMethod.Instant:
                    // Spawn immediately
                    SpawnCharacter(charState, spawnPoint.position, spawnPoint.rotation);
                    break;

                case SpawnMethod.Delayed:
                    // Start a coroutine to spawn after a delay
                    StartCoroutine(SpawnCharacterWithDelay(charState, spawnPoint.position, spawnPoint.rotation));
                    break;

                    // case SpawnMethod.FromCover:
                    // (Future logic to find a cover point and spawn there)
                    // break;
            }
        }

        // Clear the list so these characters don't get re-spawned on the next scene load
        SceneTransitionManager.Instance.charactersToCarryOver.Clear();
    }

    private void SpawnCharacter(PersistentCharacterState charState, Vector3 position, Quaternion rotation)
    {
        VivCharacter existingChar = Viv.Viv.Instance.FindVivCharacter(charState.characterId);
        if (existingChar != null)
        {
            Destroy(existingChar.gameObject);
        }

        GameObject prefab = Resources.Load<GameObject>(charState.prefabResourcePath);
        if (prefab != null)
        {
            GameObject newCharGO = Instantiate(prefab, position, rotation);
            VivCharacter newChar = newCharGO.GetComponent<VivCharacter>();
            newChar.InitializeFromState(charState);
            Debug.Log($"Spawned {charState.characterName} instantly.");
        }
    }

    private IEnumerator SpawnCharacterWithDelay(PersistentCharacterState charState, Vector3 position, Quaternion rotation)
    {
        Debug.Log($"Delaying spawn for {charState.characterName} by {charState.spawnDelay} seconds...");
        yield return new WaitForSeconds(charState.spawnDelay);
        SpawnCharacter(charState, position, rotation);
        Debug.Log($"{charState.characterName} spawned after delay.");
    }
}

public enum SpawnMethod
{
    Instant,        // Appears immediately at the spawn point (e.g., for an aggressive chase).
    Delayed,        // Appears at the spawn point after a short delay.
    FromCover       // (Advanced/Future) Appears at a nearby designated "cover" spot.
}