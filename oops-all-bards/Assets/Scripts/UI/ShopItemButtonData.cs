using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButtonData : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image icon;
    public BaseItem item;
    public int cost;
    public Button button;

    public ShopManager shop;
    public BasePlayer player;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        icon = transform.Find("Icon").GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void PurchaseItem()
    {
        //on click for button
        player.Inventory.Add(item);
        player.Gold -= cost;
        shop.UpdateShop();

    }

    public void UpdateAvailability()
    {
        button.enabled = false;
        if (player.Gold >= cost)
        {
            button.enabled = true;
        }
    }
}
