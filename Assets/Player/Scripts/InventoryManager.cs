using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    [SerializeField]
    public List<Item> ItemList = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(Item item)
    {
        ItemList.Add(item);
    }

    public void Remove(Item item)
    {
        ItemList.Remove(item);
    }

    public void ListItems()
    {
        foreach (Item item in ItemList)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemImage = obj.transform.Find("ItemImage").GetComponent<Image>();
            var itemInfoStorer = obj.GetComponent<ItemInfoStorer>();

            itemName.text = item.itemName;
            itemImage.sprite = item.icon;
            itemInfoStorer.SetItem(item);
        }
    }

    public void ClearList()
    {
        ItemList.Clear();
    }
}
