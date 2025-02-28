using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfrontPlayerAction : IABLAction
{
    public void Execute(ActionData data)
    {
        int characterId = data.actingCharacter;
        GameObject confrontingCharacter = null;
        // TODO: Find confrontingCharacter through characterId
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (confrontingCharacter != null && player != null)
        {
            // - Move the confrontingCharacter towards the player:
            //   confrontingCharacter.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
            // - Play an animation:
            //   confrontingCharacter.GetComponent<Animator>().SetTrigger("Confront");
            // - Start a dialogue:
            //   DialogueManager.Instance.StartDialogue(confrontingCharacter, player);
            // - Potentially transition to combat:
            //    if (dialogueOutcome == DialogueOutcome.Fight) {
            //        SceneManager.LoadScene("CombatScene");
            //    }
            Debug.Log("ConfrontPlayerAction: Wurguth (ID " + characterId + ") is confronting the player.");
        }
        else
        {
            Debug.LogError("ConfrontPlayerAction: Could not find character or player.");
        }
    }
}
