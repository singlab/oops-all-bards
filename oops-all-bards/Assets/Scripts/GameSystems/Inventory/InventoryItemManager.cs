using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemManager : MonoBehaviour
{
    // --- Configuration (Serialized Fields) ---

    [Tooltip("The main inventory UI panel.")]
    [SerializeField] private GameObject playerInventoryUI;

    [Tooltip("Prefab for an inventory slot.")]
    [SerializeField] private GameObject inventoryTilePrefab;

    [Tooltip("Container for inventory item slots.")]
    [SerializeField] private Transform inventoryItemsContainer;

    [Tooltip("Container for crafting recipe buttons.")]
    [SerializeField] private Transform recipeContainer;

    [Tooltip("Tooltip UI for displaying recipe information.")]
    [SerializeField] private GameObject recipeTooltip;

    [Tooltip("Prefab for the crafting recipe buttons.")]
    [SerializeField] private GameObject recipeButtonPrefab;

    [Tooltip("Text component to display the inventory count.")]
    [SerializeField] private TMP_Text inventoryCountText;

    [Tooltip("The tabs for switching between inventory sections (Inventory, Character, Crafts).")]
    [SerializeField] private GameObject[] tabs;

    [Tooltip("The tabs for switching between equipment slots (Weapon, Head, Body, Feet).")]
    [SerializeField] private GameObject[] equipableTabs;

    [Tooltip("Reference to the ModelViewer component.")]
    [SerializeField] private ModelViewer modelViewer; //  Consider if this dependency is necessary

    [Tooltip("Spawn point for the character model in the character inventory.")]
    [SerializeField] private Transform characterModelSpawnPoint;


    // --- Internal Data ---

    private InventoryTile[] inventorySlots;
    private CraftTooltip recipeTooltipComponent;  // Cache for easier access


    // --- Initialization ---

    private void Start()
    {
        // Initialize UI elements and grids
        InitializeInventoryGrid();
        InitializeCraftingUI();
        recipeTooltipComponent = recipeTooltip.GetComponent<CraftTooltip>(); // Get and store the component.

        // Initially hide UI elements
        playerInventoryUI.SetActive(false);
        SetTabsActive(false, tabs);         // Helper method for consistent tab handling
        SetTabsActive(false, equipableTabs);
        tabs[0].SetActive(true);  // Keep at least one tab active to avoid console error
        
        characterInventoryModel();
    }


    private void InitializeInventoryGrid()
    {
        inventorySlots = new InventoryTile[32]; //  Magic Number!  Consider a constant or serialized field

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            GameObject tileInstance = Instantiate(inventoryTilePrefab, inventoryItemsContainer);
            tileInstance.name = $"InventoryTile_{i}";  //  Helpful for debugging in the hierarchy
            inventorySlots[i] = tileInstance.GetComponent<InventoryTile>();
            inventorySlots[i].Initialize(this); // Pass the manager to the tile.  Cleaner than a public field.

            // Connect the InventoryTile's events to methods in the InventoryItemManager.
            inventorySlots[i].OnUse.AddListener(UseItem);
            inventorySlots[i].OnEquip.AddListener(EquipItem);
            inventorySlots[i].OnUnequip.AddListener(UnequipItem);
            inventorySlots[i].OnTrash.AddListener(TrashItem);
        }
        UpdateInventoryCount();
    }

     // update the inventory to display the correct items
    public void UpdateInventory()
    {
        List<BaseItem> playerInventory = DataManager.Instance.PlayerData.Inventory;

        // Populate slots with items.
        for (int i = 0; i < playerInventory.Count; i++)
        {
            inventorySlots[i].SetItem(playerInventory[i]);
             inventorySlots[i].UpdateFrameColor();
        }

        // Clear remaining slots.
        for (int i = playerInventory.Count; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].ClearItem();
        }

        UpdateInventoryCount();
        DeselectAllTiles(); // Clear any lingering selections
    }

    // These methods are now *event handlers*. They receive the BaseItem from the event.
    public void UseItem(BaseItem item)
    {
        if (item.Type is BaseItem.ItemTypes.POTABLES) //  Consider a switch statement for more types
        {
            // potable.UseEffect(player); //  You'd call the item's specific use effect here.
            // ^^^ This is where your game-specific logic goes! ^^^
            DataManager.Instance.PlayerData.Inventory.Remove(item);
            UpdateInventory();
        }
    }
    public void EquipItem(BaseItem item)
    {
        if (item.Type is BaseItem.ItemTypes.DUDS || item.Type is BaseItem.ItemTypes.INSTRUMENTS) //  Consider a switch statement for more types
        {
            // TODO: Implement equipment logic
             DataManager.Instance.PlayerData.Equipment.Add(item); //Add to equipment
             UpdateInventory(); // Refresh the inventory UI
        }
    }
     public void UnequipItem(BaseItem item)
    {
        if (item.Type is BaseItem.ItemTypes.DUDS || item.Type is BaseItem.ItemTypes.INSTRUMENTS)
        {
            DataManager.Instance.PlayerData.Equipment.Remove(item); //Remove from equipment
            UpdateInventory(); // Refresh the inventory UI
        }

    }

    public void TrashItem(BaseItem item)
    {
        // Implement trash/destroy item logic.
        DataManager.Instance.PlayerData.Inventory.Remove(item); // Remove from the player's inventory
        UpdateInventory(); // Update the UI
    }

    private void InitializeCraftingUI()
    {
        //Currently only initializes the tooltip.  Crafting grid created dynamically.
        recipeTooltip.SetActive(false);
    }

    // --- Input Handling ---

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !DialogueManager.Instance.dialogueUI.activeInHierarchy)
        {
            ToggleInventoryUI();
        }
    }

    // --- UI Control ---

    public void ToggleInventoryUI()
    {
        bool newState = !playerInventoryUI.activeSelf;
        playerInventoryUI.SetActive(newState);

        if (newState)
        {
            Cursor.lockState = CursorLockMode.Confined;
            GameManager.Instance.StartCoroutine(GameManager.togglePlayerPause()); // Consider if a coroutine is really necessary here
            SwitchTab(tabs[0]); // Default to the first tab.
            UpdateInventory();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            SetTabsActive(false, equipableTabs);
            GameManager.Instance.TogglePlayerControls();  //  Direct call is likely fine.
        }
    }


    public void SwitchTab(GameObject tab)
    {
        // Deactivate all tabs, then activate the selected one.
        SetTabsActive(false, tabs);
        tab.SetActive(true);
    }

    public void SwitchEquipTab(GameObject equipTab)
    {
        SetTabsActive(false, equipableTabs);
        equipTab.SetActive(true);
    }


    // Helper method to activate/deactivate tabs
    private void SetTabsActive(bool active, GameObject[] tabArray)
    {
        foreach (GameObject tab in tabArray)
        {
            tab.SetActive(active);
        }
    }

    // --- Inventory Display & Update ---

    private void UpdateInventoryCount()
    {
        inventoryCountText.text = $"{DataManager.Instance.PlayerData.Inventory.Count}/{inventorySlots.Length}";
    }

    public void DeselectAllTiles()
    {
        foreach (InventoryTile tile in inventorySlots)
        {
            tile.OnDeselect();
        }
    }

    // --- Crafting System ---
    public void UpdateCraftingTab()
    {
        // Clear existing recipe buttons.
        foreach (Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }

        //  LINQ for conciseness and readability.
        var availableRecipes = ItemData.items
            .Where(item => item.Recipe != null && item.Recipe.All(ingredient => DataManager.Instance.PlayerData.Inventory.Contains(ingredient)))
            .ToList(); //  Create a list to avoid multiple enumerations

        foreach (BaseItem item in availableRecipes)
        {
            GameObject buttonInstance = Instantiate(recipeButtonPrefab, recipeContainer);
            CraftItemButton buttonData = buttonInstance.GetComponent<CraftItemButton>();
            buttonData.Initialize(item, this);  // Initialize method for cleaner setup.
        }

        recipeTooltip.SetActive(false); // Ensure tooltip is hidden initially.
    }
    
    public void UpdateCraftingTooltip(BaseItem item)
    {
        recipeTooltip.SetActive(true);

        //  Use the cached component.
        recipeTooltipComponent.image.sprite = item.Icon;
        string recipeText = $"To Craft: {item.Description}\n\n";
        recipeText += string.Join(", ", item.Recipe.Select(recipeItem => recipeItem.DisplayName)); //  Concise ingredient list
        recipeTooltipComponent.text.text = recipeText;

        recipeTooltipComponent.Initialize(item, this); // Use consistent initialization
    }
    public void characterInventoryModel()
    {
        GameObject playerModel = null;
        BasePlayer playerData = DataManager.Instance.PlayerData;

        //repurpose this so that it's not actually using the player and instead just the model
        playerModel = Instantiate(playerData.Model, characterModelSpawnPoint.position, Quaternion.Euler(new Vector3(0f, 180f, 0f)));
        playerModel.name = "ccClone";
        playerModel.AddComponent<RotateObj>();

        //Find the child of Canvas called "Panel" and so on
        Transform inventoryUIPanel = GameObject.Find("Canvas").transform.Find("Panel").transform.Find("InventoryUI");

        //Validate the path, only proceed if panel is found
        if (inventoryUIPanel == null)
        {
                Debug.LogError("Inventory UI panel not found!");
                return;
        }

        //Make the object a child of the container
        //More robust path finding, less likely to break on UI changes
        Transform modelContainer = inventoryUIPanel.Find("Tab2BackShadow/CharacterInventoryContainer/CharacterModelContainer");
        if (modelContainer == null)
        {
            Debug.LogError("Character model container not found!");
            return; //Prevent further execution if container is missing
        }
        playerModel.transform.SetParent(modelContainer);

        foreach (Transform child in playerModel.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = 5; //layer 5 literally just means it's in the ui layer
        }
        
        RenderModel(playerModel); //May want to remove
    }

    public void RenderModel(GameObject model) //May want to remove
    {
        if (transform.childCount > 0)
        {
            GameObject currentChild = transform.GetChild(0).gameObject;
            Destroy(currentChild);
        }
    }
}
