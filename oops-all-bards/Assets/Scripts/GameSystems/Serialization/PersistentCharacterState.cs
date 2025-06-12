using System.Collections.Generic;
using UnityEngine;
using Viv;

[System.Serializable]
public class PersistentCharacterState
{
    public int characterId;
    public string characterName;
    public string prefabResourcePath;

    // Positional State
    public Vector3 lastPosition;
    public Quaternion lastRotation;

    // Viv State
    public string activeSupertaskName;
    public List<string> runtimeFacts; // Only the facts added at runtime

    // Spawn State
    public SpawnMethod spawnMethod;
    public float spawnDelay;

    public PersistentCharacterState(VivCharacter character, SpawnMethod method, float delay = 0f)
    {
        this.characterId = character.characterID;
        this.characterName = character.characterName;
        this.prefabResourcePath = character.prefabPath;
        this.lastPosition = character.transform.position;
        this.lastRotation = character.transform.rotation;
        this.activeSupertaskName = character.CurrentSupertask?.Name;
        this.spawnMethod = method;
        this.spawnDelay = delay;

        // Only save the dynamic facts, not the base rules
        runtimeFacts = new List<string>(character.delpEntity.Facts);
    }
}