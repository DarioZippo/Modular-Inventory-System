using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory{
    /*
        The "Controller" in the MVC pattern for inventory implemantation
    */
    public class InventoryController : MonoBehaviour{

        [SerializeField]
        private UIInventoryPage inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        [SerializeField]
        GameObject player;

        [SerializeField]
        private AudioClip dropClip;

        [SerializeField]
        private AudioSource audioSource;

        bool isInitializing = true;

        private void Start(){
            player = GetComponent<Player>().gameObject;
            InitializeUI();
        }

        public void AttachNeededObjects(){
            inventoryUI = GameObject.FindGameObjectWithTag("InventoryMenu").GetComponent<UIInventoryPage>();
        }

        public void InitializeUI()
        {
            inventoryUI.Hide();

            PrepareUI();

            if(isInitializing){
                isInitializing = false;
                PrepareInventoryData();
            }
            else{
                InventoryUpdate(inventoryData.GetCurrentInventoryState());
            }
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += InventoryUpdate;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void InventoryUpdate(Dictionary<int, InventoryItem> dictionary)
        {
            UpdateInventoryUI(dictionary);
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();

            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, 
                    item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            bool itemActionShown = false;
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if(itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);
                itemActionShown = true;

                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                if(!itemActionShown){
                    inventoryUI.ShowItemAction(itemIndex);
                }
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.quantity));
            }

        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }   

        public void HandlePickUp(PickUpItem item)
        {
            if (item != null)
            {
                int reminder = inventoryData.AddItem(item.inventoryItem, item.quantity);
                if (reminder == 0)
                    item.DestroyItem();
                else
                    item.quantity = reminder;
            }
        }

        public void HandlePickUp(InventoryItem item)
        {
            if (item.quantity != 0){
                inventoryData.AddItem(item);
            }
        }
        
        public void PerformAction(int itemIndex){
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(player, inventoryItem.itemState);
                audioSource.PlayOneShot(itemAction.actionSFX);
                if (inventoryData.GetItemAt(itemIndex).IsEmpty){
                    inventoryUI.ResetSelection();
                }
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            /*
            Debug.Log("Sto swappando " + inventoryData.GetItemAt(itemIndex_1) + " con "
                + inventoryData.GetItemAt(itemIndex_2));
            */
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage,
                item.name, description);
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void TriggerUI(){
            if (inventoryUI.isActiveAndEnabled == false){
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key,
                        item.Value.item.ItemImage,
                        item.Value.quantity);
                }

                Messenger.Broadcast(MessengerGameEvent.STOP_TIME);
            }
            else{
                inventoryUI.Hide();

                Messenger.Broadcast(MessengerGameEvent.START_TIME);
            }
        }

        public InventorySO GetInventoryData(){
            return inventoryData;
        }

        public bool Contains(ItemSO item){
            return inventoryData.Contains(item);
        }
    }
}