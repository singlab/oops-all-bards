using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationManager : MonoBehaviour
{
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    private int currentStep = 0;
    private BasePlayer playerData = new BasePlayer();

    // Start is called before the first frame update
    void Start()
    {
        ContinueToNextStep();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Continue to the next step on button click.
    public void ContinueToNextStep()
    {
        currentStep++;
        if (currentStep == 1)
        {
            ToggleGameObject(step2);
            ToggleGameObject(step3);
        } else if (currentStep == 2)
        {
            ToggleGameObject(step1);
            ToggleGameObject(step2);
        } else if (currentStep == 3)
        {
            ToggleGameObject(step2);
            ToggleGameObject(step3);
        } else
        {
            // ExitCharacterCreationScene();
        }
    }

    // Toggle a gameobject's visibility.
    void ToggleGameObject(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

    // Save all player data and exit character creation scene.

}
