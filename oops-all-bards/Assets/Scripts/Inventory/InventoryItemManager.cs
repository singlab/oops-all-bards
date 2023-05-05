using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemManager : MonoBehaviour
{
    //private static InventoryItemManager _instance;
    //public static InventoryItemManager Instance => InventoryItemManager._instance;

    [SerializeField] private GameObject[] tabs;
    [SerializeField] private GameObject[] equipableTabs; //test

    public GameObject playerInventoryUI;
    public GameObject inventoryTile;
    public GameObject inventoryItemsContainer;
    public GameObject ingredientsContainer;
    public GameObject recipiesContainer;

    public GameObject invNum;

    public GameObject[] invSpaces;
    public GameObject[] ingredientSpaces;
    public GameObject[] recipeSpaces;

    public ModelViewer modelViewer;
    public GameObject ccSpawn;

    void Awake()
    {
        /*if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);*/
    }

    // Start is called before the first frame update
    void Start()
    {
        // Persisting item manmager sets these variables in inspector and are lost on scene switch ...
        // ... there is 100% a better way to do this but this will do for now
        // Even lazier fix how about lets just not make this a singleton?
        /*invNum = GameObject.Find("InvSizeCounter");
        playerInventoryUI = GameObject.Find("InventoryUI"); 
        inventoryItemsContainer = GameObject.Find("InventoryItems");
        ingredientsContainer = GameObject.Find("IngredientsContainer");
        recipiesContainer = GameObject.Find("RecipiesContainer");
        tabs[0] = GameObject.Find("InventoryContainer");
        tabs[1] = GameObject.Find("CharacterInventoryContainer");
        tabs[2] = GameObject.Find("CraftsInventoryContainer");
        equipableTabs[0] = GameObject.Find("WeaponGearUI");
        equipableTabs[1] = GameObject.Find("HeadGearUI");
        equipableTabs[2] = GameObject.Find("BodyGearUI");
        equipableTabs[3] = GameObject.Find("FeetGearUI");*/

        //Fill in item inventory with empty inventory slots
        fillInventoryGrid();
        inventoryNumber();

        characterInventoryModel();

        fillCraftingRecipiesGrid();
        fillCraftingIngredientsGrid();

        playerInventoryUI.SetActive(false);
        tabs[1].SetActive(false);
        tabs[2].SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) //test key
        {
            toggleInventoryUI();
            if (playerInventoryUI.activeSelf) UpdateInventory();
        }
    }

    public void tabSwitch(GameObject tab)
    {
        tab.SetActive(true);

        //Check through the available tabs in the list and only set active the one that matches with gameobject that was passed in
        for(int i = 0; i < tabs.Length; i++)
        {
            if (tabs[i] != tab)
            {
                tabs[i].SetActive(false);
            }
        }
    }

    //Same thing for the primary tabs, but done for the equipables //TEST
    public void equipSwitch(GameObject equipTab)
    {
        equipTab.SetActive(true);

        //Check through the available tabs in the list and only set active the one that matches with gameobject that was passed in
        for (int i = 0; i < equipableTabs.Length; i++)
        {
            if (equipableTabs[i] != equipTab)
            {
                equipableTabs[i].SetActive(false);
            }
        }
    }

    public void fillInventoryGrid()
    {
        //Set size of inventory
        invSpaces = new GameObject[32];

        for (int i = 0; i < invSpaces.Length; i++)
        {
            //Fill the inventory with spaces for items to be stored
            GameObject toInstantiate = Instantiate(inventoryTile, inventoryItemsContainer.transform.position, Quaternion.identity);
            invSpaces[i] = toInstantiate;
            toInstantiate.transform.SetParent(inventoryItemsContainer.transform, false); 
        }
    }
    /////////////////

    public void characterInventoryModel()
    {        

        GameObject playerModel = null;
        BasePlayer playerData = DataManager.Instance.PlayerData;

        //repurpose this so that it's not actually using the player and instead just the model
        playerModel = Instantiate(playerData.Model, ccSpawn.transform.position, Quaternion.Euler(new Vector3(0f, 180f, 0f)));
        playerModel.name = "ccClone";
        playerModel.AddComponent<RotateObj>();


        //Make the object a child of the container
        //A bit crude but it works for now
        playerModel.transform.SetParent(GameObject.Find("Canvas").transform.Find("Panel").transform.Find("InventoryUI").transform.Find("Tab2BackShadow").transform.Find("CharacterInventoryContainer").transform.Find("CharacterModelContainer"));

        if (GameObject.Find("Canvas").transform.Find("Panel").transform.Find("InventoryUI").transform.Find("Tab2BackShadow").transform.Find("CharacterInventoryContainer").transform.Find("CharacterModelContainer") == null)
        {
            Debug.Log("no luck");
        }

        foreach (Transform child in playerModel.transform.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 5; //layer 5 literally just means it's in the ui layer
        }

        RenderModel(playerModel);
    }

    public void RenderModel(GameObject model)
    {
        if (transform.childCount > 0)
        {
            GameObject currentChild = transform.GetChild(0).gameObject;
            Destroy(currentChild);
        }
    }


    /////////////////
    public void fillCraftingIngredientsGrid()
    {
        //Set size of inventory
        ingredientSpaces = new GameObject[5];

        //Fill ingredients section with empty boxes
        for (int i = 0; i < ingredientSpaces.Length; i++)
        {
            //Fill the inventory with spaces for items to be stored
            GameObject toInstantiate = Instantiate(inventoryTile, ingredientsContainer.transform.position, Quaternion.identity);
            //make size smaller to fit into ui layout
            toInstantiate.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            ingredientSpaces[i] = toInstantiate;
            toInstantiate.transform.SetParent(ingredientsContainer.transform, false); //test
        }

        
    }

    public void fillCraftingRecipiesGrid()
    {

        //Set size of inventory
        recipeSpaces = new GameObject[5];

        //Fill ingredients section with empty boxes
        for (int i = 0; i < recipeSpaces.Length; i++)
        {
            //Fill the inventory with spaces for items to be stored
            GameObject toInstantiate = Instantiate(inventoryTile, recipiesContainer.transform.position, Quaternion.identity);
            //make size smaller to fit into ui layout
            toInstantiate.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            toInstantiate.AddComponent<ToolTips>();

            //toInstantiate.AddComponent<TMP_Text>().text = "j"; //fix later

            recipeSpaces[i] = toInstantiate;
            toInstantiate.transform.SetParent(recipiesContainer.transform, false);
        }

    }

    /////////////////

    public void toggleInventoryUI()
    {
        playerInventoryUI.SetActive(!playerInventoryUI.activeSelf);

        //Lock movement and camera while in menu
        if (playerInventoryUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Confined;
            GameManager.Instance.StartCoroutine(GameManager.togglePlayerPause());
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            for (int i = 0; i < equipableTabs.Length; i++) //closes equipables screen when inventory is not enabled
            {
                equipableTabs[i].SetActive(false);

            }
            GameManager.Instance.TogglePlayerControls();
        }
    }

    public void inventoryNumber()
    {
        //used to manipulate inventory counter in bottom right cornerw
        invNum.GetComponent<TMP_Text>().text = $"{DataManager.Instance.PlayerData.Inventory.Count.ToString()}/{invSpaces.Length}";

    }

    public void UpdateInventory()
    {
        for (int i = 0; i < DataManager.Instance.PlayerData.Inventory.Count; i++)
        {
            Debug.Log(DataManager.Instance.PlayerData.Inventory[i].DisplayName);
            invSpaces[i].transform.Find("Frame").transform.Find("Background").GetComponent<Image>().sprite = DataManager.Instance.PlayerData.Inventory[i].Icon;
        }
        inventoryNumber();
    }
}
