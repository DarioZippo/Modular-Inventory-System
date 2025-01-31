using Inventory;
using UnityEngine;

public class Player : MonoBehaviour{
    [SerializeField] private float moveSpeed = 5f;

    private bool isTimeStopped = false;

    private PlayerInput playerInput;
    private InventoryController inventory;

    private void Awake(){
        playerInput = GetComponent<PlayerInput>();
        inventory = GetComponent<InventoryController>();
    }

    private void OnEnable() {
        Messenger.AddListener(MessengerGameEvent.STOP_TIME, OnStopTime);
        Messenger.AddListener(MessengerGameEvent.START_TIME, OnStartTime);
    }

    private void OnDisable() {
        Messenger.RemoveListener(MessengerGameEvent.STOP_TIME, OnStopTime);
        Messenger.RemoveListener(MessengerGameEvent.START_TIME, OnStartTime);
    }

    private void OnStopTime(){
        isTimeStopped = true;
    }

    private void OnStartTime(){
        isTimeStopped = false;
    }

    private void Update() {
        if(!isTimeStopped)
            transform.Translate(playerInput.GetMoveInput().x * moveSpeed * Time.deltaTime, 0, playerInput.GetMoveInput().y * moveSpeed * Time.deltaTime);
    
        if(playerInput.GetIsTriggeringInventory()){
            inventory.TriggerUI();
        }
    }
}
