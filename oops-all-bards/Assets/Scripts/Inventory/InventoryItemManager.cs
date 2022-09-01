using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemManager : MonoBehaviour
{
    private static InventoryItemManager _instance;
    public static InventoryItemManager Instance => InventoryItemManager._instance;

    public GameObject playerInventoryUI;
    public GameObject inventoryTile;
    public GameObject inventoryItemsContainer;
    public RectTransform containerRT;

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
        fillInventoryGrid();
    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) //test key
        {
            toggleInventoryUI();
        }
    }

    public void fillInventoryGrid()
    {
        //Check if null
        if (inventoryItemsContainer == null)
        {
            inventoryItemsContainer = GameObject.Find("InventoryItems");
        }

        containerRT = inventoryItemsContainer.GetComponent<RectTransform>();

        //Maybe inventory should fill as you go? cause this just breaks the unity
        //for (int x = 0; x < containerRT.rect.width; x++)
        //{
           // for(int y = 0; y < containerRT.rect.height; y++)
           // {

               //Fill the inventory with spaces for items to be stored
               GameObject toInstantiate = Instantiate(inventoryTile, inventoryItemsContainer.transform.position, Quaternion.identity);
               toInstantiate.transform.SetParent(inventoryItemsContainer.transform, true);

           // }

        //}
    }

    public void toggleInventoryUI()
    {
        playerInventoryUI.SetActive(!playerInventoryUI.activeSelf);

        //lock movement and camera while in menu
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
}
