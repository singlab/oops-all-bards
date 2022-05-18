using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    private bool triggering;
    public int dialogueID;

    // Assigned DialogueTrigger starts dialogue from manager.
    public void Execute()
    {
        Debug.Log("Executing dialogue.");
        DialogueManager.Instance.StartDialogue(dialogueID);  
    }

    void Update()
    {
        if (triggering)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Execute();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            triggering = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            triggering = false;
        }
    }
}
