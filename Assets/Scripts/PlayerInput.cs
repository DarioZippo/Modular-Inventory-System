using UnityEngine;
using UnityEngine.InputSystem;

/*
    This class handles all input from the new input system, which other classes can access.
*/
public class PlayerInput : MonoBehaviour{
    [Header("Character Input Values")]
	private Vector2 move;
    private bool isTriggeringInventory = false;

    public void OnMove(InputAction.CallbackContext context) {
        move = context.ReadValue<Vector2>();
    }

    public void OnInventory(InputAction.CallbackContext context) {
        if(context.performed){
            isTriggeringInventory = true;
        }
        else if(context.canceled){
            isTriggeringInventory = false;
        }
    }

    public Vector2 GetMoveInput(){
        return move;
    }

    public bool GetIsTriggeringInventory(){
        var result = isTriggeringInventory;
        isTriggeringInventory = false;
        return result;
    }
}
