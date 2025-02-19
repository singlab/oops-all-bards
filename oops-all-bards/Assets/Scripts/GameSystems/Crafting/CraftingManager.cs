using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private LedgerManager ledgerManager; // Reference to the main manager.
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private GameObject recipeTooltip;
    [SerializeField] private GameObject recipeButtonPrefab; // Assign in Inspector

    private CraftTooltip recipeTooltipComponent;

    private void Awake()
    {
          if (ledgerManager == null)
        {
            ledgerManager = GetComponentInParent<LedgerManager>();
            if(ledgerManager == null)
            {
                 Debug.LogError("CraftingManager: LedgerManager not found in parent!");
            }
        }
    }

    public void Initialize()
    {
        if (recipeTooltip != null)
        {
             recipeTooltipComponent = recipeTooltip.GetComponent<CraftTooltip>();
             recipeTooltip.SetActive(false);
        } else
        {
            Debug.LogError("CraftingManager: recipeTooltip is not assigned in the Inspector!");
        }

    }

    public void UpdateCraftingTab()
    {
        // Clear existing recipe buttons.
        foreach (Transform child in recipeContainer)
        {
            Destroy(child.gameObject);
        }

        var availableRecipes = ItemData.items
            .Where(item => item.Recipe != null && item.Recipe.All(ingredient => DataManager.Instance.PlayerData.Inventory.Contains(ingredient)))
            .ToList();

        foreach (BaseItem item in availableRecipes)
        {
            GameObject buttonInstance = Instantiate(recipeButtonPrefab, recipeContainer);
            CraftItemButton buttonData = buttonInstance.GetComponent<CraftItemButton>();
            buttonData.Initialize(item, this); // Pass THIS manager.
        }

        if (recipeTooltip != null)
        {
            recipeTooltip.SetActive(false); // Ensure tooltip is hidden initially.
        }
    }

     public void UpdateCraftingTooltip(BaseItem item)
    {
        if (item == null)
        {
            recipeTooltip.SetActive(false);
            recipeTooltipComponent.ClearTooltip();
            return;
        }

        if (recipeTooltip != null)
        {
          recipeTooltip.SetActive(true);

            recipeTooltipComponent.Initialize(item, this); // Pass THIS manager

            //Update contents here
            recipeTooltipComponent.GetComponent<Image>().sprite = item.Icon;
            string recipeText = $"To Craft: {item.Description}\n\n";
            recipeText += string.Join(", ", item.Recipe.Select(recipeItem => recipeItem.DisplayName));
            recipeTooltipComponent.GetComponentInChildren<TMP_Text>().text = recipeText;
        }

    }

    public void CraftItem(BaseItem item)
    {
        // Check again if the player has the required ingredients.
        if (item.Recipe.All(ingredient => DataManager.Instance.PlayerData.Inventory.Contains(ingredient)))
        {
            // Remove ingredients
            foreach (BaseItem ingredient in item.Recipe)
            {
                DataManager.Instance.PlayerData.Inventory.Remove(DataManager.Instance.PlayerData.Inventory.First(invItem => invItem.Name == ingredient.Name));
            }

            // Add crafted item
            DataManager.Instance.PlayerData.Inventory.Add(item);

            // Update UI
            ledgerManager.inventoryManager.UpdateInventory(); // Update the main inventory.
            UpdateCraftingTab(); // Update the crafting tab to reflect changes.
        }
        else
        {
            // Optionally, display a message to the player: "Not enough ingredients!"
            Debug.Log("Not enough ingredients to craft " + item.DisplayName);
        }
    }
}