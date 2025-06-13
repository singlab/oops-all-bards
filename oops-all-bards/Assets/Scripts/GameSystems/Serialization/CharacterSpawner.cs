using UnityEngine;
using UnityEngine.SceneManagement;
using Viv;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using Viv;
using System.Collections;
using System.Collections.Generic;

public class CharacterSpawner : MonoBehaviour
{
    // A small helper class to temporarily hold a character and its state
    private class SpawnedCharacterInfo
    {
        public VivCharacter CharacterInstance;
        public PersistentCharacterState CharacterState;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LaunchSpawnerCoroutine;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LaunchSpawnerCoroutine;
    }

    void LaunchSpawnerCoroutine(Scene scene, LoadSceneMode mode)
    {
        if (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.charactersToCarryOver.Count > 0)
        {
            StartCoroutine(OnSceneLoadedCoroutine(scene, mode));
        }
    }

    private IEnumerator OnSceneLoadedCoroutine(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Character Spawner detected characters to carry over.");

        // Find the spawn point for this transition
        Transform spawnPoint = GameObject.FindWithTag(SceneTransitionManager.Instance.targetSpawnPointTag)?.transform;
        if (spawnPoint == null)
        {
            Debug.LogError($"Cannot find spawn point with tag: {SceneTransitionManager.Instance.targetSpawnPointTag}");
            yield break; // Use yield break to exit a coroutine
        }

        var spawnedCharacterInfos = new List<SpawnedCharacterInfo>();
        var characterStatesToProcess = new List<PersistentCharacterState>(SceneTransitionManager.Instance.charactersToCarryOver);

        SceneTransitionManager.Instance.charactersToCarryOver.Clear();

        foreach (var charState in characterStatesToProcess)
        {
            VivCharacter existingChar = Viv.Viv.Instance.FindVivCharacter(charState.characterId);
            if (existingChar != null)
            {
                Destroy(existingChar.gameObject);
            }

            // Load the prefab from the Resources folder
            GameObject prefab = Resources.Load<GameObject>(charState.prefabResourcePath);
            if (prefab == null)
            {
                Debug.LogError($"Failed to load character prefab from path: {charState.prefabResourcePath}");
                continue;
            }

            GameObject newCharGO = null;
            VivCharacter newChar = null;

            switch (charState.spawnMethod)
            {
                case SpawnMethod.Instant:
                    newCharGO = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                    Debug.Log($"Instantiated {charState.characterName} instantly.");
                    break;

                case SpawnMethod.Delayed:
                    newCharGO = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                    newCharGO.SetActive(false); // Start disabled
                    StartCoroutine(EnableCharacterAfterDelay(newCharGO, charState.spawnDelay, charState.characterName));
                    Debug.Log($"Instantiated {charState.characterName} (inactive, pending delay).");
                    break;
            }

            if (newCharGO != null)
            {
                newChar = newCharGO.GetComponent<VivCharacter>();
                if (newChar != null)
                {
                    spawnedCharacterInfos.Add(new SpawnedCharacterInfo { CharacterInstance = newChar, CharacterState = charState });
                }
                else
                {
                    Debug.LogError($"Prefab at {charState.prefabResourcePath} does not have a VivCharacter component.");
                }
            }
        }

        yield return new WaitForEndOfFrame();

        Debug.Log("Initializing state for newly spawned characters...");
        foreach (var info in spawnedCharacterInfos)
        {
            info.CharacterInstance.InitializeFromState(info.CharacterState);
        }

        Debug.Log("Character spawning and initialization complete.");
    }

    // Coroutine to handle delayed activation
    private IEnumerator EnableCharacterAfterDelay(GameObject characterObject, float delay, string characterName)
    {
        yield return new WaitForSeconds(delay);
        if (characterObject != null)
        {
            characterObject.SetActive(true);
            Debug.Log($"{characterName} activated after delay.");
        }
    }
}

public enum SpawnMethod
{
    Instant,        // Appears immediately at the spawn point (e.g., for an aggressive chase).
    Delayed,        // Appears at the spawn point after a short delay.
    FromCover       // (Advanced/Future) Appears at a nearby designated "cover" spot.
}