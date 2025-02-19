using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEquipmentManager : MonoBehaviour
{
    [SerializeField] private LedgerManager ledgerManager;
    [SerializeField] private Transform characterModelSpawnPoint;
    [SerializeField] private GameObject[] equipmentSlots;
    [SerializeField] private GameObject characterSummary;
    [SerializeField] private GameObject characterName;
    [SerializeField] private GameObject characterClass;
    [SerializeField] private GameObject[] classStats;
    [SerializeField] private GameObject[] classAbilites;
    [SerializeField] private GameObject[] abilityDescriptions;

    private GameObject currentModel; 

    private void Awake()
    {
          if (ledgerManager == null)
        {
            ledgerManager = GetComponentInParent<LedgerManager>();
            if(ledgerManager == null)
            {
                Debug.LogError("CharacterEquipmentManager: LedgerManager not found in the parent!");
            }
        }
    }

    public void Initialize()
    {
        // TODO: If any head, body, feet, or weapon equipped -- show them in the equipment slots

        DisableItemOptions();
        InitializeCharacterModel();
        InitializeCharacterSummary();
    }

    public void EquipItem(BaseItem item)
    {
        DataManager.Instance.PlayerData.Equipment.Add(item);
        DataManager.Instance.PlayerData.Inventory.Remove(item); //Remove once equipped
        InitializeCharacterModel(); // Refresh model to show equipped item
    }

    public void UnequipItem(BaseItem item)
    {
        DataManager.Instance.PlayerData.Equipment.Remove(item);
        DataManager.Instance.PlayerData.Inventory.Add(item); //Re-add once unequipped
        InitializeCharacterModel(); // Refresh model to show unequipped item
    }

    //Creates and parents player model
    public void InitializeCharacterModel()
    {
        //Destroy existing model if it exists
        if (currentModel != null)
        {
            Destroy(currentModel); // Use Destroy, not GameObject.Destroy
            currentModel = null;
        }

        BasePlayer playerData = DataManager.Instance.PlayerData;

        //Instantiate player model
        currentModel = Instantiate(playerData.Model, characterModelSpawnPoint.position, Quaternion.Euler(new Vector3(0f, 180f, 0f)));
        currentModel.name = "ccClone";
        currentModel.AddComponent<RotateObj>();

        //Set model as child
        currentModel.transform.SetParent(characterModelSpawnPoint);
        if (currentModel.transform.localScale != new Vector3(200f, 200f, 200f))
        {
            currentModel.transform.localScale = new Vector3(200f, 200f, 200f);
        }

        //Set layer to UI
        foreach (Transform child in currentModel.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 5;
        }
    }

    private void DisableItemOptions()
    {
        foreach (GameObject slot in equipmentSlots)
        {
            slot.transform.Find("ItemOptions").gameObject.SetActive(false);
        }
    }

    private void InitializeCharacterSummary()
    {
        BaseClass c = DataManager.Instance.PlayerData.PlayerClass;

        characterName.GetComponent<TMP_Text>().text = DataManager.Instance.PlayerData.Name;
        characterClass.GetComponent<TMP_Text>().text = DataManager.Instance.PlayerData.PlayerClass.Name;
        
        //Set class stats
        for (int i = 0; i < c.Stats.Count; i++)
        {
            classStats[i].GetComponent<TMP_Text>().text = c.Stats[i].Name + ": " + c.Stats[i].BaseValue; 
        }

        //Set class abilities, using minus 1 to avoid out of bounds exception for extra ability that shouldn't be listed
        for (int i = 0; i < c.Abilities.Count - 1; i++)
        {
            Debug.Log("Setting ability " + i + ": " + c.Abilities[i].Name);
            classAbilites[i].GetComponent<TMP_Text>().text = c.Abilities[i].Name;
            abilityDescriptions[i].GetComponent<TMP_Text>().text = c.Abilities[i].Description;
            //abilityDescriptions[i].AddComponent<ToolTips>(); //Add comment later when not dead
        }
    }
}
