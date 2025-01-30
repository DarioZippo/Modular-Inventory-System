using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour{
    [Header("Character Input Values")]
	private Vector2 move;

    public void OnMove(InputAction.CallbackContext context) {
        move = context.ReadValue<Vector2>();
    }

    public Vector2 GetMoveInput(){
        return move;
    }
}
