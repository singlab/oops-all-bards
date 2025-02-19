using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftTooltip : MonoBehaviour
{
    [SerializeField] private Image image;  // Keep these private, accessed via methods if needed.
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button craftButton;  // Rename for clarity: craftButton

    private BaseItem item; //  Private now, no need for external access.
    private CraftingManager craftingManager; // Use CraftingManager, not InventoryItemManager

    // Initialization (called by CraftingManager)
    public void Initialize(BaseItem craftingItem, CraftingManager manager)
    {
        item = craftingItem;
        craftingManager = manager;

        if (craftButton != null) //  Null check! Important for UI elements.
        {
            craftButton.onClick.AddListener(OnCraftButtonClicked);
        }
        else
        {
            Debug.LogError("CraftTooltip: craftButton is not assigned in the Inspector!");
        }

        // You could also set the image and text here if you want the tooltip
        // to immediately reflect the item, even before it's made visible.  This
        // can be useful to avoid a one-frame flicker of old data.  It depends
        // on how you want the tooltip to behave.  For example:
        //
        // if (craftingItem != null) {
        //     image.sprite = craftingItem.Icon;
        //     string recipeText = $"To Craft: {craftingItem.Description}\n\n";
        //     recipeText += string.Join(", ", craftingItem.Recipe.Select(recipeItem => recipeItem.DisplayName));
        //     text.text = recipeText;
        // } else {
        //     // Clear the tooltip if craftingItem is null.  Important!
        //     image.sprite = null;
        //     text.text = "";
        // }
    }

    private void OnCraftButtonClicked()
    {
        if (craftingManager != null) //  Good practice to check.
        {
            craftingManager.CraftItem(item);
        }
    }

    public Image GetImage() { return image; }
    public TMP_Text GetText() { return text; }
    
    //IMPORTANT: Add a function to clear out text if tooltip is to be hidden
    public void ClearTooltip()
    {
        image.sprite = null;
        text.text = "";
    }
}