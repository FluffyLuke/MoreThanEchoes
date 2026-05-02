using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour {
    public float walkSpeed = 10;
    public float runSpeed = 20;
    private CharacterController controller;
    private GameInput input;
    void Awake() {
        controller = GetComponent<CharacterController>();
        input = new GameInput();
    }

    void OnEnable() {
        input.Player.Enable();
    }

    void OnDisable() {
        input.Player.Disable();
    }

    void Update() {
        move();
    }

    private void move() {
        Vector2 moveBy = input.Player.Move.ReadValue<Vector2>();
        bool isSprinting = input.Player.Sprint.IsPressed();

        float speed = moveBy.x * Time.deltaTime;
        speed *= isSprinting ? runSpeed : walkSpeed;

        controller.Move(new(speed,0,0));
    }
}