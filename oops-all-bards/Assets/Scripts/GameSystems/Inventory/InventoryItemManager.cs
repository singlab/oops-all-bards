using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemManager : MonoBehaviour
{
    // --- Constants ---
    private const int InventorySize = 32;

    // --- References ---
    [SerializeField] private LedgerManager ledgerManager; // Reference to the parent manager.

    // --- UI Elements ---
    [SerializeField] private GameObject inventoryTilePrefab; 
    [SerializeField] private Transform inventoryItemsContainer;
    [SerializeField] private Transform inventoryItems;
    [SerializeField] private TMP_Text inventoryCountText;

    // --- Internal Data ---
    private InventoryTile[] inventorySlots;


    public void Awake()
    {
         if (ledgerManager == null)
        {
            ledgerManager = GetComponentInParent<LedgerManager>();
            if (ledgerManager == null)
            {
               Debug.LogError("InventoryItemManager: LedgerManager not found in parent!");
            }
        }
    }

    public void Initialize()
    {
        InitializeInventoryGrid();
    }

    private void InitializeInventoryGrid()
    {
        inventorySlots = new InventoryTile[InventorySize];

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            GameObject tileInstance = Instantiate(inventoryTilePrefab, inventoryItems);
            tileInstance.name = $"InventoryTile_{i}";
            inventorySlots[i] = tileInstance.GetComponent<InventoryTile>();
            inventorySlots[i].Initialize(this); // Pass THIS manager to the tile.

            // Connect the InventoryTile's events.
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

    // Event Handlers
    public void UseItem(BaseItem item)
    {
        if (item.Type == BaseItem.ItemTypes.POTABLES) //  Simplified check
        {
            // potable.UseEffect(player);  // Game-specific logic.
            DataManager.Instance.PlayerData.Inventory.Remove(item);
            UpdateInventory();
        }
    }
    public void EquipItem(BaseItem item)
    {
        if (item.Type == BaseItem.ItemTypes.DUDS || item.Type == BaseItem.ItemTypes.INSTRUMENTS)
        {
            ledgerManager.equipmentManager.EquipItem(item);
            UpdateInventory();
        }
    }
    public void UnequipItem(BaseItem item)
    {
        if (item.Type == BaseItem.ItemTypes.DUDS || item.Type == BaseItem.ItemTypes.INSTRUMENTS)
        {
            ledgerManager.equipmentManager.UnequipItem(item);
            UpdateInventory();
        }
    }

    public void TrashItem(BaseItem item)
    {
        DataManager.Instance.PlayerData.Inventory.Remove(item);
        UpdateInventory();
    }

    // --- Inventory Display & Update ---

    private void UpdateInventoryCount()
    {
        inventoryCountText.text = $"{DataManager.Instance.PlayerData.Inventory.Count}/{InventorySize}";
    }

    public void DeselectAllTiles()
    {
        foreach (InventoryTile tile in inventorySlots)
        {
            tile.OnDeselect();
        }
    }
}