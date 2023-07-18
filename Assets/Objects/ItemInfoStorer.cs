using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoStorer : MonoBehaviour
{
    public Item Item;

    public void SetItem(Item item)
    {
        Item = item;
    }

    public Item GetItem()
    {
        return Item;
    }

    public void DeleteItem()
    {
        // On call, delete this item from the inventory
        InventoryManager.Instance.Remove(this.Item);
        // Destroy the game object
        Destroy(gameObject);
    }

    public void UseItem()
    {
        //Get gameObject named Player
        GameObject player = GameObject.Find("Player");
        //Get the SurvivalStatsManager component from the Player gameObject
        SurvivalStatsManager survivalStatsManager = player.GetComponent<SurvivalStatsManager>();
        survivalStatsManager.UseItem(Item);

        
        DeleteItem();
    }
}
