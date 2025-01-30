using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO inventoryItem;
    public int quantity = 1;
    public AudioSource audioSource;
    public float duration = 0.3f;
}
