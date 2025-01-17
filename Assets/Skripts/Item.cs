using System.IO;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string itemName; 
    public Sprite icon;
    public int quantity; 

    public Item(int id, string name, Sprite icon, int quantity)
    {
        this.ID = id;
        this.itemName = name;
        this.icon = icon;
        this.quantity = quantity;
    }

    public static Item CreateFromData(int id, string name, Sprite icon, int quantity)
    {
        Item item = new Item( id, name, icon, quantity);
        item.ID = id;
        item.itemName = name;
        item.icon = icon;
        item.quantity = quantity;

        return item;
    }
}
