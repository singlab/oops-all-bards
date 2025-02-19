using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftItemButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text; // Make this private and serialized
    [SerializeField] private Button button;   // Reference to the button component
    public BaseItem item { get; private set; } //  Public getter, private setter
    private InventoryItemManager manager;

    public void Initialize(BaseItem craftingItem, InventoryItemManager inventoryManager)
    {
        item = craftingItem;
        manager = inventoryManager;
        text.text = item.DisplayName;
        button.onClick.AddListener(OnCraftButtonClicked); // Use UnityEvents
    }

     private void OnCraftButtonClicked()
    {
        manager.UpdateCraftingTooltip(item); // Show tooltip on click
    }
}
