using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Inventory inventory = GameObject.FindObjectOfType<Inventory>();
            if (inventory != null)
            {
                inventory.AddItem(item);
                Destroy(gameObject);
            }

            if (item.itemName == "Bullets")
            {
                inventory.CountBullet += item.quantity;
                inventory.TextCountBullet.text = inventory.CountBullet.ToString();
            }
        }
    }
}
