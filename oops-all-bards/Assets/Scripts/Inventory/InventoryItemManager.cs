using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItemManager : MonoBehaviour
{
    private static InventoryItemManager _instance;
    public static InventoryItemManager Instance => InventoryItemManager._instance;

    [SerializeField] private GameObject[] tabs;

    public GameObject playerInventoryUI;
    public GameObject inventoryTile;
    public GameObject inventoryItemsContainer;
    public GameObject ingredientsContainer;
    public GameObject recipiesContainer;

    public GameObject invNum;

    public GameObject[] invSpaces;
    public GameObject[] ingredientSpaces;
    public GameObject[] recipeSpaces;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //Fill in item inventory with empty inventory slots
        fillInventoryGrid();
        inventoryNumber();


        fillCraftingRecipiesGrid();
        fillCraftingIngredientsGrid();
        
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) //test key
        {
            toggleInventoryUI();
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

    public void fillInventoryGrid()
    {
        //Set size of inventory
        invSpaces = new GameObject[32];

        //Check if null
        if (inventoryItemsContainer == null)
        {
            inventoryItemsContainer = GameObject.Find("InventoryItems");
        }

        for (int i = 0; i < invSpaces.Length; i++)
        {
            //Fill the inventory with spaces for items to be stored
            GameObject toInstantiate = Instantiate(inventoryTile, inventoryItemsContainer.transform.position, Quaternion.identity);
            invSpaces[i] = toInstantiate;
            toInstantiate.transform.SetParent(inventoryItemsContainer.transform, true);

        }
    }

    public void fillCraftingIngredientsGrid()
    {
        
        //Set size of inventory
        ingredientSpaces = new GameObject[5];

        //Check if null
        if (ingredientsContainer == null)
        {
            ingredientsContainer = GameObject.Find("IngredientsContainer");
        }

        //Fill ingredients section with empty boxes
        for (int i = 0; i < ingredientSpaces.Length; i++)
        {
            //Fill the inventory with spaces for items to be stored
            GameObject toInstantiate = Instantiate(inventoryTile, ingredientsContainer.transform.position, Quaternion.identity);
            //make size smaller to fit into ui layout
            toInstantiate.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            ingredientSpaces[i] = toInstantiate;
            toInstantiate.transform.SetParent(ingredientsContainer.transform, true);
        }

        
    }

    public void fillCraftingRecipiesGrid()
    {

        //Set size of inventory
        recipeSpaces = new GameObject[5];

        //Check if null
        if (recipiesContainer == null)
        {
            recipiesContainer = GameObject.Find("RecipiesContainer");
        }

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
            toInstantiate.transform.SetParent(recipiesContainer.transform, true);
        }

    }


    public void toggleInventoryUI()
    {
        playerInventoryUI.SetActive(!playerInventoryUI.activeSelf);

        //Lock movement and camera while in menu
        if (playerInventoryUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Confined;
            DemoManager.Instance.StartCoroutine(DemoManager.togglePlayerPause());
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            DemoManager.Instance.TogglePlayerControls();
        }
    }

    public void inventoryNumber()
    {
        invNum.GetComponent<TMP_Text>().text = "0/" + invSpaces.Length;

    }
}
