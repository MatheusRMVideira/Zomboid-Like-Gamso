using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName="Inventory/CreateItem")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int itemWeight;
    public Sprite icon;
    public float health;
    public float hunger;
    public float thirst;
    public float sleep;
    public float sanity;
}
