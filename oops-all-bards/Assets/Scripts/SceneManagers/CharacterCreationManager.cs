using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterCreationManager : MonoBehaviour
{
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    public ModelViewer modelViewer;
    public ClassViewer classViewer;
    public TMP_InputField nameInputField;
    private int currentStep = 0;
    private BasePlayer playerData = new BasePlayer();

    // Start is called before the first frame update
    void Start()
    {
        ContinueToNextStep();
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
            SetPlayerModel(modelViewer.GetModel());
            ToggleGameObject(step1);
            ToggleGameObject(step2);
        } else if (currentStep == 3)
        {
            SetPlayerClass(classViewer.GetClass());
            ToggleGameObject(step2);
            ToggleGameObject(step3);
        } else
        {
            SetPlayerName(nameInputField.text);
            ExitCharacterCreationScene();
        }
    }

    // Toggle a gameobject's visibility.
    void ToggleGameObject(GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }

    // Save all player data and exit character creation scene.
    void ExitCharacterCreationScene()
    {
        FinalizePlayerData(playerData.PlayerClass);
        DataManager.Instance.PlayerData = playerData;
        DataManager.Instance.SavePlayerData();
        SceneManager.LoadScene("TavernDemo");
    }

    void SetPlayerModel(GameObject model)
    {
        playerData.Model = model;
    }

    void SetPlayerClass(BaseClass playerClass)
    {
        playerData.PlayerClass = playerClass;
    }

    void SetPlayerName(string name)
    {
        playerData.Name = name;
    }

    void FinalizePlayerData(BaseClass playerClass)
    {
        playerData.Health = 15;
        playerData.Flourish = 10;
    }
}
