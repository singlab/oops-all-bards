using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftItemButton : MonoBehaviour
{
    public Image image;
    public TMP_Text text;
    private BaseItem item;
    private CraftingManager craftingManager; // Reference to the CraftingManager

    // Initialization method (called by CraftingManager)
    public void Initialize(BaseItem item, CraftingManager manager)
    {
        this.item = item;
        this.craftingManager = manager;  // Store the CraftingManager reference
        image.sprite = item.Icon;
        text.text = item.DisplayName;

        GetComponent<Button>().onClick.AddListener(OnCraftButtonClick); // Add the click listener
    }

    // Called when the button is clicked
    public void OnCraftButtonClick()
    {
        // Call the CraftItem method on the *CraftingManager*.
        craftingManager.CraftItem(item);

        // Optionally close/refresh the crafting UI here, if desired.  For example:
        // craftingManager.UpdateCraftingTab(); // Refresh the list of craftable items.
    }

    // Called when the mouse enters the button (for tooltip)
    public void OnPointerEnter()
    {
        if (craftingManager != null) //  Null check is good practice.
        {
            craftingManager.UpdateCraftingTooltip(item);
        }
    }

    // Called when the mouse exits the button (for tooltip)
    public void OnPointerExit()
    {
        //  You *could* hide the tooltip here, BUT it's usually better to let the
        //  CraftingManager handle that, in case another button is immediately hovered.
        //  craftingManager.HideTooltip(); //  Less desirable.

         if (craftingManager != null) // Check for null
        {
            craftingManager.UpdateCraftingTooltip(null); //By passing null, we tell it there is no item
        }
    }
}