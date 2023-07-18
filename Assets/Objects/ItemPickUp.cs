using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

    void Pickup()
    {
        InventoryManager.Instance.Add(item);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {

        GameObject player = GameObject.Find("Player");
        FieldOfView fov = player.GetComponent<FieldOfView>();
        Transform playerTransform = player.GetComponent<Transform>();
        Vector3 playerPosition = playerTransform.position;
        Vector3 itemPosition = transform.position;
        float distance = Vector3.Distance(playerPosition, itemPosition);
        if (distance <= fov.viewRadius)
        {
            Pickup();
        }
    }
}
