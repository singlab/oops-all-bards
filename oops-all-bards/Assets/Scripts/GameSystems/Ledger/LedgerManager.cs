// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class LedgerManager : MonoBehaviour
// {
//     [Tooltip("The main inventory UI panel.")]
//     [SerializeField] private GameObject ledgerUI;

//     [Tooltip("The tabs for switching between inventory sections (Inventory, Character, Crafts).")]
//     [SerializeField] private Button[] tabs; // Use Buttons instead of GameObjects

//     // Sub-Manager Content GameObjects (to show/hide)
//     [SerializeField] private GameObject inventoryContent;
//     [SerializeField] private GameObject characterContent;
//     [SerializeField] private GameObject craftingContent;


//     // Sub-Managers  Make these serialized so you can assign them in the Inspector.
//     [SerializeField] public InventoryItemManager inventoryManager;
//     [SerializeField] public CraftingManager craftingManager;
//     [SerializeField] public CharacterEquipmentManager equipmentManager;

//     private void Awake()
//     {
//         // No longer need to instantiate managers here.
//     }

//     private void Start()
//     {
//         // Initialize all sub-managers
//         inventoryManager.Initialize();
//         craftingManager.Initialize();
//         equipmentManager.Initialize();

//         // Initially hide UI elements and set up tab buttons.
//         ledgerUI.SetActive(false);
//         InitializeTabs(); // Set up button listeners and initial state.
//     }

//     // --- Input Handling ---
//     private void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.I) && !DialogueManager.Instance.dialogueUI.activeInHierarchy)
//         {
//             ToggleLedgerUI();
//         }
//     }

//     // --- UI Control ---
//     public void ToggleLedgerUI()
//     {
//         bool newState = !ledgerUI.activeSelf;
//         ledgerUI.SetActive(newState);

//         if (newState)
//         {
//             Cursor.lockState = CursorLockMode.Confined;
//             GameManager.Instance.StartCoroutine(GameManager.togglePlayerPause());
//             SwitchTab(0); // Default to inventory (first tab).  Use index.
//             inventoryManager.UpdateInventory(); // Initialize inventory on open.
//         }
//         else
//         {
//             Cursor.lockState = CursorLockMode.Locked;
//             GameManager.Instance.TogglePlayerControls();
//         }
//     }

//     // Sets up tab button listeners and initial tab state.
//     private void InitializeTabs()
//     {
//         for (int i = 0; i < tabs.Length; i++)
//         {
//             int tabIndex = i; // Capture the index in a local variable.
//             tabs[i].onClick.AddListener(() => SwitchTab(tabIndex));
//         }

//         // Initially hide all content.
//         inventoryContent.SetActive(false);
//         characterContent.SetActive(false);
//         craftingContent.SetActive(false);

//         // Show the initial tab content (usually the inventory).
//         SwitchTab(0); // Start with the first tab (inventory).
//     }

//     // Switch to a tab by its index.
//     public void SwitchTab(int tabIndex)
//     {
//         // Hide all content.
//         inventoryContent.SetActive(false);
//         characterContent.SetActive(false);
//         craftingContent.SetActive(false);

//         // Show the selected tab's content.
//         if (tabIndex == 0)
//         {
//             inventoryContent.SetActive(true);
//             inventoryManager.UpdateInventory();
//         }
//         else if (tabIndex == 1)
//         {
//             characterContent.SetActive(true);
//             equipmentManager.InitializeCharacterModel();
//         }
//         else if (tabIndex == 2)
//         {
//             craftingContent.SetActive(true);
//             craftingManager.UpdateCraftingTab();
//         }

//         // Optionally, you might want to highlight the selected tab button here.
//         // This would involve changing the button's colors or adding a visual indicator.
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LedgerManager : MonoBehaviour
{
    [Tooltip("The main inventory UI panel.")]
    [SerializeField] private GameObject ledgerUI;

    [Tooltip("The tabs for switching between inventory sections (Inventory, Character, Crafts).")]
    [SerializeField] private Button[] tabs; // Use Buttons instead of GameObjects

    // Sub-Manager Content GameObjects (to show/hide)
    [SerializeField] private GameObject inventoryContent;
    [SerializeField] private GameObject characterContent;
    [SerializeField] private GameObject craftingContent;


    // Sub-Managers  Make these serialized so you can assign them in the Inspector.
    [SerializeField] public InventoryItemManager inventoryManager;
    [SerializeField] public CraftingManager craftingManager;
    [SerializeField] public CharacterEquipmentManager equipmentManager;

    private void Start()
    {
        // Initialize all sub-managers
        inventoryManager.Initialize();
        craftingManager.Initialize();
        equipmentManager.Initialize();

        // Initially hide UI elements and set up tab buttons.
        ledgerUI.SetActive(false);
        InitializeTabs(); // Set up button listeners and initial state.
    }

    // --- Input Handling ---
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !DialogueManager.Instance.dialogueUI.activeInHierarchy)
        {
            ToggleLedgerUI();
        }
    }

    // --- UI Control ---
    public void ToggleLedgerUI()
    {
        bool newState = !ledgerUI.activeSelf;
        ledgerUI.SetActive(newState);

        if (newState)
        {
            Cursor.lockState = CursorLockMode.Confined;
            GameManager.Instance.StartCoroutine(GameManager.togglePlayerPause());
            SwitchTab(0); // Default to inventory (first tab).  Use index.
            inventoryManager.UpdateInventory(); // Initialize inventory on open.
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameManager.Instance.TogglePlayerControls();
        }
    }

    // Sets up tab button listeners and initial tab state.
    private void InitializeTabs()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            int tabIndex = i; // Capture the index in a local variable.
            tabs[i].onClick.AddListener(() => SwitchTab(tabIndex));
        }

        // Initially hide all content.
        inventoryContent.SetActive(false);
        characterContent.SetActive(false);
        craftingContent.SetActive(false);

        // Show the initial tab content (usually the inventory).
        SwitchTab(0); // Start with the first tab (inventory).
    }

    // Switch to a tab by its index.
    public void SwitchTab(int tabIndex)
    {
        // Hide all content.
        inventoryContent.SetActive(false);
        characterContent.SetActive(false);
        craftingContent.SetActive(false);

        // Show the selected tab's content.
        if (tabIndex == 0)
        {
            inventoryContent.SetActive(true);
            inventoryManager.UpdateInventory();
        }
        else if (tabIndex == 1)
        {
            characterContent.SetActive(true);
            equipmentManager.InitializeCharacterModel();
        }
        else if (tabIndex == 2)
        {
            craftingContent.SetActive(true);
            craftingManager.UpdateCraftingTab();
        }

        // Optionally, you might want to highlight the selected tab button here.
        // This would involve changing the button's colors or adding a visual indicator.
    }
}