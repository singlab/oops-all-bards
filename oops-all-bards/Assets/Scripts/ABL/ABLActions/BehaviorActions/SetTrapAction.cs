using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrapAction : IABLAction
{
    public void Execute(ActionData data)
    {
        int characterId = data.actingCharacter;
        string trapLocationName = data.location;

        // TODO: Find trappingCharacter and trapLocation through characterId and trapLocationName
        GameObject trappingCharacter = null;
        GameObject trapLocation = null;


        if (trappingCharacter != null && trapLocation != null)
        {
            // Implement trap setting logic. Examples:
            // - Move the character to the location:
            //   trappingCharacter.GetComponent<NavMeshAgent>().SetDestination(trapLocation.transform.position);
            // - Play an animation:
            //   trappingCharacter.GetComponent<Animator>().SetTrigger("SetTrap");
            // - Instantiate a trap prefab at the location:
            //   GameObject trap = GameObject.Instantiate(trapPrefab, trapLocation.transform.position, Quaternion.identity);
            // - Add a fact to Wurguth's DELPEntity:
            //  Viv.Viv.Instance.FindCharacterDELPEntity(characterID).AddFact("trapSet(wurguth, " + trapLocationName + ")");
            // - Disable the trap location so another trap can't be set:
            //   trapLocation.SetActive(false);

             Debug.Log("SetTrapAction: Wurguth (ID " + characterId + ") is setting a trap at " + trapLocationName);
        }
        else
        {
            Debug.LogError("SetTrapAction: Could not find character or trap location.");
        }
    }
}
