// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// public class InventoryTile : MonoBehaviour
// {
//     public BaseItem item;
//     public InventoryItemManager manager;

//     [SerializeField]
//     GameObject itemOptions;
//     GameObject use;
//     GameObject equip;
//     GameObject unequip;
//     GameObject trash;
//     TextMeshProUGUI text;

//     private void Awake()
//     {
//         use = itemOptions.transform.Find("Buttons").transform.Find("Use").gameObject;
//         equip = itemOptions.transform.Find("Buttons").transform.Find("Equip").gameObject;
//         unequip = itemOptions.transform.Find("Buttons").transform.Find("Unequip").gameObject;
//         trash = itemOptions.transform.Find("Buttons").transform.Find("Trash").gameObject;
//         text = itemOptions.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
//         itemOptions.SetActive(false);
//     }

//     public void OnClick()
//     {
//         if (itemOptions.activeSelf || item == null)
//         {
//             manager.DisableItemOptions();
//             return;
//         }

//         manager.DisableItemOptions();
//         itemOptions.SetActive(!itemOptions.activeSelf);

//         use.SetActive(true);
//         equip.SetActive(true);
//         trash.SetActive(true);
//         unequip.SetActive(false);

//         if (item.Type != BaseItem.ItemTypes.POTABLES)
//         {
//             use.SetActive(false);
//         }
//         if (item.Type != BaseItem.ItemTypes.INSTRUMENTS)
//         {
//             equip.SetActive(false);
//         }
//         if (DataManager.Instance.PlayerData.Equipment.Contains(item))
//         {
//             trash.SetActive(false);
//             equip.SetActive(false);
//             unequip.SetActive(true);
//         }

//         text.text = $"{item.DisplayName}\n{item.Description}";
//     }

//     public void OnDeselect()
//     {
//         if (!itemOptions.activeSelf) return;
//         itemOptions.SetActive(false);
//     }

//     public void Use()
//     {
//         // PotableItem.UseEffect
//         DataManager.Instance.PlayerData.Inventory.Remove(item);
//         manager.UpdateInventory();
//     }

//     public void Equip()
//     {
//         DataManager.Instance.PlayerData.Equipment.Add(item);
//         manager.UpdateInventory();
//     }

//     public void Trash()
//     {
//         DataManager.Instance.PlayerData.Inventory.Remove(item);
//         manager.UpdateInventory();
//     }

//     public void Unequip()
//     {
//         // unequip item effect here
//         DataManager.Instance.PlayerData.Equipment.Remove(item);
//         manager.UpdateInventory();
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class InventoryTile : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image frame;
    [SerializeField] private Button tileButton; // The button that covers the entire tile

    [SerializeField] private GameObject itemOptions;
    [SerializeField] private Button useButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button trashButton;
    [SerializeField] private TextMeshProUGUI descriptionText;

    // UnityEvents for actions.  These are set up in the Inspector.
    [System.Serializable] public class ItemEvent : UnityEvent<BaseItem> { }
    public ItemEvent OnUse = new ItemEvent();
    public ItemEvent OnEquip = new ItemEvent();
    public ItemEvent OnUnequip = new ItemEvent();
    public ItemEvent OnTrash = new ItemEvent();

    public BaseItem item { get; private set; }
    private InventoryItemManager manager;

    public void Initialize(InventoryItemManager inventoryManager)
    {
        manager = inventoryManager;
        //itemOptions.SetActive(false); // Hide options initially.  Do this *after* finding the buttons.

        // Connect the tile's main button to show the options menu.
        tileButton.onClick.AddListener(OnTileClicked);

        // Connect the option buttons to their respective events.
        useButton.onClick.AddListener(() => OnUse.Invoke(item));
        equipButton.onClick.AddListener(() => OnEquip.Invoke(item));
        unequipButton.onClick.AddListener(() => OnUnequip.Invoke(item));
        trashButton.onClick.AddListener(() => OnTrash.Invoke(item));

        //Hide itemOptions after everything is connected up
        itemOptions.SetActive(false);
    }

    public void SetItem(BaseItem newItem)
    {
        item = newItem;
        itemIcon.sprite = item.Icon;
        itemIcon.enabled = true;
        frame.color = Color.white; // Reset frame color
    }

    public void ClearItem()
    {
        if (item == null) return; // No item to clear
        
        item = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        frame.color = Color.white;
    }

    public void OnDeselect()
    {
        itemOptions.SetActive(false);
    }

    private void OnTileClicked()
    {
        if (item == null) return;  // No item, no options.

        manager.DeselectAllTiles(); // Close other open menus
        itemOptions.SetActive(!itemOptions.activeSelf); // Toggle the options menu

        // Configure which buttons are visible based on the item and its state.
        ConfigureOptionButtons();
    }

    private void ConfigureOptionButtons()
    {
        if (item == null)
        {
            itemOptions.SetActive(false);
            return;
        }

        useButton.gameObject.SetActive(item.Type is BaseItem.ItemTypes.POTABLES); // Example:  Only show "Use" for Potables
        equipButton.gameObject.SetActive(item.Type is BaseItem.ItemTypes.DUDS);
        equipButton.gameObject.SetActive(item.Type is BaseItem.ItemTypes.INSTRUMENTS); // Example:  Only show "Equip" for Instruments or Duds
        unequipButton.gameObject.SetActive(DataManager.Instance.PlayerData.Equipment.Contains(item)); // Show if equipped
        trashButton.gameObject.SetActive(!DataManager.Instance.PlayerData.Equipment.Contains(item)); //Don't show if equipped
        descriptionText.text = $"{item.DisplayName}\n{item.Description}";
    }

     //Added to change the color of the Frame to show that it has been equipped
    public void UpdateFrameColor()
    {
        if (DataManager.Instance.PlayerData.Equipment.Contains(item))
        {
            frame.color = Color.red;  // Or any visual indication you prefer
        }
        else
        {
            frame.color = Color.white; // Reset to default
        }
    }
}
