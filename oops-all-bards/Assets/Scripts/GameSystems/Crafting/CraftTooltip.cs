using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftTooltip : MonoBehaviour
{
    [SerializeField] public Image image;
    [SerializeField] public TMP_Text text; // Make this private and serialized
    [SerializeField] private Button button;   // Reference to the button component
    public BaseItem item { get; set; } //  Public getter, private setter
    public InventoryItemManager manager;

    public void Initialize(BaseItem craftingItem, InventoryItemManager inventoryManager)
    {
        item = craftingItem;
        manager = inventoryManager;
        button.onClick.AddListener(OnCraftButtonClicked); // Use UnityEvents
    }
     private void OnCraftButtonClicked()
    {
        CraftItem();
    }
    public void CraftItem()
    {
        foreach(BaseItem ingredient in item.Recipe)
        {
            DataManager.Instance.PlayerData.Inventory.Remove(ingredient);
        }
        DataManager.Instance.PlayerData.Inventory.Add(item);
        manager.UpdateInventory();
        manager.UpdateCraftingTab();
    }
}
