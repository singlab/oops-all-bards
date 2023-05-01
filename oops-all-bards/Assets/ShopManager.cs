using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    GameObject shopButton;
    ScrollRect scrollRect;

    ShopInteractable shop;
    List<ShopItemButtonData> shopButtons = new List<ShopItemButtonData>();

    Transform panel;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        shop = GetComponentInParent<ShopInteractable>();
        PopulateShop();
        anim = GetComponent<Animator>();
        Debug.Log($"canvas {GameObject.Find("Canvas")}");
        Debug.Log($"panel {GameObject.Find("Canvas").transform.Find("Panel")}");
        panel = GameObject.Find("Canvas").transform.Find("Panel");
    }

    public void PopulateShop()
    {
        foreach (ShopItem item in shop.shop)
        {
            ShopItemButtonData obj = Instantiate(shopButton, scrollRect.content).GetComponent<ShopItemButtonData>();

            obj.player = DataManager.Instance.PlayerData;
            obj.shop = this;
            BaseItem itemData = BaseItem.GetItem(item.name);
            obj.text.text = $"{itemData.DisplayName} - {item.cost}";
            if (itemData.Icon != null)
            {
                obj.icon.sprite = itemData.Icon;
            }
            obj.item = itemData;
            obj.cost = item.cost;
            shopButtons.Add(obj);
            obj.UpdateAvailability();
        }
    }

    public void DisplayShop()
    {
        Cursor.lockState = CursorLockMode.Confined;
        transform.SetParent(panel, false);
    }

    public void CloseShop()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.SetParent(shop.transform, false);
    }

    public void UpdateShop()
    {
        foreach(ShopItemButtonData button in shopButtons)
        {
            button.UpdateAvailability();   
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
