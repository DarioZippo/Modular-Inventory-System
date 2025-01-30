using UnityEngine;

public class Player : MonoBehaviour{
    [SerializeField] private float moveSpeed = 5f;

    private PlayerInput playerInput;

    void Awake(){
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update() {
        transform.Translate(playerInput.GetMoveInput().x * moveSpeed * Time.deltaTime, 0, playerInput.GetMoveInput().y * moveSpeed * Time.deltaTime);
    }
}
