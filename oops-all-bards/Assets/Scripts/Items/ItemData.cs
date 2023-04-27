using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : BaseItem
{
    // went with the laziest implementation for now. Can easily make a list of items in a JSON file to be read
    // into the item data class in the future if needed

    string id;
    public string name;
    public ItemData[] recipe;
    public Sprite icon;

    public static ItemData stick = new ItemData("stick", "Stick", null, null);
    public static ItemData greenHerb = new ItemData("greenHerb", "Green Herb", null, null);
    public static ItemData biggerStick = new ItemData("biggerStick", "Bigger Stick", new ItemData[] {stick, stick}, null);

    static ItemData[] items = { stick, greenHerb, biggerStick };

    public ItemData(string id, string name, ItemData[] recipe, Sprite icon)
    {
        this.id = id;
        this.name = name;
        this.recipe = recipe;
        this.icon = icon;
    }

    public static ItemData GetItem(string id)
    {
        foreach (ItemData item in items)
        {
            if (id == item.id) return item;
        }
        Debug.Log("item not found");
        return null;
    }
}
