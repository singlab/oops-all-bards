using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButtonData : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image icon;
    public ItemData item;
    public int cost;
    public Button button;

    public ShopManager shop;
    public BasePlayer player;

    // Start is called before the first frame update
    private void Start()
    {

    }

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        icon = GetComponentInChildren<Image>();
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
