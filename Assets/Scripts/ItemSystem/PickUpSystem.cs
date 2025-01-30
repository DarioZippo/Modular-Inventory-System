using Inventory;
using Inventory.Model;
using UnityEngine;

public class PickUpSystem : MonoBehaviour{
    private InventoryController inventory;

    private void Awake() {
        inventory = GetComponent<InventoryController>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUpItem item = other.GetComponent<PickUpItem>();
        inventory.HandlePickUp(item);
    }
}
