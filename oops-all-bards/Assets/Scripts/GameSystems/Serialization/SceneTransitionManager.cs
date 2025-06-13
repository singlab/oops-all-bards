using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Viv;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    public List<PersistentCharacterState> charactersToCarryOver = new List<PersistentCharacterState>();
    public string targetSpawnPointTag;

    // A reference to the fader component
    private BlackFade fader;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Find the fader when a new scene loads
    private void OnEnable() { SceneManager.sceneLoaded += FindFader; }
    private void OnDisable() { SceneManager.sceneLoaded -= FindFader; }

    private void FindFader(Scene scene, LoadSceneMode mode)
    {
        fader = FindObjectOfType<BlackFade>();
        if (fader == null)
        {
            Debug.LogError("SceneTransitionManager could not find a BlackFade component in the scene.");
        }
    }

    public void CarryCharacterToNextScene(VivCharacter character, SpawnMethod method, float delay = 0f)
    {
        if (!charactersToCarryOver.Exists(c => c.characterId == character.characterID))
        {
            Debug.Log($"Carrying character {character.characterName} (ID: {character.characterID}) to next scene with method: {method}, delay: {delay}");
            
            charactersToCarryOver.Add(new PersistentCharacterState(character, method, delay));
        }
    }

    public void TransitionToScene(string sceneName, string spawnPointTag)
    {
        if (fader == null)
        {
            Debug.LogError("Cannot transition scene: BlackFade component not found!");
            return;
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Cannot transition to an empty scene name.");
            return;
        }

        if (string.IsNullOrEmpty(spawnPointTag))
        {
            Debug.Log("Transitioning to scene without a specific spawn point tag.");
            fader.FadeToLevel(sceneName);
        }
        else
        {
            this.targetSpawnPointTag = spawnPointTag;
            Debug.Log($"Transitioning to scene: {sceneName} with spawn point tag: {spawnPointTag}");
            fader.FadeToLevel(sceneName);
        }
    }
}